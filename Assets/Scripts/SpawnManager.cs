using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class SpawnManager : MonoBehaviour
{
  #region Serialized Fields
  [SerializeField] private Transform screen_ui = null;
  [SerializeField] private Transform screen_3d = null;
  [SerializeField] private QuadContentController quad_prefab = null;
  [SerializeField] private ConectorController conector_prefab = null;
  [SerializeField] private ScreenMainUI screen_main_ui = null;
  [SerializeField] private ScreenLevelsUI screen_levels_ui = null;
  [SerializeField] private ScreenLevelUI screen_level_ui = null;
  [SerializeField] private ScreenWinUI screen_win_ui = null;
  [SerializeField] private ScreenLevel3D screen_level_3d = null;
  [SerializeField] private ScreenMain3D screen_main_3d = null;
  [SerializeField] private ScreenLevels3D screen_levels_3d = null;
  [SerializeField] private StartFinishPoint start_finish_point = null;
  #endregion

  #region Private Fields

  private SinglePool<ScreenMainUI> screen_main_ui_pool = new SinglePool<ScreenMainUI>();
  private SinglePool<ScreenLevelsUI> screen_levels_ui_pool = new SinglePool<ScreenLevelsUI>();
  private SinglePool<ScreenLevelUI> screen_level_ui_pool = new SinglePool<ScreenLevelUI>();
  private SinglePool<ScreenWinUI> screen_win_ui_pool = new SinglePool<ScreenWinUI>();
  private SinglePool<ScreenLevel3D> screen_level_3d_pool = new SinglePool<ScreenLevel3D>();
  private SinglePool<ScreenMain3D> screen_main_3d_pool = new SinglePool<ScreenMain3D>();
  private SinglePool<ScreenLevels3D> screen_levels_3d_pool = new SinglePool<ScreenLevels3D>();

  private MultiPool<QuadContentController> quads_pool = new MultiPool<QuadContentController>();
  private MultiPool<ConectorController> conectors_pool = new MultiPool<ConectorController>();
  private MultiPool<StartFinishPoint> start_finish_points_pool = new MultiPool<StartFinishPoint>();
  #endregion


  #region Public Methods
  public QuadContentController spawnQuad( Vector3 position, Transform parent_transform )
  {
    return quads_pool.spawn( quad_prefab, position, Quaternion.identity, parent_transform );
  }

  public StartFinishPoint spawnStartFinishPoint( Vector3 position, Quaternion rotation, Transform parent_transform )
  {
    return start_finish_points_pool.spawn( start_finish_point, position, rotation, parent_transform );
  }

  public ConectorController spawnConector( Transform root_transform )
  {
    return conectors_pool.spawn( conector_prefab, root_transform );
  }

  public ScreenMainUI spawnScreenMainUI()
  {
    return screen_main_ui_pool.spawn( screen_main_ui, screen_ui );
  }

  public ScreenLevelsUI spawnScreenLevelsUI()
  {
    return screen_levels_ui_pool.spawn( screen_levels_ui, screen_ui );
  }

  public ScreenLevelUI spawnScreenLevelUI()
  {
    return screen_level_ui_pool.spawn( screen_level_ui, screen_ui );
  }

  public ScreenWinUI spawnScreenWinUI()
  {
    return screen_win_ui_pool.spawn( screen_win_ui, screen_ui );
  }

  public ScreenLevel3D spawnScreenLevel3D()
  {
    return screen_level_3d_pool.spawn( screen_level_3d, screen_3d );
  }

  public ScreenMain3D spawnScreenMain3D()
  {
    return screen_main_3d_pool.spawn( screen_main_3d, screen_3d );
  }

  public ScreenLevels3D spawnScreenLevels3D()
  {
    return screen_levels_3d_pool.spawn( screen_levels_3d, screen_3d );
  }

  public void despawnScreenMainUI()
  {
    screen_main_ui_pool.despawn();
  }

  public void despawnScreenLevelsUI()
  {
    screen_levels_ui_pool.despawn();
  }

  public void despawnScreenLevelUI()
  {
    screen_level_ui_pool.despawn();
  }

  public void despawnScreenWinUI()
  {
    screen_win_ui_pool.despawn();
  }

  public void despawnScreenLevel3D()
  {
    screen_level_3d_pool.despawn();
  }

  public void despawnScreenMain3D()
  {
    screen_main_3d_pool.despawn();
  }

  public void despawnScreenLevels3D()
  {
    screen_levels_3d_pool.despawn();
  }

  public void despawnAllQuads()
  {
    quads_pool.despawnAll();
  }

  public void despawnAllStartFinishPoints()
  {
    start_finish_points_pool.despawnAll();
  }

  public void despawnAllConectors()
  {
    conectors_pool.despawnAll();
  }
  #endregion
}

public class SinglePool<T> where T : MonoBehaviourPoolable
{
  T instance = null;

  public T spawn( T prefab, Vector3 position, Quaternion rotation, Transform parent_transform )
  {
    if ( instance == null )
      instance = MonoBehaviour.Instantiate( prefab, position, rotation, parent_transform );

    instance.onSpawn();

    return instance;
  }

  public T spawn( T prefab, Transform parent_transform )
  {
    if ( instance == null )
      instance = MonoBehaviour.Instantiate( prefab, parent_transform );

    instance.onSpawn();

    return instance;
  }

  public void despawn()
  {
    instance.onDespawn();
  }
}

public class MultiPool<T> where T : MonoBehaviourPoolable
{
  private List<T> instances = new List<T>();

  public T spawn( T prefab, Transform parent_transform )
  {
    T instance = checkForDespawned();

    if ( instance != null )
    {
      Debug.LogError( "Spawned from pool" );
      instance.transform.SetParent( parent_transform );
      instance.onSpawn();
      return instance;
    }

    instance = MonoBehaviour.Instantiate( prefab, parent_transform );
    instances.Add( instance );
    instance.onSpawn();
    Debug.LogError( "Spawned from new" );
    return instance;
  }

  public T spawn( T prefab, Vector3 position, Quaternion rotation, Transform parent_transform )
  {
    T instance = checkForDespawned();

    if ( instance != null )
    {
      Debug.LogError( "Spawned from pool" );
      instance.transform.SetParent( parent_transform );
      instance.transform.position = position;
      instance.transform.rotation = rotation;
      instance.onSpawn();
      return instance;
    }

    instance = MonoBehaviour.Instantiate( prefab, position, rotation, parent_transform );
    instances.Add( instance );
    instance.onSpawn();
    Debug.LogError( "Spawned from new" );
    return instance;
  }

  public void despawnAll()
  {
    foreach( T instance in instances )
      instance.onDespawn();
  }

  private T checkForDespawned()
  {
    T instance = null;

    if ( instances.Any( x => x.isAvailuableToSpawn() ) )
      instance = instances.First( x => x.isAvailuableToSpawn() );

    if ( instance == null )
      return null;

    //instance.transform.SetParent( parent_transform );
    //instance.transform.position = position;
    //instance.transform.rotation = rotation;
    instance.onSpawn();
    return instance;

  }
}
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class SpawnManager : MonoBehaviour
{
  #region Serialized Fields
  [SerializeField] private Transform screen_ui = null;
  [SerializeField] private Transform screen_3d = null;
  [SerializeField] private QuadContentController quad_prefab = null;
  [SerializeField] private ConectorController[] conector_prefabs = null;
  [SerializeField] private QuadSetUpController quad_setup_prefab = null;
  [SerializeField] private ScreenMainUI screen_main_ui = null;
  [SerializeField] private ScreenLevelsUI screen_levels_ui = null;
  [SerializeField] private ScreenLevelUI screen_level_ui = null;
  [SerializeField] private ScreenWinUI screen_win_ui = null;
  [SerializeField] private ScreenLevel3D screen_level_3d = null;
  [SerializeField] private ScreenMain3D screen_main_3d = null;
  #endregion

  #region Private Fields

  private SinglePool<ScreenMainUI> screen_main_ui_pool = new SinglePool<ScreenMainUI>();
  private SinglePool<ScreenLevelsUI> screen_levels_ui_pool = new SinglePool<ScreenLevelsUI>();
  private SinglePool<ScreenLevelUI> screen_level_ui_pool = new SinglePool<ScreenLevelUI>();
  private SinglePool<ScreenWinUI> screen_win_ui_pool = new SinglePool<ScreenWinUI>();
  private SinglePool<ScreenLevel3D> screen_level_3d_pool = new SinglePool<ScreenLevel3D>();
  private SinglePool<ScreenMain3D> screen_main_3d_pool = new SinglePool<ScreenMain3D>();
  #endregion


  #region Public Methods
  public QuadContentController spawnQuad( Vector3 position, Transform parent_transform )
  {
    return Instantiate( quad_prefab, position, Quaternion.identity, parent_transform );
  }

  public QuadSetUpController spawnQuadSetUp( Vector3 position )
  {
    return Instantiate( quad_setup_prefab, position, Quaternion.identity );
  }

  public ConectorController spawnConector( Transform root_transform, QuadConectionType conection_type )
  {
    ConectorController conector_controller = conector_prefabs.FirstOrDefault( x => x.conectionType == conection_type );
    return Instantiate( conector_controller, root_transform );
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

  public T spawn( T prefab, Vector3 position, Quaternion rotation, Transform parent_transform )
  {
    T instance = instances.First( x => x.isAvailuableToSpawn() );

    if ( instance != null )
      return instance;

    instance = MonoBehaviour.Instantiate( prefab, parent_transform );
    instances.Add( instance );
    instance.onSpawn();
    return instance;
  }

  public void despawnAll()
  {
    foreach( T instance in instances )
      instance.onDespawn();
  }
}
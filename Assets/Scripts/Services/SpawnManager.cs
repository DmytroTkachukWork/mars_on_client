using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class SpawnManager : MonoBehaviourService<SpawnManager>
{
  #region Serialized Fields
  [SerializeField] private Transform screen_ui = null;
  [SerializeField] private Transform screen_3d = null;
  [SerializeField] private QuadContentController quad_prefab = null;
  [SerializeField] private QuadContentController start_point = null;
  [SerializeField] private QuadContentController finish_point = null;
  [SerializeField] private ConectorController conector_prefab = null;
  [SerializeField] private WaterFallController waterfall_controller = null;
  [SerializeField] private ScreenMainUI screen_main_ui = null;
  [SerializeField] private ScreenLibraryUI screen_library_ui = null;
  [SerializeField] private ScreenCardUI screen_card_ui = null;
  [SerializeField] private ScreenSectorUI screen_sector_ui = null;
  [SerializeField] private ScreenLevelUI screen_level_ui = null;
  [SerializeField] private ScreenWinUI screen_win_ui = null;
  [SerializeField] private ScreenLoseUI screen_lose_ui = null;
  [SerializeField] private ScreenPauseUI screen_pause_ui = null;
  [SerializeField] private SectorInfoUI sector_info_ui = null;
  [SerializeField] private LevelInfoUI level_info_ui = null;
  
  [SerializeField] private PlanetController planet_controller = null;
  [SerializeField] private ResourceEntityController resource_entity_controller = null;
  #endregion

  #region Private Fields

  private SinglePool<ScreenMainUI> screen_main_ui_pool = new SinglePool<ScreenMainUI>();
  private SinglePool<ScreenLibraryUI> screen_library_ui_pool = new SinglePool<ScreenLibraryUI>();
  private SinglePool<ScreenCardUI> screen_card_ui_pool = new SinglePool<ScreenCardUI>();
  private SinglePool<ScreenSectorUI> screen_sector_ui_pool = new SinglePool<ScreenSectorUI>();
  private SinglePool<ScreenLevelUI> screen_level_ui_pool = new SinglePool<ScreenLevelUI>();
  private SinglePool<ScreenWinUI> screen_win_ui_pool = new SinglePool<ScreenWinUI>();
  private SinglePool<ScreenLoseUI> screen_lose_ui_pool = new SinglePool<ScreenLoseUI>();
  private SinglePool<ScreenPauseUI> screen_pause_ui_pool = new SinglePool<ScreenPauseUI>();
  private SinglePool<PlanetController> planet_controller_pool = new SinglePool<PlanetController>();

  private MultiPool<SectorInfoUI> sector_info_ui_pool = new MultiPool<SectorInfoUI>();
  private MultiPool<LevelInfoUI> level_info_ui_pool = new MultiPool<LevelInfoUI>();
  private MultiPool<QuadContentController> quads_pool = new MultiPool<QuadContentController>();
  private MultiPool<ConectorController> conectors_pool = new MultiPool<ConectorController>();
  private MultiPool<WaterFallController> waterfall_controller_pool = new MultiPool<WaterFallController>();
  private MultiPool<QuadContentController> start_points_pool = new MultiPool<QuadContentController>();
  private MultiPool<QuadContentController> finish_points_pool = new MultiPool<QuadContentController>();
  private MultiPool<ResourceEntityController> resource_entity_controller_pool = new MultiPool<ResourceEntityController>();
  #endregion


  #region Public Methods
  public QuadContentController spawnQuad( Vector3 position, Transform parent_transform, QuadRoleType role_type )
  {
    switch( role_type )
    {
    case QuadRoleType.PLAYABLE: return quads_pool.spawn( quad_prefab, parent_transform );
    case QuadRoleType.STARTER:  return start_points_pool.spawn( start_point, parent_transform );
    case QuadRoleType.FINISHER: return finish_points_pool.spawn( finish_point, parent_transform );
    }
    return null;
  }

  public ConectorController spawnConector( Transform root_transform )
  {
    return conectors_pool.spawn( conector_prefab, root_transform );
  }

  public WaterFallController spawnWaterFall( Transform position, Quaternion rotation )
  {
    WaterFallController spawned_waterfall = waterfall_controller_pool.spawn( waterfall_controller, position );
    spawned_waterfall.transform.localRotation = rotation;
    return spawned_waterfall;
  }

  public PlanetController spawnPlanet()
  {
    return planet_controller_pool.spawn( planet_controller, screen_3d );
  }

  public ResourceEntityController spawnRec( Transform root_transform )
  {
    return resource_entity_controller_pool.spawn( resource_entity_controller, root_transform );
  }

  public SectorInfoUI getOrSpawnSectorInfoUI( SectorController sector )
  {
    SectorInfoUI inst = sector_info_ui_pool.spawn( sector_info_ui, screen_ui );
    inst.init( sector );
    return inst;
  }

  public LevelInfoUI getOrSpawnLevelInfoUI( LevelController level )
  {
    LevelInfoUI inst = level_info_ui_pool.spawn( level_info_ui, screen_ui );
    inst.init( level );
    return inst;
  }

  public ScreenBaseUI getOrSpawnScreenUI( ScreenUIId screen_id )
  {
    switch( screen_id )
    {
    case ScreenUIId.MAIN:        return screen_main_ui_pool.spawn( screen_main_ui, screen_ui );
    case ScreenUIId.SECTOR:      return screen_sector_ui_pool.spawn( screen_sector_ui, screen_ui );
    case ScreenUIId.LEVEL:       return screen_level_ui_pool.spawn( screen_level_ui, screen_ui );
    case ScreenUIId.LEVEL_WIN:   return screen_win_ui_pool.spawn( screen_win_ui, screen_ui );
    case ScreenUIId.LEVEL_LOSE:  return screen_lose_ui_pool.spawn( screen_lose_ui, screen_ui );
    case ScreenUIId.LEVEL_PAUSE: return screen_pause_ui_pool.spawn( screen_pause_ui, screen_ui );
    case ScreenUIId.LIBRARY:     return screen_library_ui_pool.spawn( screen_library_ui, screen_ui );
    case ScreenUIId.CARD:        return screen_card_ui_pool.spawn( screen_card_ui, screen_ui );
    default:                     return null;
    }
  }

  public void despawnScreenUI( ScreenUIId screen_id )
  {
    switch( screen_id )
    {
    case ScreenUIId.MAIN:        screen_main_ui_pool.despawn(); break;
    case ScreenUIId.SECTOR:      screen_sector_ui_pool.despawn(); break;
    case ScreenUIId.LEVEL:       screen_level_ui_pool.despawn(); break;
    case ScreenUIId.LEVEL_WIN:   screen_win_ui_pool.despawn(); break;
    case ScreenUIId.LEVEL_LOSE:  screen_lose_ui_pool.despawn(); break;
    case ScreenUIId.LEVEL_PAUSE: screen_pause_ui_pool.despawn(); break;
    case ScreenUIId.LIBRARY:     screen_library_ui_pool.despawn(); break;
    case ScreenUIId.CARD:        screen_card_ui_pool.despawn(); break;
    }
  }

  public void despawnAllSectorInfoUI()
  {
    sector_info_ui_pool.despawnAll();
  }

  public void despawnAllLevelInfoUI()
  {
    level_info_ui_pool.despawnAll();
  }

  public void despawnAllQuads()
  {
    quads_pool.despawnAll();
    start_points_pool.despawnAll();
    finish_points_pool.despawnAll();
  }

  public void despawnAllConectors()
  {
    conectors_pool.despawnAll();
  }

  public void despawnPlanet()
  {
    planet_controller_pool.despawn();
  }

  public void despawnAllWaterfalls()
  {
    waterfall_controller_pool.despawnAll();
  }
  #endregion
}

public class SinglePool<T> where T : MonoBehaviourPoolable
{
  private T instance = null;

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

    if ( instance.isAvailuableToSpawn() )
      instance.onSpawn();

    return instance;
  }

  public void despawn()
  {
    instance?.onDespawn();
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
      instance.transform.SetParent( parent_transform );
      instance.transform.localPosition = Vector3.zero;
      instance.transform.localRotation = Quaternion.identity;

      instance.onSpawn();

      return instance;
    }

    instance = MonoBehaviour.Instantiate( prefab, parent_transform );
    instances.Add( instance );
    instance.onSpawn();

    return instance;
  }

  public T spawn( T prefab, Vector3 position, Quaternion rotation, Transform parent_transform )
  {
    T instance = checkForDespawned();

    if ( instance != null )
    {
      instance.transform.SetParent( parent_transform );
      instance.transform.localPosition = position;
      instance.transform.localRotation = rotation;
      instance.onSpawn();

      return instance;
    }

    instance = MonoBehaviour.Instantiate( prefab, position, rotation, parent_transform );
    instances.Add( instance );
    instance.onSpawn();

    return instance;
  }

  public void despawnAll()
  {
    foreach( T instance in instances )
      instance?.onDespawn();
  }

  private T checkForDespawned()
  {
    T instance = null;

    if ( instances.Any( x => x.isAvailuableToSpawn() ) )
      instance = instances.First( x => x.isAvailuableToSpawn() );

    return instance;
  }
}
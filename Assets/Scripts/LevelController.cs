using UnityEngine;

public class LevelController : MonoBehaviourBase
{
  #region Serialized Fields
  [SerializeField] private GameObject sector_level_content = null;
  [SerializeField] private GameObject level_content = null;
  [SerializeField] private ClickableBase3D clickable_base = null;
  [SerializeField] private LevelCameraContainerController camera_container = null;
  [SerializeField] private GameObject smoke = null;

  [SerializeField] private FieldManager field_manager = null;
  [SerializeField] private int level_id = 0;
  [SerializeField] private int sector_id = 0;
  #endregion

  #region Public Fields
  public LevelCameraContainerController cameraContainer => camera_container;
  #endregion


  #region Public Methods
  public void startShowClose()
  {
    clickable_base.onClick -= moveToLevel;
    clickable_base.gameObject.SetActive( false );

    level_content.SetActive( true );
    sector_level_content.SetActive( true );

    initLevel();
  }

  public void finishShowClose()
  {
    camera_container.init();
    sector_level_content.SetActive( false );
  }

  public void startShowFar()
  {
    level_content.SetActive( true );
    sector_level_content.SetActive( true );

    camera_container.deinit();
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL );

    smoke.SetActive( !playerDataManager.hasAccessToLevel( sector_id, level_id ) );
  }

  public void finishShowFar()
  {
    clickable_base.gameObject.SetActive( true );
    clickable_base.onClick -= moveToLevel;
    clickable_base.onClick += moveToLevel;

    level_content.SetActive( false );
    sector_level_content.SetActive( true );

    camera_container.deinit();
    field_manager.deinit();
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL );
  }

  public void hide()
  {
    clickable_base.onClick -= moveToLevel;
    clickable_base.gameObject.SetActive( false );

    level_content.SetActive( false );
    sector_level_content.SetActive( false );

    camera_container.deinit();
    field_manager.deinit();
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL );
    smoke.SetActive( false );
  }

  public void moveToLevel()
  {
    if ( !playerDataManager.hasAccessToLevel( sector_id, level_id ) && !playerDataManager.isCurentLevel( sector_id, level_id ) )
      return;

    cameraController.moveCameraToLevel( this );
  }

  public void initLevel()
  {
    field_manager.init( levelsHolder.getLevel( sector_id, level_id ) );
    spawnManager.despawnScreenUI( ScreenUIId.SECTOR );
    (spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL ) as ScreenLevelUI ).init();
  }
  #endregion
}

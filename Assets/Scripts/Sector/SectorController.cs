using UnityEngine;

public class SectorController : MonoBehaviourBase
{
  #region Serialized Fields
  [SerializeField] private PlanetSectorContentController   planet_sector_content = null;
  [SerializeField] private GameObject                      sector_content        = null;
  [SerializeField] private ClickableBase3D                 clickable_base        = null;
  [SerializeField] private SectorCameraContainerController camera_container      = null;
  [SerializeField] private LevelController[]               level_controllers     = null;
  [SerializeField] private int                             sector_id             = 0;
  #endregion

  #region Public Fields
  public SectorCameraContainerController cameraContainer => camera_container;
  #endregion


  #region Public Methods
  public void startShowClose()
  {
    clickable_base.onClick -= moveToSector;
    clickable_base.gameObject.SetActive( false );

    sector_content.SetActive( true );

    foreach( LevelController level in level_controllers )
      level.startShowFar();

    spawnManager.despawnScreenUI( ScreenUIId.SECTOR );
  }

  public void finishShowClose()
  {
    clickable_base.onClick -= moveToSector;
    clickable_base.gameObject.SetActive( false );

    planet_sector_content.deinit();
    sector_content.SetActive( true );

    foreach( LevelController level in level_controllers )
      level.finishShowFar();

    camera_container.init();
    spawnManager.getOrSpawnScreenUI( ScreenUIId.SECTOR );
  }

  public void startShowFar()
  {
    sector_content.SetActive( true );
    planet_sector_content.init();

    camera_container.deinit();
    spawnManager.despawnScreenUI( ScreenUIId.SECTOR );
  }
  
  public void finishShowFar()
  {
    clickable_base.gameObject.SetActive( true );
    clickable_base.onClick -= moveToSector;
    clickable_base.onClick += moveToSector;

    foreach( LevelController level in level_controllers )
      level.hide();

    sector_content.SetActive( false );
    planet_sector_content.init();

    camera_container.deinit();
    spawnManager.despawnScreenUI( ScreenUIId.SECTOR );
  }

  public void hide()
  {
    clickable_base.onClick -= moveToSector;
    clickable_base.gameObject.SetActive( false );

    sector_content.SetActive( false );
    planet_sector_content.deinit();

    camera_container.deinit();
    spawnManager.despawnScreenUI( ScreenUIId.SECTOR );
  }

  public void moveToSector()
  {
    if ( !playerDataManager.hasAccessToSector( sector_id ) )
      return;

    cameraController.moveCameraToSectorFromPlanet( this );
  }

  public void markSelected( bool state )
  {
    planet_sector_content.markSelected( state );
  }
  #endregion
}

using UnityEngine;

public class SectorController : MonoBehaviourBase
{
  public GameObject planet_sector_content = null;
  public GameObject sector_content = null;

  public ClickableBase3D clickable_base = null;
  public SectorCameraContainerController camera_container = null;

  public LevelController[] level_controllers = null;


  public void startShowClose()// like from sector view
  {
    clickable_base.onClick -= moveToSector;
    clickable_base.gameObject.SetActive( false );

    planet_sector_content.SetActive( true );
    sector_content.SetActive( true );

    foreach( LevelController level in level_controllers )
      level.startShowFar();
  }

  public void finishShowClose()// like from sector view
  {
    clickable_base.onClick -= moveToSector;
    clickable_base.gameObject.SetActive( false );

    planet_sector_content.SetActive( false );
    sector_content.SetActive( true );

    foreach( LevelController level in level_controllers )
      level.finishShowFar();

    camera_container.init();
    spawnManager.spawnScreenLevelsUI();
  }

  public void startShowFar()// like from sector view
  {
    sector_content.SetActive( true );
    planet_sector_content.SetActive( true );

    foreach( LevelController level in level_controllers )
      level.hide();

    camera_container.deinit();
    spawnManager.despawnScreenLevelsUI();
  }
  
  public void finishShowFar()// like from sector view
  {
    clickable_base.gameObject.SetActive( true );
    clickable_base.onClick -= moveToSector;
    clickable_base.onClick += moveToSector;

    sector_content.SetActive( false );
    planet_sector_content.SetActive( true );

    camera_container.deinit();
    spawnManager.despawnScreenLevelsUI();
  }

  public void hide()
  {
    clickable_base.onClick -= moveToSector;
    clickable_base.gameObject.SetActive( false );

    sector_content.SetActive( false );
    planet_sector_content.SetActive( false );

    camera_container.deinit();
    spawnManager.despawnScreenLevelsUI();
  }

  public void moveToSector()
  {
    //start move camera
    Debug.LogError( "moveToSector" );
    cameraController.moveCameraToSectorFromPlanet( this );
  }
}

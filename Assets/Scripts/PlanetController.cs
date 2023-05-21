using UnityEngine;

public class PlanetController : MonoBehaviourBase
{
  public GameObject planet_content = null;
  public PlanetCameraContainerController camera_container = null;
  public SectorController[] sector_controllers = null;


  public void init()
  {
    showClose();
  }

  public void showClose()
  {
    planet_content.SetActive( true );

    foreach( SectorController sector in sector_controllers )
      sector.showFar();

    camera_container.init();
    spawnManager.spawnScreenMainUI();
  }

  public void hide()
  {
    planet_content.SetActive( false );

    camera_container.deinit();
    spawnManager.despawnScreenMainUI();
  }

  public void moveToPlanet()
  {
    Debug.LogError( "moveToSector" );
    cameraController.moveCameraToPlanet();
  }
}

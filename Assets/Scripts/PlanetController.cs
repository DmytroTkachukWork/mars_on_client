using UnityEngine;

public class PlanetController : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private GameObject planet_content = null;
  [SerializeField] private PlanetCameraContainerController camera_container = null;
  [SerializeField] private SectorController[] sector_controllers = null;
  #endregion

  #region Public Fields
  public PlanetCameraContainerController cameraContainer => camera_container;
  #endregion


  #region Public Methods
  public void init()
  {
    planet_content.SetActive( true );

    foreach( SectorController sector in sector_controllers )
    {
      sector.startShowFar();
      sector.finishShowFar();
    }

    camera_container.init();
    spawnManager.getOrSpawnScreenUI( ScreenUIId.MAIN );
  }

  public void startShowClose()
  {
    planet_content.SetActive( true );

    foreach( SectorController sector in sector_controllers )
      sector.startShowFar();
  }

  public void finishShowClose()
  {
    planet_content.SetActive( true );

    foreach( SectorController sector in sector_controllers )
      sector.finishShowFar();

    camera_container.init();
    spawnManager.getOrSpawnScreenUI( ScreenUIId.MAIN );
  }

  public void startHide()
  {
    spawnManager.despawnScreenUI( ScreenUIId.MAIN );
  }

  public void hide( SectorController curent_sector )
  {
    planet_content.SetActive( false );

    camera_container.deinit();
    spawnManager.despawnScreenUI( ScreenUIId.MAIN );

    foreach( SectorController sector in sector_controllers )
    {
      if ( curent_sector != sector )
        sector.hide();
    }

  }

  public void moveToPlanet()
  {
    cameraController.moveCameraToPlanet();
  }
  #endregion
}

using System.Collections;
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

  #region Private Fields
  private IEnumerator selecting_cor = null;
  #endregion


  #region Public Methods
  public void init()
  {
    startShowClose();
    finishShowClose();
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
    startSelectingSectors();
  }

  public void startHide()
  {
    spawnManager.despawnScreenUI( ScreenUIId.MAIN );
  }

  public void hide( SectorController curent_sector )
  {
    stopSelectingSectors();
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

  public void startSelectingSectors()
  {
    selecting_cor.stop();
    selecting_cor = tweener.waitAndDoCycle(
        selectSector
      , 0.3f
      , 9999.9f
      , null  
    );
    selecting_cor.start();
  }

  public void stopSelectingSectors()
  {
    selecting_cor.stop();
  }

  public void selectSector()
  {
    Vector3 camera_pos =  cameraController.getCameraPos();
    float cached_distance = 0.0f;
    SectorController cached_sector = null;
    float min_distance = float.MaxValue;

    foreach( SectorController sector in sector_controllers )
    {
      sector.markSelected( false );
      cached_distance = Vector3.Distance( sector.transform.position, camera_pos );

      if ( cached_distance < min_distance )
      {
        cached_sector = sector;
        min_distance = cached_distance;
      }
    }

    cached_sector.markSelected( true );
  }
  #endregion
}

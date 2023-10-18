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
  public SectorController curentSector => curent_sector;
  #endregion

  #region Private Fields
  private IEnumerator selecting_cor = null;
  private SectorController curent_sector = null;
  #endregion


  #region Public Methods
  public void init()
  {
    cameraController.init( this );
    startShowClose();
    finishShowClose();
  }

  public void deinit()
  {
    selecting_cor.stop();
    spawnManager.despawnAllSectorInfoUI();
    cameraController.deinit();
  }

  public override void onSpawn()
  {
    base.onSpawn();

    init();
  }

  public override void onDespawn()
  {
    deinit(); 

    base.onDespawn();
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

    //spawn sector info
    foreach( SectorController sector in sector_controllers )
      spawnManager.getOrSpawnSectorInfoUI( sector );
  }

  public void startHide()
  {
    spawnManager.despawnScreenUI( ScreenUIId.MAIN );
    spawnManager.despawnAllSectorInfoUI();
  }

  public void hide( SectorController curent_sector )
  {
    stopSelectingSectors();
    this.curent_sector = curent_sector;
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
    SectorController cached_sector = curent_sector;
    curent_sector = null;
    float min_distance = float.MaxValue;

    foreach( SectorController sector in sector_controllers )
    {
      sector.markSelected( false );
      cached_distance = Vector3.Distance( sector.transform.position, camera_pos );

      if ( cached_distance < min_distance )
      {
        curent_sector = sector;
        min_distance = cached_distance;
      }
    }

    curent_sector.markSelected( true );
    if ( cached_sector != curent_sector )
      ( spawnManager.getOrSpawnScreenUI( ScreenUIId.MAIN ) as ScreenMainUI ).updateCurentSectorID( curent_sector.sectorID );
  }

  public SectorController getNextSector()
  {
    if ( curent_sector.sectorID == sector_controllers.Length - 1 )
      return null;

    return sector_controllers[curent_sector.sectorID+1];
  }

  public SectorController getPrevSector()
  {
    if ( curent_sector.sectorID == 0 )
      return null;

    return sector_controllers[curent_sector.sectorID-1];
  }

  public SectorController getSectorById( int sector_id )
  {
    if ( sector_id >= sector_controllers.Length )
      return null;

    return sector_controllers[sector_id];
  }
  #endregion
}

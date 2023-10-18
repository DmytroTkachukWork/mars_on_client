using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSectorListUI : ScreenBaseUI
{
  #region Serialized Fields
  [SerializeField] private ButtonBase exit_button = null;
  [SerializeField] private SectorListButtonController[] sector_button_controllers = null;
  #endregion

  #region Private Fields
  private int cached_start_page_number = 0;
  #endregion

  #region Public Methods
  public void init( int start_page_number = 0 )
  {
    deinit();

    cached_start_page_number = start_page_number;

    exit_button.onClick += onExit;

    for( int i = 0; i < sector_button_controllers.Length; i++ )
    {
      if ( i + start_page_number > playerDataManager.getLastSectorNumber() )
      {
        sector_button_controllers[i].deinit();
        continue;
      }

      sector_button_controllers[i].init( i );
      sector_button_controllers[i].onSectorClicked += onSectorButtonClicked;
    }
  }

  public void deinit()
  {
    for( int i = 0; i < sector_button_controllers.Length; i++ )
    {
      sector_button_controllers[i].deinit();
      sector_button_controllers[i].onSectorClicked -= onSectorButtonClicked;
    }

    exit_button.onClick -= onExit;
  }

  public override void onSpawn()
  {
    base.onSpawn();

    init();
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }
  #endregion

  #region Private Methods
  private void onSectorButtonClicked( int sector_id )
  {
    spawnManager.spawnPlanet();
    spawnManager.despawnScreenUI( ScreenUIId.SECTOR_LIST );
    cameraController.rotateCameraToSector( sector_id, () => cameraController.moveCameraToSectorFromPlanet( null, sector_id ) );
  }

  private void onExit()
  {
    spawnManager.spawnPlanet();
    spawnManager.despawnScreenUI( ScreenUIId.SECTOR_LIST );
    spawnManager.getOrSpawnScreenUI( ScreenUIId.MAIN );
  }
  #endregion
}

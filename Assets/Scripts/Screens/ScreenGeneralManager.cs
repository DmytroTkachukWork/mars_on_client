using System;
using UnityEngine;

public class ScreenGeneralManager : MonoBehaviourBase
{
  #region Private Fields
  private PlanetController planet_controller = null;
  #endregion


  #region Private Methods
  private void Start()
  {
    init();
  }

  private void init()
  {
    Application.targetFrameRate = 60;
    playerDataManager.loadProgress();
    spawnManager.getOrSpawnScreenUI( ScreenUIId.MAIN );
    planet_controller = spawnManager.spawnPlanet();
    cameraController.init( planet_controller );
    planet_controller.init();
  }
  #endregion
}

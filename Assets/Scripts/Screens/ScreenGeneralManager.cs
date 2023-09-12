using System;
using UnityEngine;

public class ScreenGeneralManager : MonoBehaviourBase
{
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
    spawnManager.spawnPlanet();
  }
  #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenGeneralManager : MonoBehaviourBase
{
  #region Serialized Fields
  [SerializeField] private PlanetController planet_controller = null;
  #endregion
  #region Private Methods
  private void Start()
  {
    init();
  }

  private void init()
  {
    Debug.LogError( "init UI" );
    Application.targetFrameRate = 60;
    spawnManager.getOrSpawnScreenUI( ScreenUIId.MAIN );
    cameraController.init();
    planet_controller.init();
  }
  #endregion
}

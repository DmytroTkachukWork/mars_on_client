using System.Collections;
using System.Collections.Generic;
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
    Debug.LogError( "init UI" );
    spawnManager.spawnScreenMainUI().init();
    spawnManager.spawnScreenMain3D();
  }
  #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLevelUI : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private ButtonBase exit_button = null;
  #endregion

  #region Public Methods
  public void init()
  {
    this.gameObject.SetActive( true );
    exit_button.onClick += onExit;
  }

  public void deinit()
  {
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
  private void onExit()
  {
    Debug.LogError( "onExit" );
    cameraController.moveCameraToSectorFromLevel();
    spawnManager.despawnScreenLevelUI();
  }
  #endregion 
}

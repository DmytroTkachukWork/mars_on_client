using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPauseUI : ScreenBaseUI
{
  #region Serialized Fields
  [SerializeField] private ButtonBase exit_button = null;
  [SerializeField] private ButtonBase replay_button = null;
  [SerializeField] private ButtonBase continue_button = null;
  #endregion


  #region Public Methods
  public void init()
  {
    deinit();
    Time.timeScale = 0.0f;
    exit_button.onClick += onExitClick;
    replay_button.onClick += onReplayClick;
    continue_button.onClick += onContinueClick;
  }

  public void deinit()
  {
    Time.timeScale = 1.0f;
    exit_button.onClick -= onExitClick;
    replay_button.onClick -= onReplayClick;
    continue_button.onClick -= onContinueClick;
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
  private void onExitClick()
  {
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL_PAUSE );
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL );
    levelManager.stopLevel();
    cameraController.moveCameraToSectorFromLevel();
  }

  private void onReplayClick()
  {
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL_PAUSE );
    levelManager.restartLevel();
  }

  private void onContinueClick()
  {
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL_PAUSE );
  }
  #endregion
}

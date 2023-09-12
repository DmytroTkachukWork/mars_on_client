using UnityEngine;
using UnityEngine.UI;

public class ScreenPauseUI : ScreenBaseUI
{
  #region Serialized Fields
  [SerializeField] private ButtonBase exit_button = null;
  [SerializeField] private ButtonBase replay_button = null;
  [SerializeField] private ButtonBase continue_button = null;
  [SerializeField] private RawImage   background_raw_image = null;
  #endregion


  #region Public Methods
  public void init()
  {
    deinit();
    levelManager.pauseLevel();
    exit_button.onClick += onExitClick;
    replay_button.onClick += onReplayClick;
    continue_button.onClick += onContinueClick;

    StartCoroutine( blurScreenshot.takeScreenshot( background_raw_image, false ) );
    background_raw_image.color = Color.white;
  }

  public void deinit()
  {
    exit_button.onClick -= onExitClick;
    replay_button.onClick -= onReplayClick;
    continue_button.onClick -= onContinueClick;

    background_raw_image.color = Color.clear;
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
    levelManager.resumeLevel();
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL_PAUSE );
  }
  #endregion
}

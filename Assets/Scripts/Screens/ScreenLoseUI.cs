using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenLoseUI : ScreenBaseUI
{
  #region Serilized Fields
  [SerializeField] private CanvasGroup canvas_group = null;
  [SerializeField] private ButtonBase exit_button = null;
  [SerializeField] private ButtonBase replay_button = null;
  [SerializeField] private RawImage background_raw_image = null;
  #endregion

  #region Private Fields
  private IEnumerator tween_cor = null;
  #endregion

  
  #region Public Methods
  public void init( Action callback = null )
  {
    deinit();

    exit_button.onClick += onExit;
    replay_button.onClick += replayLevel;

    StartCoroutine( blurScreenshot.takeScreenshot( background_raw_image, false ) );
    background_raw_image.color = Color.white;

    spawnManager.despawnScreenUI( ScreenUIId.LEVEL );
    tween_cor = tween_cor.startCoroutineAndStopPrev( tweener.tweenFloat( ( value ) => canvas_group.alpha = value, 0.0f, 1.0f, myVariables.LEVEL_LOSE_FADE_TIME, callback ) );
  }

  public void deinit()
  {
    exit_button.onClick -= onExit;
    replay_button.onClick -= replayLevel;
    tween_cor.stop();
    background_raw_image.color = Color.clear;
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }
  #endregion

  #region Private Methods
  private void replayLevel()
  {
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL );
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL_LOSE );
    levelManager.restartLevel();
  }

  private void onExit()
  {
    cameraController.moveCameraToSectorFromLevel();
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL_LOSE );
    spawnManager.getOrSpawnScreenUI( ScreenUIId.SECTOR );
  }
  #endregion
}

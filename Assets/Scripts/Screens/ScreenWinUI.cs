using System;
using System.Collections;
using UnityEngine;

public class ScreenWinUI : ScreenBaseUI
{
  #region Serilized Fields
  [SerializeField] private CanvasGroup canvas_group = null;
  [SerializeField] private ButtonBase exit_button = null;
  [SerializeField] private ButtonBase replay_button = null;
  #endregion

  #region Private Fields
  private IEnumerator my_cor = null;
  #endregion

  
  #region Public Methods
  public void init( Action callback = null )
  {
    exit_button.onClick -= exitLevel;
    exit_button.onClick += exitLevel;
    replay_button.onClick -= replayLevel;
    replay_button.onClick += replayLevel;
    my_cor = my_cor.startCoroutineAndStopPrev( tweener.tweenFloat( ( value ) => canvas_group.alpha = value, 0.0f, 1.0f, myVariables.LEVEL_WIN_FADE_TIME, callback ) );
  }

  public void deinit()
  {
    exit_button.onClick -= exitLevel;
    replay_button.onClick -= replayLevel;
    my_cor?.stop();
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
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL_WIN );
    levelManager.restartLevel();
  }

  private void exitLevel()
  {
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL );
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL_WIN );
    cameraController.moveCameraToSectorFromLevel();
    spawnManager.getOrSpawnScreenUI( ScreenUIId.SECTOR );
  }
  #endregion
}

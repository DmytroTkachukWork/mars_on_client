using System;
using System.Collections;
using UnityEngine;

public class ScreenLoseUI : ScreenBaseUI
{
  #region Serilized Fields
  [SerializeField] private CanvasGroup canvas_group = null;
  [SerializeField] private ButtonBase full_button = null;
  #endregion

  #region Private Fields
  private IEnumerator tween_cor = null;
  #endregion

  
  #region Public Methods
  public void init( Action callback = null )
  {
    full_button.onClick += onDespawn;
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL );
    tween_cor = tween_cor.startCoroutineAndStopPrev( tweener.tweenFloat( ( value ) => canvas_group.alpha = value, 0.0f, 1.0f, myVariables.LEVEL_LOSE_FADE_TIME, callback == null ? moveCamera : callback ) );
  }

  public void moveCamera()
  {
    cameraController.moveCameraToSectorFromLevel();
  }

  public void deinit()
  {
    full_button.onClick -= onDespawn;
    tween_cor.stop();
    moveCamera();
    spawnManager.getOrSpawnScreenUI( ScreenUIId.SECTOR );
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }
  #endregion
}

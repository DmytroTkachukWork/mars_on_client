using System;
using UnityEngine;

public class ScreenWinUI : MonoBehaviourPoolable
{
  #region Serilized Fields
  [SerializeField] private CanvasGroup canvas_group = null;
  [SerializeField] private float fade_time = 5.5f;
  [SerializeField] private ButtonBase full_button = null;
  #endregion

  #region Private Fields
  private MyTask my_task = null;
  #endregion

  
  #region Public Methods
  public void init( Action callback = null )
  {
    full_button.onClick += onDespawn;
    my_task = tweener.tween( ( value ) => canvas_group.alpha = value, 0.0f, 1.0f, fade_time, callback == null ? moveCamera : null );
  }

  public void moveCamera()
  {
    spawnManager.despawnScreenLevelUI();
    cameraController.moveCameraToSectorFromLevel();
  }

  public void deinit()
  {
    full_button.onClick -= onDespawn;
    my_task?.stop();
    moveCamera();
    spawnManager.spawnScreenLevelsUI();
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }
  #endregion
}

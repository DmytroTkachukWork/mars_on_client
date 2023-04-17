using System;
using UnityEngine;

public class ScreenWinUI : MonoBehaviourPoolable
{
  #region Serilized Fields
  [SerializeField] private CanvasGroup canvas_group = null;
  [SerializeField] private float fade_time = 0.5f;
  [SerializeField] private ButtonBase full_button = null;
  #endregion

  #region Private Fields
  private Tweener tweener = new Tweener();
  #endregion

  
  #region Public Methods
  public void init( Action callback )
  {
    full_button.onClick += onDespawn;
    tweener.tween( ( value ) => canvas_group.alpha = value, 0.0f, 1.0f, fade_time, callback );
  }

  public void deinit()
  {
    full_button.onClick -= deinit;
    tweener.stop();
    spawnManager.despawnScreenLevel3D();
    spawnManager.despawnScreenLevelUI();
    spawnManager.spawnScreenLevelsUI();
    spawnManager.spawnScreenLevels3D();
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }
  #endregion
}

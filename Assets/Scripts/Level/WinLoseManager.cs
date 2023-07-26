using System;
using System.Collections;
using UnityEngine;

public class WinLoseManager : MonoBehaviourBase
{
  #region Private Fields
  private LevelQuadMatrix level_quad_matrix = null;
  private int cached_steps_left = 0;
  private IEnumerator counter_cor = null;
  #endregion

  #region Public Fields
  public event Action onLose = delegate{};
  #endregion


  #region Public Methods
  public void init( LevelQuadMatrix level_quad_matrix )
  {
    this.level_quad_matrix = level_quad_matrix;
    cached_steps_left = level_quad_matrix.max_steps_to_lose;

    if ( level_quad_matrix.lose_type == LevelLoseType.TIME_LEFT )
      countToZero();

  }

  public void deinit()
  {
    counter_cor.stop();
  }

  public void onBeginRotate( bool is_reverse )
  {
    cached_steps_left += is_reverse ? 1 : -1;
    (spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL ) as ScreenLevelUI ).updateStepsCount( cached_steps_left );

    if ( cached_steps_left < 0 )
      onLose.Invoke();
  }
  
  public ushort getCurentStepsCount()
  {
    return (ushort)cached_steps_left;
  }
  #endregion

  #region Private Methods
  private void countToZero()
  {
    counter_cor = tweener.waitAndDoCycle( 
        () => (spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL ) as ScreenLevelUI ).updateStepsCount( cached_steps_left-- )
      , 1.0f
      , cached_steps_left
      , () => onLose.Invoke()
    );
    counter_cor.start();
  }
  #endregion
}

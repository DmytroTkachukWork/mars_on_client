using System;
using System.Collections;
using UnityEngine;

public class ScreenWinUI : ScreenBaseUI
{
  #region Serilized Fields
  [SerializeField] private CanvasGroup canvas_group = null;
  [SerializeField] private ButtonBase exit_button = null;
  [SerializeField] private ButtonBase replay_button = null;
  [SerializeField] private GameObject[] stars = null;
  [SerializeField] private GameObject card = null;
  #endregion

  #region Private Fields
  private IEnumerator my_cor = null;
  #endregion

  
  #region Public Methods
  public void init( ushort stars_count, bool can_receive_card )
  {
    if ( stars_count > 3 )
      return;

    deinit();
    exit_button.onClick += exitLevel;
    replay_button.onClick += replayLevel;

    for ( ushort i = 0; i < stars.Length; i++ )
      stars[i].SetActive( false );

    for ( ushort i = 0; i < stars_count; i++ )
      stars[i].SetActive( true );

    card.SetActive( can_receive_card );

    my_cor = my_cor.startCoroutineAndStopPrev( tweener.tweenFloat( ( value ) => canvas_group.alpha = value, 0.0f, 1.0f, myVariables.LEVEL_WIN_FADE_TIME, null ) );
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

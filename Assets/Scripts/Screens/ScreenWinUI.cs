using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenWinUI : ScreenBaseUI
{
  #region Serilized Fields
  [SerializeField] private CanvasGroup canvas_group = null;
  [SerializeField] private ButtonBase exit_button = null;
  [SerializeField] private ButtonBase replay_button = null;
  [SerializeField] private ButtonBase continue_button = null;
  [SerializeField] private GameObject[] stars = null;
  [SerializeField] private TMP_Text level_number_text = null;
  [SerializeField] private CardController card_controller = null;
  [SerializeField] private RawImage background_raw_image = null;
  #endregion

  #region Private Fields
  private IEnumerator my_cor = null;
  #endregion

  
  #region Public Methods
  public void init( LevelQuadMatrix level, ushort stars_count, bool can_receive_card )
  {
    if ( stars_count > 3 )
      return;

    deinit();
    exit_button.onClick += exitLevel;
    replay_button.onClick += replayLevel;
    continue_button.onClick += continueNextLevel;

    for ( ushort i = 0; i < stars.Length; i++ )
      stars[i].SetActive( false );

    for ( ushort i = 0; i < stars_count; i++ )
      stars[i].SetActive( true );

    card_controller.gameObject.SetActive( can_receive_card );

    if ( can_receive_card )
      card_controller.init( cardManager.getCardInfoByIndex( playerDataManager.getAbsoluteLevelNumber( level ) + 1 ), ScreenUIId.LEVEL_WIN, true );

    string zero_string = level.level_id + 1 >= 10 ? "" : "0";
    level_number_text.text = zero_string + (level.level_id + 1);

    StartCoroutine( blurScreenshot.takeScreenshot( background_raw_image, false ) );
    background_raw_image.color = Color.white;

    my_cor = my_cor.startCoroutineAndStopPrev( tweener.tweenFloat( ( value ) => canvas_group.alpha = value, 0.0f, 1.0f, myVariables.LEVEL_WIN_FADE_TIME, null ) );
  }

  public void deinit()
  {
    exit_button.onClick -= exitLevel;
    replay_button.onClick -= replayLevel;
    continue_button.onClick -= continueNextLevel;
    my_cor?.stop();
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

  private void continueNextLevel()
  {
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL );
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL_WIN );
    cameraController.teleportCameraToNextLevel();
  }
  #endregion
}

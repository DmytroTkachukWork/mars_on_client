using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenCardUI : ScreenBaseUI
{
  #region Serialized Fields
  [SerializeField] private LeftRightButtonsController left_right_buttons = null;
  [SerializeField] private Image landscape_image = null;
  [SerializeField] private TMP_Text card_text = null;
  [SerializeField] private ButtonBase exit_button = null;
  #endregion

  #region Private Fields
  private CardInfo cached_card_info = null; 
  private ScreenUIId cached_prev_screen_ui_id = ScreenUIId.NONE;
  #endregion

  #region Public Methods
  public void init( CardInfo card_info, ScreenUIId prev_screen_ui_id )
  {
    deinit();
    if ( card_info == null )
      return;

    cached_card_info = card_info;
    cached_prev_screen_ui_id = prev_screen_ui_id;
    exit_button.onClick += onExit;
    card_text.text = cached_card_info.bodyText;
    landscape_image.sprite = cached_card_info.cardLendscape;

    left_right_buttons.init( getLeftInteraction(), getRightInteraction() );

    left_right_buttons.onRightClick += goToNextCard;
    left_right_buttons.onLeftClick  += goToPrevCard;
  }

  public void deinit()
  {
    cached_card_info = null;
    card_text.text = null;
    landscape_image.sprite = null;
    exit_button.onClick -= onExit;

    left_right_buttons.onRightClick -= goToNextCard;
    left_right_buttons.onLeftClick  -= goToPrevCard;
  }
  #endregion

  #region Private Methods
  private void onExit()
  {
    deinit();
    spawnManager.despawnScreenUI( ScreenUIId.CARD );
    spawnManager.getOrSpawnScreenUI( cached_prev_screen_ui_id );
  }

  private void goToNextCard()
  {
    if ( cached_card_info.cardNumber >= playerDataManager.getCurentCardsCount() )
      return;

    CardInfo next_card_info = cardManager.getCardInfoByIndex( cached_card_info.cardNumber + 1 );

    if ( next_card_info == null )
      return;

    init( next_card_info, cached_prev_screen_ui_id );
  }

  private void goToPrevCard()
  {
    if ( cached_card_info.cardNumber == 1 )
      return;

    CardInfo next_card_info = cardManager.getCardInfoByIndex( cached_card_info.cardNumber - 1 );

    if ( next_card_info == null )
      return;

    init( next_card_info, cached_prev_screen_ui_id );
  }

  private bool getLeftInteraction()
  {
    return cached_prev_screen_ui_id == ScreenUIId.LIBRARY && cached_card_info.cardNumber > 1;
  }

  private bool getRightInteraction()
  {
    return cached_prev_screen_ui_id == ScreenUIId.LIBRARY && cached_card_info.cardNumber < playerDataManager.getCurentCardsCount();
  }
  #endregion
}

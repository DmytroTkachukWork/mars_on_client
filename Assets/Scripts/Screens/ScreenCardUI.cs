using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenCardUI : ScreenBaseUI
{
  #region Serialized Fields
  [SerializeField] private ButtonBase next_page_button = null;
  [SerializeField] private ButtonBase prev_page_button = null;
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

    next_page_button.gameObject.SetActive( cached_prev_screen_ui_id == ScreenUIId.LIBRARY );
    prev_page_button.gameObject.SetActive( cached_prev_screen_ui_id == ScreenUIId.LIBRARY );

    next_page_button.onClick += goToNextCard;
    prev_page_button.onClick += goToPrevCard;
  }

  public void deinit()
  {
    cached_card_info = null;
    card_text.text = null;
    landscape_image.sprite = null;
    exit_button.onClick -= onExit;

    next_page_button.onClick -= goToNextCard;
    prev_page_button.onClick -= goToPrevCard;
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
  #endregion
}

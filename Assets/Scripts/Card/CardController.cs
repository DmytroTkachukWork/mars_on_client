using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardController : MonoBehaviourBase
{
  #region Serialized Fields
  [SerializeField] private ButtonBase self_button = null;
  [SerializeField] private TMP_Text number_text = null;
  [SerializeField] private Image card_avatar = null;
  #endregion

  #region Private Fields
  private CardInfo cached_card_info = null;
  private bool cached_open_card_on_click = false;
  private ScreenUIId cached_prev_screen_id = ScreenUIId.NONE;
  #endregion

  #region Public Fields
  public event Action<int> onCardClick = delegate{};
  #endregion

  public void init( CardInfo card_info, ScreenUIId prev_screen_id, bool open_card_on_click = false )
  {
    if ( card_info == null )
      return;

    cached_card_info = card_info;
    cached_open_card_on_click = open_card_on_click;
    self_button.onClick += onCardClickAction;

    string zero_string = cached_card_info.cardNumber >= 10 ? "" : "0";
    number_text.text = zero_string + cached_card_info.cardNumber;

    if ( cached_card_info.cardNumber > playerDataManager.getCurentCardsCount() )
    {
      card_avatar.enabled = false;
      return;
    }

    card_avatar.sprite = cached_card_info.cardAvatar;
    card_avatar.enabled = true;
  }

  public void deinit()
  {
    cached_card_info = null;
    card_avatar.enabled = false;
    card_avatar.sprite = null;
    self_button.onClick -= onCardClickAction;
  }

  private void onCardClickAction()
  {
    onCardClick.Invoke( cached_card_info.cardNumber );

    if ( !cached_open_card_on_click )
      return;

    spawnManager.despawnScreenUI( ScreenUIId.LIBRARY );
    (spawnManager.getOrSpawnScreenUI( ScreenUIId.CARD ) as ScreenCardUI).init( cached_card_info, cached_prev_screen_id );
  }
}

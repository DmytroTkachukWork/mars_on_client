using UnityEngine;
using TMPro;
using System;

public class SectorListButtonController : MonoBehaviourBase
{
  #region Serialized Fields
  [SerializeField] private ButtonBase self_button = null;
  [SerializeField] private TMP_Text curent_stars_count = null;
  [SerializeField] private TMP_Text all_stars_count = null;
  [SerializeField] private TMP_Text curent_cards_count = null;
  [SerializeField] private TMP_Text all_cards_count = null;
  [SerializeField] private TMP_Text progress_percent = null;
  [SerializeField] private TMP_Text sector_number = null;
  [SerializeField] private RectTransform lower_progress_bar = null;
  [SerializeField] private RectTransform higher_progress_bar = null;
  [SerializeField] private float progress_bar_max_hight = 0.0f;
  [SerializeField] private GameObject locked_sector = null;
  #endregion

  #region Public Fields
  public event Action<int> onSectorClicked = delegate{};
  #endregion

  #region Private Fields
  private int cached_sector_id = 0;
  #endregion

  #region Public Methods
  public void init( int sector_id )
  {
    deinit();

    cached_sector_id = sector_id;
    self_button.onClick += onSectorClick;

    updateFields(); 
  }

  public void deinit()
  {
    self_button.onClick -= onSectorClick;
  }
  #endregion

  #region Privat Methods
  private void onSectorClick()
  {
    onSectorClicked.Invoke( cached_sector_id );
  }

  private void updateFields()
  {
    if ( cached_sector_id <= playerDataManager.getCurentSectorNumber() )
    {
      curent_stars_count.text = playerDataManager.getReceivedStarsCountBySector( cached_sector_id ).ToString();
      curent_cards_count.text = playerDataManager.getReceivedCardsCountBySector( cached_sector_id ).ToString();
      progress_percent.text = Mathf.CeilToInt(playerDataManager.getLevelsProgressBySector( cached_sector_id ) * 100).ToString() + "%";
    }
    else
    {
      curent_stars_count.text = "0";
      curent_cards_count.text = "0";
      progress_percent.text = "0%";
    }

    all_cards_count.text = "/" + playerDataManager.getAllCardsCountBySector( cached_sector_id ).ToString();
    all_stars_count.text = "/" + playerDataManager.getAllStarsCountBySector( cached_sector_id ).ToString();
    sector_number.text = cached_sector_id.ToString();

    Vector2 progress_delta = lower_progress_bar.sizeDelta;
    progress_delta.y = progress_bar_max_hight * playerDataManager.getLevelsProgressBySector( cached_sector_id );

    higher_progress_bar.gameObject.SetActive( false );
    if ( progress_delta.y > progress_bar_max_hight - 30.0f )
    {
      progress_delta.y = progress_bar_max_hight - 30.0f;
      higher_progress_bar.gameObject.SetActive( true );
    }

    lower_progress_bar.sizeDelta = progress_delta;
    locked_sector.SetActive( cached_sector_id > playerDataManager.getCurentSectorNumber() );
  }
  #endregion
}

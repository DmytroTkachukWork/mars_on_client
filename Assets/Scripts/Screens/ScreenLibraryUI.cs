using UnityEngine;
using TMPro;

public class ScreenLibraryUI : ScreenBaseUI
{
  #region Serialized Fields
  [SerializeField] private LeftRightButtonsController left_right_buttons = null;
  [SerializeField] private ButtonBase exit_button = null;
  [SerializeField] private TMP_Text stars_text = null;
  [SerializeField] private TMP_Text cards_text = null;
  [SerializeField] private TMP_Text progress_text = null;
  [SerializeField] private CardController[] card_controllers = null;
  [SerializeField] private LibraryPageDotController[] page_controllers = null;
  #endregion

  #region Private Fields
  private int cached_start_page_number = 0;
  #endregion


  #region Public Methods
  public void init( int start_page_number = 0 )
  {
    deinit();

    cached_start_page_number = start_page_number;

    updateStarsCount();
    updateCardsCount();
    updateProgress();

    exit_button.onClick += onExit;

    left_right_buttons.init( cached_start_page_number > 0, cached_start_page_number < page_controllers.Length - 1 );

    left_right_buttons.onRightClick += goToNextPage;
    left_right_buttons.onLeftClick += goToPrevPage;

    for( int i = 0; i < page_controllers.Length; i++ )
      page_controllers[i].init( (int)(start_page_number / 10) == i );

    for( int i = 0; i < card_controllers.Length; i++ )
    {
      if ( i + start_page_number >= playerDataManager.getCurentCardsCount() )
      {
        card_controllers[i].deinit();
        continue;
      }

      card_controllers[i].init( cardManager.getCardInfoByIndex( i + start_page_number + 1 ), ScreenUIId.LIBRARY );
      card_controllers[i].onCardClick += onCardClicked;
    }
  }

  public void deinit()
  {
    for( int i = 0; i < card_controllers.Length; i++ )
    {
      card_controllers[i].deinit();
      card_controllers[i].onCardClick -= onCardClicked;
    }

    exit_button.onClick -= onExit;
    left_right_buttons.onRightClick-= goToNextPage;
    left_right_buttons.onLeftClick -= goToPrevPage;
  }

  public override void onSpawn()
  {
    base.onSpawn();

    init();
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }
  #endregion

  #region Private Methods
  private void onCardClicked( int index )
  {
    spawnManager.despawnScreenUI( ScreenUIId.LIBRARY );
    (spawnManager.getOrSpawnScreenUI( ScreenUIId.CARD ) as ScreenCardUI).init( cardManager.getCardInfoByIndex( index ), ScreenUIId.LIBRARY );
  }

  private void updateStarsCount()
  {
    ushort curent_stars_count = playerDataManager.getCurentStarsCount();
    ushort max_stars_count = playerDataManager.getMaxStarsCount();

    stars_text.text = curent_stars_count.ToString() + "/" + max_stars_count.ToString();
  }

  private void updateCardsCount()
  {
    ushort curent_cards_count = playerDataManager.getCurentCardsCount();
    ushort max_cards_count = playerDataManager.getMaxCardsCount();

    cards_text.text = curent_cards_count.ToString() + "/" + max_cards_count.ToString();
  }

  private void updateProgress()
  {
    int curent_progress = playerDataManager.getCurentProgressPercent();

    progress_text.text = "PROGRESS " + curent_progress.ToString() + "%";
  }

  private void goToNextPage()
  {
    if( cached_start_page_number + 10 >= playerDataManager.getMaxCardsCount() )
      return;

    init( cached_start_page_number + 10 );
  }

  private void goToPrevPage()
  {
    if ( cached_start_page_number <= 0 )
      return;

    init( cached_start_page_number - 10 );
  }

  private void onExit()
  {
    spawnManager.spawnPlanet();
    spawnManager.despawnScreenUI( ScreenUIId.LIBRARY );
    spawnManager.getOrSpawnScreenUI( ScreenUIId.MAIN );
  }
  #endregion
}

using UnityEngine;
using TMPro;

public class ScreenLibraryUI : ScreenBaseUI
{
  #region Serialized Fields
  [SerializeField] private ButtonBase next_page_button = null;
  [SerializeField] private ButtonBase prev_page_button = null;
  [SerializeField] private ButtonBase exit_button = null;
  [SerializeField] private TMP_Text stars_text = null;
  [SerializeField] private TMP_Text cards_text = null;
  [SerializeField] private TMP_Text progress_text = null;
  [SerializeField] private CardController[] card_controllers = null;
  #endregion


  #region Public Methods
  public void init()
  {
    deinit();
    exit_button.onClick += onExit;
    

    updateStarsCount();
    updateCardsCount();
    updateProgress();

    for( int i = 0; i < card_controllers.Length; i++ )
    {
      if ( i >= playerDataManager.getCurentCardsCount() )
        break;

      card_controllers[i].init( cardManager.getCardInfoByIndex( i+1 ), ScreenUIId.LIBRARY );
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

  private void onExit()
  {
    spawnManager.spawnPlanet();
    spawnManager.despawnScreenUI( ScreenUIId.LIBRARY );
    spawnManager.getOrSpawnScreenUI( ScreenUIId.MAIN );
  }
  #endregion
}

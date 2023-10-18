using TMPro;
using UnityEngine;

public class ScreenMainUI : ScreenBaseUI
{
  #region Serialized Fields
  [SerializeField] private ButtonBase reset_user_button = null;
  [SerializeField] private ButtonBase max_user_button = null;
  [SerializeField] private LeftRightButtonsController left_right_buttons = null;
  [SerializeField] private ButtonBase library_button = null;
  [SerializeField] private ButtonBase sector_list_button = null;
  [SerializeField] private TMP_Text sector_id_text = null;
  [SerializeField] private TMP_Text stars_text = null;
  [SerializeField] private TMP_Text cards_text = null;
  [SerializeField] private TMP_Text progress_text = null;
  #endregion

  #region Private Fields
  private int curent_sector_id = 0;
  private string sector_id_text_string_1 = "SECTOR ";
  private string sector_id_text_string_2 = "/3";
  #endregion


  #region Public Methods
  public void init()
  {
    deinit();
    reset_user_button.onClick += resetUser;

    left_right_buttons.init( !isFirstSectorSelected(), !isLastSectorSelected() );

    left_right_buttons.onRightClick += selectNextSector;
    left_right_buttons.onLeftClick += selectPrevtSector;

    max_user_button.onClick += setMaxUser;
    library_button.onClick += openLibrary;
    sector_list_button.onClick += openSectorList;
    

    updateStarsCount();
    updateCardsCount();
    updateProgress();
  }

  public void deinit()
  {
    reset_user_button.onClick -= resetUser;
    left_right_buttons.onRightClick -= selectNextSector;
    left_right_buttons.onLeftClick -= selectPrevtSector;
    max_user_button.onClick -= setMaxUser;
    library_button.onClick -= openLibrary;
    sector_list_button.onClick -= openSectorList;
  }

  public void selectNextSector()
  {
    cameraController.rotateCameraToNextSector();
  }

  public void selectPrevtSector()
  {
    cameraController.rotateCameraToPrevSector();
  }

  public void updateCurentSectorID( int new_sector_id )
  {
    curent_sector_id = new_sector_id;
    sector_id_text.text = sector_id_text_string_1 + (curent_sector_id + 1) + sector_id_text_string_2;
    left_right_buttons.init( !isFirstSectorSelected(), !isLastSectorSelected() );
  }

  public bool isFirstSectorSelected()
  {
    return curent_sector_id == 0;
  }

  public bool isLastSectorSelected()
  {
    return curent_sector_id == playerDataManager.getLastSectorNumber();
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
  private void resetUser()
  {
    playerDataManager.resetProgress();
    updateStarsCount();
    updateProgress();
  }

  private void setMaxUser()
  {
    playerDataManager.setMaxProgress();
    updateStarsCount();
    updateProgress();
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

  private void openLibrary()
  {
    spawnManager.despawnPlanet();
    spawnManager.despawnScreenUI( ScreenUIId.MAIN );
    spawnManager.getOrSpawnScreenUI( ScreenUIId.LIBRARY );
  }

  private void openSectorList()
  {
    spawnManager.despawnPlanet();
    spawnManager.despawnScreenUI( ScreenUIId.MAIN );
    spawnManager.getOrSpawnScreenUI( ScreenUIId.SECTOR_LIST );
  }
  #endregion
}

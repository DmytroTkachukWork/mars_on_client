using TMPro;
using UnityEngine;

public class ScreenMainUI : ScreenBaseUI
{
  #region Serialized Fields
  [SerializeField] private ButtonBase reset_user_button = null;
  [SerializeField] private ButtonBase max_user_button = null;
  [SerializeField] private ButtonBase next_sector_button = null;
  [SerializeField] private ButtonBase prev_sector_button = null;
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
    next_sector_button.onClick += selectNextSector;
    prev_sector_button.onClick += selectPrevtSector;
    max_user_button.onClick += setMaxUser;
    

    updateStarsCount();
    updateCardsCount();
    updateProgress();
  }

  public void deinit()
  {
    reset_user_button.onClick -= resetUser;
    next_sector_button.onClick -= selectNextSector;
    prev_sector_button.onClick -= selectPrevtSector;
    max_user_button.onClick -= setMaxUser;
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
    sector_id_text.text = sector_id_text_string_1 + curent_sector_id + sector_id_text_string_2;
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
    int curent_progress = playerDataManager.getCurentProgress();

    progress_text.text = "PROGRESS " + curent_progress.ToString() + "%";
  }
  #endregion
}

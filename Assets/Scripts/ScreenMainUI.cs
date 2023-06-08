using UnityEngine;

public class ScreenMainUI : ScreenBaseUI
{
  #region Serialized Fields
  [SerializeField] private ButtonBase exit_button = null;
  [SerializeField] private ButtonBase reset_user_button = null;
  #endregion


  #region Public Methods
  public void init()
  {
    reset_user_button.onClick += resetUser;
  }

  public void deinit()
  {
    reset_user_button.onClick -= resetUser;
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
  }
  #endregion
}

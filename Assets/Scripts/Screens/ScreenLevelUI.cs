using TMPro;
using UnityEngine;

public class ScreenLevelUI : ScreenBaseUI
{
  #region Serialized Fields
  [SerializeField] private ButtonBase undo_button = null;
  [SerializeField] private ButtonBase pause_button = null;
  [SerializeField] private TMP_Text steps_to_lose_text = null;
  #endregion

  #region Public Methods
  public void init()
  {
    deinit();
    undo_button.onClick += undoAction;
    pause_button.onClick += pauseAction;
  }

  public void deinit()
  {
    undo_button.onClick -= undoAction;
    pause_button.onClick -= pauseAction;
    steps_to_lose_text.text = string.Empty;
  }

  public void updateStepsCount( int count )
  {
    steps_to_lose_text.text = count.ToString();
  }

  public void undoAction()
  {
    levelManager.undoAction();
  }

  public void pauseAction()
  {
    spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL_PAUSE );
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
}

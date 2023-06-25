using TMPro;
using UnityEngine;

public class ScreenLevelUI : ScreenBaseUI
{
  #region Serialized Fields
  [SerializeField] private ButtonBase exit_button = null;
  [SerializeField] private ButtonBase undo_button = null;
  [SerializeField] private TMP_Text steps_to_lose_text = null;
  #endregion

  #region Public Methods
  public void init()
  {
    deinit();
    exit_button.onClick += onExit;
    undo_button.onClick += undoAction;
  }

  public void deinit()
  {
    exit_button.onClick -= onExit;
    undo_button.onClick -= undoAction;
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
  private void onExit()
  {
    Debug.LogError( "onExit" );
    cameraController.moveCameraToSectorFromLevel();
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL );
  }
  #endregion 
}

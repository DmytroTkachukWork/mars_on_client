using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenLevelUI : ScreenBaseUI
{
  #region Serialized Fields
  [SerializeField] private ButtonBase exit_button = null;
  [SerializeField] private TMP_Text steps_to_lose_text = null;
  #endregion

  #region Public Methods
  public void init()
  {
    this.gameObject.SetActive( true );
    exit_button.onClick += onExit;
  }

  public void deinit()
  {
    exit_button.onClick -= onExit;
  }

  public void updateStepsCount( int count )
  {
    steps_to_lose_text.text = count.ToString();
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

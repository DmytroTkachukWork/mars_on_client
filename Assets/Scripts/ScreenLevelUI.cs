using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLevelUI : MonoBehaviourBase
{
  #region Serialized Fields
  [SerializeField] private ButtonBase exit_button = null;
  #endregion

  #region Public Methods
  public void init()
  {
    this.gameObject.SetActive( true );
    exit_button.onClick += onExit;
  }

  public void deinit()
  {
    this.gameObject.SetActive( false );
    exit_button.onClick -= onExit;
  }
  #endregion

  #region Private Methods
  private void onExit()
  {
    Debug.LogError( "onExit" );
    deinit();
    spawnManager.spawnScreenMainUI().init();
    spawnManager.spawnScreenLevel3D().deinit();
  }
  #endregion 
}

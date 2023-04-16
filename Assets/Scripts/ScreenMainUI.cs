using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMainUI : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private ButtonBase levels_button = null;
  [SerializeField] private ButtonBase settings_button = null;
  #endregion

  #region Private Fields
  private SpawnManager spawner_manager = null;
  #endregion

  #region Public Methods
  public void init()
  {
    this.gameObject.SetActive( true );
    levels_button.init();
    levels_button.onClick += onLevelsClick;
  }

  public void deinit()
  {
    levels_button.deinit();
    levels_button.onClick -= onLevelsClick;
    onDespawn();
  }
  #endregion

  #region Private Methods
  private void onLevelsClick()
  {
    Debug.LogError( "onLevelsClick" );
    deinit();
    spawnManager.spawnScreenLevelsUI().init();
  }
  #endregion
}

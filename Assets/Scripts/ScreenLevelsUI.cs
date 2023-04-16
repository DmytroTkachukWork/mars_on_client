using UnityEngine;

public class ScreenLevelsUI : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private ButtonBase[] level_buttons = null;
  [SerializeField] private ButtonBase exit_button = null;
  #endregion


  #region Public Methods
  public void init()
  {
    Debug.LogError( "init ScreenLevelsUI" );
    this.gameObject.SetActive( true );
    exit_button.onClick += onExit;
    foreach ( ButtonBase level in level_buttons )
    {
      level.init();
      level.onClickInt += onLevelClick;
    }
  }

  public void deinit()
  {
    exit_button.onClick -= onExit;
    foreach ( ButtonBase level in level_buttons )
    {
      level.onClickInt -= onLevelClick;
      level.deinit();
    }
    onDespawn();
  }
  #endregion

  #region Private Methods
  private void onLevelClick( int value )
  {
    Debug.LogError( "onLevelClick" );
    spawnManager.despawnScreenMain3D();
    spawnManager.spawnScreenLevel3D().init( value );
    spawnManager.spawnScreenLevelUI().init();
    deinit();
  }

  private void onExit()
  {
    Debug.LogError( "onExit" );
    deinit();
    spawnManager.spawnScreenMainUI().init();
    spawnManager.spawnScreenMain3D();
  }
  #endregion 
}

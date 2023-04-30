using UnityEngine;

public class ScreenLevelsUI : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private ButtonBase exit_button = null;
  #endregion


  #region Public Methods
  public void init()
  {
    exit_button.onClick += onExit;
  }

  public void deinit()
  {
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
  private void onLevelClick( int value )
  {
    Debug.LogError( "onLevelClick" );
    spawnManager.despawnScreenLevels3D();
    spawnManager.despawnScreenLevelsUI();
    spawnManager.spawnScreenLevel3D().init( value );
  }

  private void onExit()
  {
    Debug.LogError( "onExit" );
    spawnManager.despawnScreenLevels3D();
    spawnManager.despawnScreenLevelsUI();
    spawnManager.spawnScreenMainUI().init();
    spawnManager.spawnScreenMain3D();
  }
  #endregion 
}

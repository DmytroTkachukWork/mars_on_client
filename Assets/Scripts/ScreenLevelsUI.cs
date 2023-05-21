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

  private void onExit()
  {
    Debug.LogError( "onExit" );
    cameraController.moveCameraToPlanet();
  }
  #endregion 
}

using UnityEngine;

public class ScreenMainUI : MonoBehaviourPoolable
{
  #region Public Methods
  public void init()
  {
    this.gameObject.SetActive( true );
  }

  public void deinit()
  {
    this.gameObject.SetActive( false );
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }
  #endregion
}

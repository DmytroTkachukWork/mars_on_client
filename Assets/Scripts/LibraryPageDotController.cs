using UnityEngine;
using UnityEngine.UI;

public class LibraryPageDotController : MonoBehaviourBase
{
  #region Serialized Fields
  [SerializeField] private Image selection_image = null;
  #endregion

  #region Public Methods
  public void init( bool state )
  {
    selection_image.enabled = state;
  }

  public void deinit()
  {
    selection_image.enabled = false;
  }
  #endregion
}

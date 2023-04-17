using System;
using UnityEngine;

public class MapLevel3DController : MonoBehaviour
{
  #region Serialized Fields
  [SerializeField] private ClickableBase3D clickable_base = null;
  [SerializeField] private int level_number = 0;
  #endregion

  #region Public Fields
  public event Action<int> onClick = delegate{};
  #endregion

  #region Public Methods
  public void init()
  {
    clickable_base.onClick += handleClick;
  }

  public void deinit()
  {
    clickable_base.onClick -= handleClick;
  }
  #endregion

  #region Private Methods
  private void handleClick()
  {
    onClick.Invoke( level_number );
  }
  #endregion
}

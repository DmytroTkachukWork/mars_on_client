using System;
using UnityEngine;


public class ClickableBase3D : MonoBehaviour
{
  #region Public Fields
  public event Action onClick = delegate{};
  #endregion

  #region Private Methods
  private void OnMouseDown()
  {
    onClick.Invoke();
  }
  #endregion
}

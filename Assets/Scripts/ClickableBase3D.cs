using System;
using System.Collections;
using UnityEngine;


public class ClickableBase3D : MonoBehaviourBase
{
  #region Public Fields
  public event Action onClick = delegate{};
  public event Action<float> onZoom = delegate{};
  public event Action onBeginDrag = delegate{};
  public event Action onEndDrag = delegate{};
  public event Action<Vector3> onDrag = delegate{};
  public bool dragEnabled = false;
  #endregion

  #region Private Fields
  private Vector3 cached_mouse_pos = Vector3.zero;
  private Vector3 drag_delta = Vector3.zero;
  private IEnumerator drag_updater = null;
  #endregion


  #region Private Methods
  private void Update()
  {
    if ( Mathf.Abs( Input.mouseScrollDelta.y ) > 0.01f )
      onZoom.Invoke( Input.mouseScrollDelta.y );
  }
  private void OnMouseDown()
  {
    onClick.Invoke();
    if ( !dragEnabled )
      return;

    onBeginDrag.Invoke();
    startDrag();
  }

  private void OnMouseUp()
  {
    stopDrag();
  }

  private void startDrag()
  {
    cached_mouse_pos = Input.mousePosition;
    drag_updater = drag_updater.startCoroutineAndStopPrev( tweener.updateUntil( drag ) );

    void drag()
    {
      drag_delta = Input.mousePosition - cached_mouse_pos;
      cached_mouse_pos = Input.mousePosition;
      onDrag.Invoke( drag_delta );
    }
  }

  private void stopDrag()
  {
    drag_updater?.stop();
    onEndDrag.Invoke();
  }
  #endregion
}

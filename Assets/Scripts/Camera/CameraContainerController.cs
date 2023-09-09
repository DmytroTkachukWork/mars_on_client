using System.Collections;
using UnityEngine;

public class CameraContainerController : MonoBehaviourBase
{
  #region Serialized Fields
  [SerializeField] private ClickableBase3D clickable_base = null;
  [SerializeField] private Transform rotation_transform = null;
  [SerializeField] private Transform zoom_transform = null;
  [SerializeField] private float drag_position_speed = 0.05f;
  [SerializeField] private float drag_rotation_speed = 0.1f;
  [SerializeField] private float zoom_speed = 0.1f;
  [SerializeField] private Vector2 top_bottom_limit = Vector2.zero;
  [SerializeField] private Vector2 left_right_limit = Vector2.zero;
  [SerializeField] private Vector2 top_bottom_limit_rotation = Vector2.zero;
  [SerializeField] private Vector2 max_min_limit_zoom = Vector2.zero;
  #endregion

  #region Private Fields
  private float SWIPE_POSITION_TIME = 0.05f;
  private float SWIPE_ROTATION_TIME = 0.4f;
  private float MIN_DELTA = 0.01f;

  private float ON_MOVE_POSITION_POGRESS = 0.0015f;
  private float ON_MOVE_POSITION_DELTA_POGRESS = 0.005f;
  private float ON_MOVE_POSITION_TIME_SCALE = 0.005f;
  private float ON_MOVE_POSITION_DELTA_TIME_SCALE = 0.1f;

  private float ON_MOVE_ROTATION_POGRESS = 0.01f;
  private float ON_MOVE_ROTATION_TIME_SCALE = 0.1f;

  private Vector3 cached_delta_sum = Vector3.zero;
  private Vector3 cached_aprox_position = Vector3.zero;
  private Vector3 cached_position = Vector3.zero;
  private IEnumerator drag_cor = null;
  private bool drag_cor_finished = true;
  private float grag_time_left = 0.0f;
  private float grag_time_left_delta = 0.0f;
  private DragType drag_type = DragType.ROTATION;

  private Vector3 target_rotation = Vector3.zero;
  private Vector3 zoomed = Vector3.zero;
  #endregion

  #region Public Fields
  public Transform cameraRoot => zoom_transform;
  #endregion


  #region Public Methods
  public virtual void init()
  {
    deinit();
    resetCamera();
    setUpCamera( DragType.POSITION );
    clickable_base.gameObject.SetActive( true );
    clickable_base.onBeginDrag += onBeginDrag;
    clickable_base.onDrag += onDrag;
    clickable_base.onZoom += onZoom;
  }

  public virtual void deinit()
  {
    resetCamera();
    clickable_base.onBeginDrag -= onBeginDrag;
    clickable_base.onDrag -= onDrag;
    clickable_base.onZoom -= onZoom;
    clickable_base.gameObject.SetActive( false );
  }

  public void resetCamera()
  {
    drag_cor?.stop();
    drag_cor_finished = true;
    rotation_transform.localPosition = Vector3.zero;
    zoom_transform.localPosition = Vector3.zero;
  }
  
  public virtual void setUpCamera( DragType drag_type )
  {
    this.drag_type = drag_type;
    clickable_base.dragEnabled = true;
    clickable_base.gameObject.SetActive( true );
  }

  public void rotateTo( Quaternion rotation, float rotation_time )
  {
    drag_cor = drag_cor.startCoroutineAndStopPrev( tweener.tweenRotation( rotation_transform, rotation, rotation_time, callback, CurveType.EASE_IN_OUT ) );

    void callback()
    {
      target_rotation = rotation.eulerAngles;
    }
  }
  #endregion

  #region Private Methods
  private void onBeginDrag()
  {
    drag_cor?.stop();
    drag_cor_finished = true;
    grag_time_left = 0.0f;
    cached_delta_sum = Vector3.zero;
  }

  private void onZoom( float delta )
  {
    zoomed = zoom_transform.localPosition;
    zoomed.z += delta * zoom_speed;
    zoomed.z = Mathf.Clamp( zoomed.z, max_min_limit_zoom.y, max_min_limit_zoom.x );
    zoom_transform.localPosition = zoomed;
  }

  private void onDrag( Vector3 delta )
  {
    switch( drag_type )
    {
    case DragType.POSITION: dragPosition( delta ); break;
    case DragType.ROTATION: dragRotation( delta ); break;
    }
  }

  private void dragPosition( Vector3 delta )
  {
    if ( !isDeltaEnought( delta ) )
      return;

    cached_delta_sum += delta;
    cached_aprox_position.x = rotation_transform.localPosition.x - cached_delta_sum.x * drag_position_speed;
    cached_aprox_position.y = rotation_transform.localPosition.y;
    cached_aprox_position.z = rotation_transform.localPosition.z - cached_delta_sum.y * drag_position_speed;
    grag_time_left = ON_MOVE_POSITION_POGRESS;
    grag_time_left_delta = ON_MOVE_POSITION_DELTA_POGRESS;

    if ( drag_cor_finished )
      drag_cor = drag_cor.startCoroutineAndStopPrev( perform() );

    IEnumerator perform()
    {
      drag_cor_finished = false;
      while( grag_time_left <= SWIPE_POSITION_TIME ) 
      {
        cached_position = Vector3.Lerp( rotation_transform.localPosition, cached_aprox_position, grag_time_left / SWIPE_POSITION_TIME );
        cached_position.x = Mathf.Clamp( cached_position.x, left_right_limit.y, left_right_limit.x );
        cached_position.z = Mathf.Clamp( cached_position.z, top_bottom_limit.y, top_bottom_limit.x );
        rotation_transform.localPosition = cached_position;
        cached_delta_sum = Vector3.Lerp( cached_delta_sum, Vector3.zero, grag_time_left_delta / SWIPE_POSITION_TIME );
        grag_time_left += Time.deltaTime * ON_MOVE_POSITION_TIME_SCALE;
        grag_time_left_delta += Time.deltaTime * ON_MOVE_POSITION_DELTA_TIME_SCALE;
        yield return null;
      }
      drag_cor_finished = true;
    }
  }

  private void dragRotation( Vector3 delta )
  {
    if ( !isDeltaEnought( delta ) )
      return;

    cached_delta_sum += delta;

    target_rotation.x -= delta.y * drag_rotation_speed;
    target_rotation.y += delta.x * drag_rotation_speed;

    cached_aprox_position.x = target_rotation.x;
    cached_aprox_position.y = target_rotation.y;
    cached_aprox_position.z = 0.0f;

    cached_aprox_position.x = Mathf.Clamp( cached_aprox_position.x, top_bottom_limit_rotation.y, top_bottom_limit_rotation.x );

    grag_time_left = ON_MOVE_ROTATION_POGRESS;
    grag_time_left_delta = ON_MOVE_ROTATION_POGRESS;

    if ( drag_cor_finished )
      drag_cor = drag_cor.startCoroutineAndStopPrev( perform() );

    IEnumerator perform()
    {
      drag_cor_finished = false;
      while( grag_time_left <= SWIPE_ROTATION_TIME ) 
      {
        rotation_transform.rotation = Quaternion.Lerp(
            rotation_transform.rotation
          , Quaternion.Euler( cached_aprox_position )
          , grag_time_left / SWIPE_ROTATION_TIME );

        cached_delta_sum = Vector3.Lerp( cached_delta_sum, Vector3.zero, grag_time_left_delta / SWIPE_ROTATION_TIME );
        grag_time_left += Time.deltaTime * ON_MOVE_ROTATION_TIME_SCALE;
        grag_time_left_delta += Time.deltaTime * ON_MOVE_ROTATION_TIME_SCALE;
        yield return null;
      }
      drag_cor_finished = true;
    }
  }
  private bool isDeltaEnought( Vector3 delta )
  {
    return Mathf.Abs(delta.x) >= MIN_DELTA || Mathf.Abs(delta.y) >= MIN_DELTA || Mathf.Abs(delta.z) >= MIN_DELTA;
  }
  #endregion
}

public enum DragType
{
  NONE = 0,
  POSITION = 1,
  ROTATION = 2
}
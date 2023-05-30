using System;
using System.Threading.Tasks;
using UnityEngine;


public class QuadMovementController : MonoBehaviourBase
{
  #region Serialized Fields
  [SerializeField] private ClickableBase3D clickable_basease = null;
  #endregion

  #region Private Fields
  private const float ANGLE_PER_CLICK = 90.0f;
  private Task rotation_task = Task.CompletedTask;
  private float target_rotation = 0.0f;
  private float rotation_time_left = 0.0f;
  private Vector3 cached_scale = Vector3.one;
  #endregion

  #region Public Fields
  public event Action onRotate = delegate{};
  public event Action<float> onBeginRotate = delegate{};
  #endregion


  #region Public Methods
  public void init( float start_angle )
  {
    target_rotation = start_angle;
    transform.localRotation = Quaternion.Euler( 0.0f, target_rotation, 0.0f );
    rotation_time_left = 0.0f;
    clickable_basease.onClick += rotateOverTime;
  }

  public void deinit()
  {
    forcestopRotation();
    clickable_basease.onClick -= rotateOverTime;
  }
  #endregion

  #region Private Methods
  private async void rotateOverTime()
  {
    rotation_time_left = myVariables.QUAD_ROTATION_TIME;
    target_rotation = target_rotation + ANGLE_PER_CLICK;

    onBeginRotate.Invoke( target_rotation );

    if ( rotation_task.IsCompleted )
      rotation_task = rotateHex();

    async Task rotateHex()
    {
      while( rotation_time_left > myVariables.QUAD_ROTATION_TIME / 1.5 )
      {
        transform.localRotation = Quaternion.Lerp(
            Quaternion.Euler( 0.0f, target_rotation, 0.0f )
          , transform.localRotation
          , rotation_time_left / myVariables.QUAD_ROTATION_TIME );

        rotation_time_left -= Time.deltaTime;


        if ( transform.localRotation.eulerAngles.y >= target_rotation - 1.0f && transform.localRotation.eulerAngles.y <= target_rotation + 1.0f )
        {
          transform.localRotation = Quaternion.Euler( 0.0f, target_rotation, 0.0f );
          break;
        }

        await Task.Yield();
      }

      transform.localRotation = Quaternion.Euler( 0.0f, target_rotation, 0.0f );
      onRotate.Invoke();
    }
  }

  private void forcestopRotation()
  {
    rotation_task = Task.CompletedTask;
    transform.localRotation = Quaternion.Euler( 0.0f, target_rotation, 0.0f );
    cached_scale = Vector3.one;
    transform.localScale = cached_scale;
  }
  #endregion
}

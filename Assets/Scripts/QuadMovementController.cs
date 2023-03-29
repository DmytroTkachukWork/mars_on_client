using System;
using System.Threading.Tasks;
using UnityEngine;


public class QuadMovementController : MonoBehaviour
{
  #region Serialized Fields
  #endregion

  #region Private Fields
  private const float ANGLE_PER_CLICK = 90.0f;
  private const float ROTATION_TIME = 0.9f;
  private const float SCALING_TIME = 0.2f;
  private const float MIN_SCALE = 0.8f;
  private const float MAX_SCALE = 1.0f;
  private Task rotation_task = Task.CompletedTask;
  private float target_rotation = 0.0f;
  private float rotation_time_left = 0.0f;
  private float scaling_time_left = 0.0f;
  private Vector3 cached_scale = Vector3.one;
  #endregion

  #region Public Fields
  public event Action<float> onRotate = delegate{};
  #endregion


  #region Private Methods
  private void OnMouseDown()
  {
    rotateOverTime();
  }

  private async void rotateOverTime()
  {
    rotation_time_left = ROTATION_TIME;
    scaling_time_left = SCALING_TIME;
    target_rotation = target_rotation + ANGLE_PER_CLICK;

    if ( rotation_task.IsCompleted )
      rotation_task = rotateHex();

    async Task rotateHex()
    {
      while( rotation_time_left > ROTATION_TIME / 2 )
      {
        transform.rotation = Quaternion.Lerp(
            Quaternion.Euler( 0.0f, target_rotation, 0.0f )
          , transform.rotation
          , rotation_time_left / ROTATION_TIME );

        float scale = Mathf.Lerp( MAX_SCALE, MIN_SCALE, scaling_time_left / SCALING_TIME );
        cached_scale.x = scale;
        cached_scale.z = scale;

        transform.localScale = cached_scale;
        rotation_time_left -= Time.deltaTime;

        scaling_time_left -= Time.deltaTime;
        await Task.Yield();
      }
      onRotate.Invoke( transform.rotation.eulerAngles.y );
    }
  }

  private void forcestopRotation()
  {
    rotation_task.Dispose();
    rotation_task = Task.CompletedTask;
    transform.rotation = Quaternion.Euler( 0.0f, target_rotation, 0.0f );
    cached_scale = Vector3.one;
    transform.localScale = cached_scale;
  }
  #endregion
}

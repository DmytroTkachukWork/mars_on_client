using System.Threading.Tasks;
using UnityEngine;


public class HexagonController : MonoBehaviourBase
{
  #region Serialized Fields
  #endregion

  #region Private Fields
  private const float ANGLE_PER_CLICK = 60.0f;
  private const float ROTATION_TIME = 1.8f;
  private Task rotation_task = Task.CompletedTask;
  private float target_rotation = 0.0f;
  private float rotation_time_left = 0.0f;
  #endregion


  #region Private Methods
  private void OnMouseDown()
  {
    rotateOverTime();
  }

  private async void rotateOverTime()
  {
    rotation_time_left = ROTATION_TIME;
    target_rotation = target_rotation + ANGLE_PER_CLICK;
    if ( rotation_task.IsCompleted )
      rotation_task = rotateHex();

    async Task rotateHex()
    {
      while( rotation_time_left > 0.0f )
      {
        transform.rotation = Quaternion.Lerp(
            Quaternion.Euler( 0.0f, target_rotation, 0.0f )
          , transform.rotation
          , rotation_time_left / ROTATION_TIME );
        rotation_time_left -= Time.deltaTime;
        await Task.Yield();
      }
    }
  }

  private void forcestopRotation()
  {
    rotation_task.Dispose();
    rotation_task = Task.CompletedTask;
    transform.rotation = Quaternion.Euler( 0.0f, target_rotation, 0.0f );
  }
  #endregion
}

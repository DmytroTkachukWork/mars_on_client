using System;
using System.Collections;
using UnityEngine;

public class CellMovementController : MonoBehaviourBase
{
  #region Serialized Fields
  [SerializeField] private Transform rotation_root = null;
  #endregion

  #region Private Fields
  private const float ANGLE_PER_CLICK = 90.0f;
  private IEnumerator rotation_cor = null;
  private bool rotation_cor_finished = true;
  private float target_rotation = 0.0f;
  private float rotation_time_left = 0.0f;
  #endregion

  #region Public Fields
  public event Action onRotate = delegate{};
  public event Action onBeginRotate = delegate{};
  #endregion


  #region Public Methods
  public void init( RotationStage start_rotation )
  {
    target_rotation = (int)start_rotation * ANGLE_PER_CLICK;
    rotation_root.localRotation = Quaternion.Euler( 0.0f, target_rotation, 0.0f );
    rotation_time_left = 0.0f;
  }

  public void deinit()
  {
    forceStopRotation();
  }

  public void rotateOverTime( RotationStage rotation )
  {
    rotation_time_left = myVariables.QUAD_ROTATION_TIME;
    target_rotation = (int)rotation * ANGLE_PER_CLICK;

    onBeginRotate.Invoke();

    if ( rotation_cor_finished )
      rotation_cor = rotation_cor.startCoroutineAndStopPrev( rotateHex() );

    IEnumerator rotateHex()
    {
      rotation_cor_finished = false;
      while( rotation_time_left > myVariables.QUAD_ROTATION_TIME / 1.5 )
      {
        rotation_root.localRotation = Quaternion.Lerp(
            Quaternion.Euler( 0.0f, target_rotation, 0.0f )
          , rotation_root.localRotation
          , rotation_time_left / myVariables.QUAD_ROTATION_TIME );

        rotation_time_left -= Time.deltaTime;


        if ( rotation_root.localRotation.eulerAngles.y >= target_rotation - 1.0f && rotation_root.localRotation.eulerAngles.y <= target_rotation + 1.0f )
        {
          rotation_root.localRotation = Quaternion.Euler( 0.0f, target_rotation, 0.0f );
          break;
        }

        yield return null;
      }
      rotation_cor_finished = true;

      rotation_root.localRotation = Quaternion.Euler( 0.0f, target_rotation, 0.0f );
      onRotate.Invoke();
    }
  }
  #endregion

  #region Private Methods
  private void forceStopRotation()
  {
    rotation_cor?.stop();
    rotation_cor_finished = true;
    rotation_root.localRotation = Quaternion.Euler( 0.0f, target_rotation, 0.0f );
  }
  #endregion
}

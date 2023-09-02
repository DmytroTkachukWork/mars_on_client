using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Tweener : MonoBehaviourService<Tweener>
{
  [SerializeField] private AnimationCurve ease_in_curve = null;
  [SerializeField] private AnimationCurve ease_out_curve = null;
  [SerializeField] private AnimationCurve ease_in_out_curve = null;
  [SerializeField] private AnimationCurve wave_curve = null;


  public IEnumerator tweenFloat( Action<float> func, float start_value, float finish_value, float time, Action callback, CurveType curve_type = CurveType.NONE )
  {
    float time_spend = 0.0f;
    float progress = 0.0f;

    while( time_spend <= time ) 
    {
      progress = culcCurve( curve_type, time_spend / time );

      func.Invoke( Mathf.Lerp( start_value, finish_value, progress ) );
      time_spend += Time.deltaTime;
      yield return null;
    }
    func.Invoke( finish_value );
    callback?.Invoke();
  }

  public IEnumerator tweenColor( Action<Color> func, Color start_value, Color finish_value, float time, Action callback, CurveType curve_type = CurveType.NONE )
  {
    float time_spend = 0.0f;
    float progress = 0.0f;

    while( time_spend <= time ) 
    {
      progress = culcCurve( curve_type, time_spend / time );

      func.Invoke( Color.Lerp( start_value, finish_value, progress ) );
      time_spend += Time.deltaTime;
      yield return null;
    }
    func.Invoke( finish_value );
    callback?.Invoke();
  }

  public IEnumerator tweenVector( Action<Vector3> func, Vector3 start_value, Vector3 finish_value, float time, Action callback, CurveType curve_type = CurveType.NONE )
  {
    float time_spend = 0.0f;
    float progress = 0.0f;

    while( time_spend <= time ) 
    {
      progress = culcCurve( curve_type, time_spend / time );

      func.Invoke( Vector3.Lerp( start_value, finish_value, progress ) );
      time_spend += Time.deltaTime;
      yield return null;
    }
    func.Invoke( finish_value );
    callback?.Invoke();
  }

  public IEnumerator tweenTransform( Transform curtent_transform, Transform target_transform, float time, Action callback = null, CurveType curve_type = CurveType.NONE )
  {
    Vector3 pos = curtent_transform.position;
    Quaternion rot = curtent_transform.rotation;
    Vector3 scale = curtent_transform.localScale;

    float time_spend = 0.0f;
    float progress = 0.0f;
    while( time_spend <= time ) 
    {
      yield return null;
      time_spend += Time.deltaTime;
      progress = culcCurve( curve_type, time_spend / time );

      curtent_transform.position = Vector3.Lerp( pos, target_transform.position, progress );
      curtent_transform.rotation = Quaternion.Lerp( rot, target_transform.rotation, progress );
      curtent_transform.localScale = Vector3.Lerp( scale, target_transform.localScale, progress );
    }


    curtent_transform.position = target_transform.position;
    curtent_transform.rotation = target_transform.rotation;
    curtent_transform.localScale = target_transform.localScale;
    callback?.Invoke();
  }

  public IEnumerator tweenRotation( Transform curtent_transform, Quaternion rotation, float time, Action callback = null, CurveType curve_type = CurveType.NONE )
  {
    Vector3 pos = curtent_transform.position;
    Quaternion rot = curtent_transform.rotation;
    Vector3 scale = curtent_transform.localScale;

    float time_spend = 0.0f;
    float progress = 0.0f;
    while( time_spend <= time ) 
    {
      yield return null;
      time_spend += Time.deltaTime;
      progress = culcCurve( curve_type, time_spend / time );

      curtent_transform.rotation = Quaternion.Lerp( rot, rotation, progress );
    }

    curtent_transform.rotation = rotation;
    callback?.Invoke();
  }

  public IEnumerator waitAndDo( Action func, float time )
  {
    yield return new WaitForSeconds( time );
    func?.Invoke();
  }

  public IEnumerator waitFrameAndDo( Action func )
  {
    yield return null;
    func?.Invoke();
  }

  public IEnumerator waitAndDoCycle( Action func, float cycle_time, float full_time, Action callback )
  {
    WaitForSeconds wait_for_seconds = new WaitForSeconds( cycle_time );
    float cached_time_left = full_time;

    while( cached_time_left >= 0.0f )
    {
      yield return wait_for_seconds;
      cached_time_left -= cycle_time;
      func?.Invoke();
    }

    callback?.Invoke();
  }

  public IEnumerator updateUntil( Action func )
  {
    while( true )
    {
      func?.Invoke();
      yield return null;
    }
  }

  public AnimationCurve getCurve( CurveType curve_type )
  {
    switch( curve_type )
    {
    case CurveType.EASE_IN : return ease_in_curve;
    case CurveType.EASE_OUT : return ease_out_curve;
    case CurveType.EASE_IN_OUT : return ease_in_out_curve;
    case CurveType.WAVE : return wave_curve;
    default : return null;
    }
  }

  public float culcCurve( CurveType curve_type, float curent_progress )
  {
    if ( curve_type != CurveType.NONE )
      curent_progress = getCurve( curve_type ).Evaluate( curent_progress );

    return curent_progress;
  }
}

public enum CurveType
{
  NONE = 0,
  EASE_IN = 1,
  EASE_OUT = 2,
  EASE_IN_OUT = 3,
  WAVE = 4
}
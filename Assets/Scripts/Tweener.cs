using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Tweener : MonoBehaviourService<Tweener>
{
  [SerializeField] private AnimationCurve ease_in_curve = null;
  [SerializeField] private AnimationCurve ease_out_curve = null;
  [SerializeField] private AnimationCurve ease_in_out_curve = null;
  private static MyTaskPool tasks_pool = new MyTaskPool();

  public static MyTask getEmptyMyTask()
  {
    return tasks_pool.get();
  }

  public MyTask tween( Action<float> func, float start_value, float finish_value, float time, Action callback )
  {
    MyTask my_task = tasks_pool.get();
    my_task.curent_task = perform();
    return my_task;

    async Task perform()
    {
      float time_left = 0.0f;
      while( time_left <= time && !my_task.cencel_token ) 
      {
        func.Invoke( Mathf.Lerp( start_value, finish_value, time_left / time ) );
        time_left += Time.deltaTime;
        await Task.Yield();
      }

      if ( !my_task.cencel_token )
        callback?.Invoke();
    }
  }

  public MyTask tweenTransform( Transform curtent_transform, Transform target_transform, float time, Action callback = null, CurveType curve_type = CurveType.NONE )
  {
    Vector3 pos = curtent_transform.position;
    Quaternion rot = curtent_transform.rotation;
    Vector3 scale = curtent_transform.localScale;
    MyTask my_task = tasks_pool.get();
    my_task.curent_task = perform();
    return my_task;

    async Task perform()
    {
      float time_left = 0.0f;
      float progress = 0.0f;
      while( time_left <= time && !my_task.cencel_token ) 
      {
        progress = time_left / time;

        if ( curve_type == CurveType.NONE )
          progress = getCurve( curve_type ).Evaluate( progress );

        curtent_transform.position = Vector3.Lerp( pos, target_transform.position, progress );
        curtent_transform.rotation = Quaternion.Lerp( rot, target_transform.rotation, progress );
        curtent_transform.localScale = Vector3.Lerp( scale, target_transform.localScale, progress );
        time_left += Time.deltaTime;
        await Task.Yield();
      }

      if ( my_task.cencel_token )
        return;

      curtent_transform.position = target_transform.position;
      curtent_transform.rotation = target_transform.rotation;
      curtent_transform.localScale = target_transform.localScale;
      callback?.Invoke();
    }
  }

  public MyTask tweenPosition( Transform curtent_transform, Transform target_transform, float time, Action callback = null )
  {
    MyTask my_task = tasks_pool.get();
    my_task.curent_task = perform();
    return my_task;

    async Task perform()
    {
      float time_left = 0.0f;
      float progress = 0.0f;
      while( time_left <= time && !my_task.cencel_token ) 
      {
        progress = time_left / time;
        curtent_transform.position = Vector3.Lerp( curtent_transform.position, target_transform.position, progress );
        time_left += Time.deltaTime;
        await Task.Yield();
      }
      callback?.Invoke();
    }
  }

  public MyTask tweenRotation( Transform curtent_transform, Transform target_transform, float time, Action callback = null )
  {
    MyTask my_task = tasks_pool.get();
    my_task.curent_task = perform();
    return my_task;

    async Task perform()
    {
      float time_left = 0.0f;
      float progress = 0.0f;
      while( time_left <= time && !my_task.cencel_token ) 
      {
        progress = time_left / time;
        curtent_transform.rotation = Quaternion.Lerp( curtent_transform.rotation, target_transform.rotation, progress );
        time_left += Time.deltaTime;
        await Task.Yield();
      }
      callback?.Invoke();
    }
  }

  public MyTask waitAndDo( Action func, float time )
  {
    MyTask my_task = tasks_pool.get();
    my_task.curent_task = perform();
    return my_task;

    async Task perform()
    {
      float ceched_check_delay = time;
      while( ceched_check_delay > 0.0f && !my_task.cencel_token )
      {
        ceched_check_delay -= Time.deltaTime;
        await Task.Yield();
      }

      if ( !my_task.cencel_token )
        func?.Invoke();
    }
  }

  public MyTask updateUntil( Action func )
  {
    MyTask my_task = tasks_pool.get();
    my_task.curent_task = perform();
    return my_task;

    async Task perform()
    {
      while( !my_task.cencel_token )
      {
        func?.Invoke();
        await Task.Yield();
      }
    }
  }

  public AnimationCurve getCurve( CurveType curve_type )
  {
    switch( curve_type )
    {
    case CurveType.EASE_IN : return ease_in_curve;
    case CurveType.EASE_OUT : return ease_out_curve;
    case CurveType.EASE_IN_OUT : return ease_in_out_curve;
    default : return null;
    }
  }
}

public class MyTask
{
  #region Public Fields
  public Task curent_task = Task.CompletedTask;
  public bool cencel_token = false;
  #endregion

  #region Public Methods
  public void init()
  {
    cencel_token = false;
  }

  public void stop()
  {
    cencel_token = true;
  }
  #endregion
}

public class MyTaskPool
{
  private List<MyTask> tasks_pool = new List<MyTask>();

  public MyTask get()
  {
    foreach( MyTask task in tasks_pool )
    {
      if ( !task.curent_task.IsCompleted )
        continue;

      task.init();
      return task;
    }

    MyTask new_task = new MyTask();
    new_task.init();
    tasks_pool.Add( new_task );
    return new_task;
  }
}

public enum CurveType
{
  NONE = 0,
  EASE_IN = 1,
  EASE_OUT = 2,
  EASE_IN_OUT = 3
}
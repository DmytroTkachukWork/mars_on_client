using System;
using System.Threading.Tasks;
using UnityEngine;

public class Awaiter
{
  private float ceched_check_delay = 1.0f;
  private Task awaiting_task = Task.CompletedTask;
  private bool is_stoped = false;
  public void waitAndDo( Action func, float time, bool restart = true )
  {
    ceched_check_delay = time;
    is_stoped = false;
    if ( awaiting_task.IsCompleted )
      awaiting_task = wait();

    async Task wait()
    {
      while( ceched_check_delay > 0.0f && !is_stoped )
      {
        ceched_check_delay -= Time.deltaTime;
        await Task.Yield();
      }

      if ( !is_stoped )
        func.Invoke();
    }
  }

  public void stop()
  {
    is_stoped = true;
  }
}

public class Tweener
{
  private Task tween_task = Task.CompletedTask;
  private float time_left = 0.0f;
  private bool is_stoped = false;

  public void tween( Action<float> func, float start_value, float finish_value, float time, Action callback )
  {
    is_stoped = false;
    time_left = 0.0f;
    if ( tween_task.IsCompleted )
      tween_task = perform();

    async Task perform()
    {
      while( time_left <= time && !is_stoped )
      {
        func.Invoke( Mathf.Lerp( start_value, finish_value, time_left / time ) );
        time_left += Time.deltaTime;
        await Task.Yield();
      }
      Debug.LogError( "callback.Invoke()" );
      callback.Invoke();
    }
  }

  public void stop()
  {
    is_stoped = true;
  }

}
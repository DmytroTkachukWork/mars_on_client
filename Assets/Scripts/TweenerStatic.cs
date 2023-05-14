using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public static class TweenerStatic
{
  private static MyTaskPool tasks_pool = new MyTaskPool();

  public static MyTask getEmptyMyTask()
  {
    return tasks_pool.get();
  }

  public static MyTask tween( Action<float> func, float start_value, float finish_value, float time, Action callback )
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
      callback?.Invoke();
    }
  }

  public static MyTask waitAndDo( Action func, float time )
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

  public static MyTask updateUntil( Action func )
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
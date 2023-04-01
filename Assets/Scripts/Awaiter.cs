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
      while( ceched_check_delay > 0.0f )
      {
        if ( is_stoped )
          return;
        await Task.Yield();
        ceched_check_delay -= Time.deltaTime;
      }
      func.Invoke();
    }
  }

  public void stop()
  {
    is_stoped = true;
  }
}
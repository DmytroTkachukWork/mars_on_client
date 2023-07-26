using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeExecutor
{
  private List<Action> cached_actions = new List<Action>();
  private Action[] cached_actions_copy = null;

  public void execute()
  {
    while ( cached_actions.Count > 0 )
    {
      cached_actions_copy = new Action[cached_actions.Count];
      cached_actions.CopyTo( cached_actions_copy );
      cached_actions.Clear();

      for ( int i = 0; i < cached_actions_copy.Length; i++ )
      {
        cached_actions_copy[i].Invoke();
      }
    }
  }

  public void addAction( Action action )
  {
    cached_actions.Add( action );
  }
}

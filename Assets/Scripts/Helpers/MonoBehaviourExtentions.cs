using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtentions
{
  public static void despawn( this MonoBehaviourPoolable poolable )
  {
    poolable.onDespawn();
  }
}

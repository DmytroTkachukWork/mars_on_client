using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourBase : MonoBehaviour
{
  private SpawnManager spawn_manager = null;
  protected SpawnManager spawnManager { get 
  {
    if ( spawn_manager == null )
      spawn_manager = FindObjectOfType<SpawnManager>();

    return spawn_manager;
  }
  private set{}}
}

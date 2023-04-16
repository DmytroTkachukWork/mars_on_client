using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourPoolable : MonoBehaviourBase
{
  #region Protected Fields
  protected bool is_spawned = false;
  #endregion

  #region Public Methods
  public virtual void onSpawn()
  {
    is_spawned = true;
    this.gameObject.SetActive( true );
  }

  public virtual void onDespawn()
  {
    is_spawned = false;
    this.gameObject.SetActive( false );
  }

  public bool isAvailuableToSpawn()
  {
    return !is_spawned;
  }
  #endregion
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class CellControllerBase : MonoBehaviour
{
  public virtual void init()
  {
  }

  public virtual void deinit()
  {
  }

  public virtual void fillFrom( CellPipeDirection[] directions ) // call to start filling pipe from direction
  {
  }

  public event Action<CellPipeDirection[], Action> onFinishFill = delegate{}; // invoke after filling and return free dirs and callback for spawnWaterFall()

  public virtual void spawnWaterFall( CellPipeDirection[] directions )
  {
  }

  public virtual void killFilling() // on pipe disconection
  {
  }
}

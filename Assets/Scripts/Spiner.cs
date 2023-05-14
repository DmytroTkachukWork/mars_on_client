using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Spiner : MonoBehaviour
{
  #region Serialized Fields
  [SerializeField] private Vector3 spin_speed = Vector3.zero;
  #endregion
  #region Private Fields
  private Task rotate_task = Task.CompletedTask;
  private bool is_cenceled = false;
  #endregion

  #region Public Methods
  public void init()
  {
    is_cenceled = false;
    if ( rotate_task == Task.CompletedTask )
      rotate_task = rotate();
  
    async Task rotate()
    {
      while( !is_cenceled )
      {
        transform.Rotate( spin_speed.x, spin_speed.y, spin_speed.z );
        await Task.Yield();
      }
    }
  }

  public void deinit()
  {
    is_cenceled = true;
    rotate_task = Task.CompletedTask;
  }
  #endregion
}

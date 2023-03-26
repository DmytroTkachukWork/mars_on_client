using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ManagerHelper : MonoBehaviour
{
  #region Private Fields
  private static SpawnManager spawn_manager = null;
  #endregion

  #region Public Fields
  public SpawnManager spawnManager = get<SpawnManager>( ref spawn_manager );
  #endregion


  #region Private Methods
  private static T get<T>( ref T instance_ref ) where T : MonoBehaviour
  {
    if ( instance_ref != null && instance_ref is T )
      return (T)instance_ref;

    var instance = FindObjectOfType<T>();
    instance_ref = instance;
    return instance;
  }
  #endregion
}

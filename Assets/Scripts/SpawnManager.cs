using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpawnManager : MonoBehaviour
{
  #region Serialized Fields
  [SerializeField] private QuadContentController quad_prefab = null;
  [SerializeField] private ConectorController[] conector_prefabs = null;
  #endregion


  #region Public Methods
  public QuadContentController spawnQuad( Vector3 position )
  {
    return Instantiate( quad_prefab, position, Quaternion.identity );
  }

  public void spawnConector( Transform root_transform, QuadConectionType conection_type )
  {
    ConectorController conector_controller = conector_prefabs.FirstOrDefault( x => x.conectionType == conection_type );
    Instantiate( conector_controller, root_transform );
  }
  #endregion
}

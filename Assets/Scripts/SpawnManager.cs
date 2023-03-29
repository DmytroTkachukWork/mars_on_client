using UnityEngine;
using System.Linq;


public class SpawnManager : MonoBehaviour
{
  #region Serialized Fields
  [SerializeField] private QuadContentController quad_prefab = null;
  [SerializeField] private ConectorController[] conector_prefabs = null;
  [SerializeField] private QuadSetUpController quad_setup_prefab = null;
  #endregion


  #region Public Methods
  public QuadContentController spawnQuad( Vector3 position )
  {
    return Instantiate( quad_prefab, position, Quaternion.identity );
  }

  public QuadSetUpController spawnQuadSetUp( Vector3 position )
  {
    return Instantiate( quad_setup_prefab, position, Quaternion.identity );
  }

  public ConectorController spawnConector( Transform root_transform, QuadConectionType conection_type )
  {
    ConectorController conector_controller = conector_prefabs.FirstOrDefault( x => x.conectionType == conection_type );
    return Instantiate( conector_controller, root_transform );
  }
  #endregion
}

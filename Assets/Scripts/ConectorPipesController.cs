using UnityEngine;

public class ConectorPipesController : MonoBehaviour
{
  #region Serialized Fields
  [SerializeField] private QuadConectionType conection_type = QuadConectionType.NONE;
  [SerializeField] private PipeRosourceController[] pipe_controllers = null;
  #endregion

  #region Public Fields
  public QuadConectionType conectionType => conection_type;
  #endregion


  #region Public Methods
  public void paintConected( QuadResourceType resource_type, int inner_dir = 0 )
  {
    if ( resource_type == QuadResourceType.NONE )
    {
      foreach( PipeRosourceController pipe_controller in pipe_controllers )
        pipe_controller?.setResourceAndFill( resource_type, true );

      return;
    }

    if ( conection_type == QuadConectionType.TWO_CORNERS )
    {
      pipe_controllers[inner_dir].setResourceAndFill( resource_type, true );
      pipe_controllers[inner_dir + (inner_dir % 2 == 0 ? 1 : -1)].setResourceAndFill( resource_type, true );
      return;
    }

    foreach( PipeRosourceController pipe_controller in pipe_controllers )
      pipe_controller?.setResourceAndFill( resource_type, true );
  }
  #endregion
}

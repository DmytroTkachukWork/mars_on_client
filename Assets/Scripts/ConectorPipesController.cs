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
  public bool paintConected( QuadResourceType resource_type, int inner_dir = 0 )
  {
    bool was_overpainted = false;
    if ( resource_type == QuadResourceType.NONE )
    {
      foreach( PipeRosourceController pipe_controller in pipe_controllers )
      {
        if ( pipe_controller == null )
          continue;

        if ( pipe_controller.setResourceAndFill( resource_type, true ) )
          was_overpainted = true;
      }

      return was_overpainted;
    }

    if ( conection_type == QuadConectionType.TWO_CORNERS )
    {
      if ( pipe_controllers[inner_dir].setResourceAndFill( resource_type, true ) )
        was_overpainted = true;
        
      if ( pipe_controllers[inner_dir + (inner_dir % 2 == 0 ? 1 : -1)].setResourceAndFill( resource_type, true ) )
        was_overpainted = true;

      return was_overpainted;
    }

    foreach( PipeRosourceController pipe_controller in pipe_controllers )
    {
      if ( pipe_controller == null )
          continue;

      if (  pipe_controller.setResourceAndFill( resource_type, true ) )
        was_overpainted = true;
    }
    return was_overpainted;
  }
  #endregion
}

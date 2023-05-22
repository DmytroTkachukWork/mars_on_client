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
      for( int i = 0; i < pipe_controllers.Length; i++ )
      {
        if ( pipe_controllers[i] == null )
          continue;

        if ( pipe_controllers[i].setResourceAndFill( resource_type, i == inner_dir ) )
          was_overpainted = true;
      }

      return was_overpainted;
    }

    if ( conection_type == QuadConectionType.TWO_CORNERS )
    {
      if ( pipe_controllers[inner_dir].setResourceAndFill( resource_type, true ) )
        was_overpainted = true;
        
      if ( pipe_controllers[inner_dir + (inner_dir % 2 == 0 ? 1 : -1)].setResourceAndFill( resource_type, false ) )
        was_overpainted = true;

      return was_overpainted;
    }

    for( int i = 0; i < pipe_controllers.Length; i++ )
    {
      if ( pipe_controllers[i] == null )
          continue;

      if ( pipe_controllers[i].setResourceAndFill( resource_type, i == inner_dir ) )
        was_overpainted = true;
    }
    return was_overpainted;
  }
  #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectorPipesController : MonoBehaviourBase
{
  #region Serialized Fields
  [SerializeField] private QuadConectionType conection_type = QuadConectionType.NONE;
  [SerializeField] private PipeRosourceController[] pipe_controllers = null;
  [SerializeField] private PipeResourceController pipe_resource_controller = null;
  #endregion

  #region Public Fields
  public QuadConectionType conectionType => conection_type;
  #endregion


  #region Public Methods
  public void deinit()
  {
    pipe_resource_controller?.deinit();
  }

  public void paintConectedCor( QuadResourceType resource_type, int inner_dir = 0, HashSet<Pipe> next_pipes = null, Action<HashSet<Pipe>> callback = null )
  {
    pipe_resource_controller.fillRecource( resource_type, inner_dir, next_pipes, callback );
  }

  public bool isPaintingInProgress()
  {
    for( int i = 0; i < pipe_controllers.Length; i++ )
    {
      if ( pipe_controllers[i] == null )
        continue;

      if ( pipe_controllers[i].isPaintInProgress() )
        return true;
    }
    return false;
  }
  #endregion
}

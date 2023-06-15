using System;
using System.Collections;
using System.Collections.Generic;
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

  #region Private Fields
  private IEnumerator paint_cor = null;
  private QuadResourceType _resource_type = QuadResourceType.NONE;
  private List<Pipe> cached_pipes = null;
  #endregion


  #region Public Methods
  public void paintConectedCor( QuadResourceType resource_type, int inner_dir = 0, List<Pipe> next_pipes = null, Action<List<Pipe>> callback = null )
  {
    cached_pipes = next_pipes;

    if ( _resource_type == resource_type )
    {
      if ( !isPaintingInProgress() )
        callback?.Invoke( cached_pipes );

      return;
    }

    _resource_type = resource_type;
    paint_cor = paint_cor.startCoroutineAndStopPrev( impl() );

    IEnumerator impl()
    {
      if ( _resource_type == QuadResourceType.NONE )
      {
        for( int i = 0; i < pipe_controllers.Length; i++ )
        {
          if ( pipe_controllers[i] == null )
            continue;

          yield return pipe_controllers[i].setResourceAndFill( _resource_type, false );
        }

        yield break;
      }

      if ( conection_type == QuadConectionType.TWO_CORNERS )
      {
        yield return pipe_controllers[inner_dir].setResourceAndFill( _resource_type, true );
        yield return pipe_controllers[inner_dir + (inner_dir % 2 == 0 ? 1 : -1)].setResourceAndFill( _resource_type, false );
        yield break;
      }

      if ( pipe_controllers[inner_dir] != null )
      {
        yield return pipe_controllers[inner_dir].setResourceAndFill( _resource_type, true );
      }

      for( int i = 0; i < pipe_controllers.Length; i++ )
      {
        if ( pipe_controllers[i] == null || i == inner_dir )
            continue;

        yield return pipe_controllers[i].setResourceAndFill( _resource_type, false );//TODO paralel
      }
      callback?.Invoke( cached_pipes );
    }
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeResourceControllerOneSide : PipeResourceController
{
  public override bool fillRecource( QuadResourceType resource_type, int inner_dir = 0, Action callback = null )
  {
    if ( _resource_type == resource_type )
    {
      if ( !is_painting_in_progress )
        callback?.Invoke();

      return true;
    }

    _resource_type = resource_type;

    fill_cor = fill_cor.startCoroutineAndStopPrev( impl() );
    return false;

    IEnumerator impl()
    {
      int start_point  = inner_dir == 0 ? 0 : resource_entity_roots.Length;
      int finish_point = inner_dir == 0 ? resource_entity_roots.Length : 0;

      is_painting_in_progress = true;
      for ( int i = start_point; i < finish_point; i++ )
      {
        ResourceEntityController rec = spawnManager.spawnRec( resource_entity_roots[i] );
        spawned_rec.Add( rec );
        yield return rec.playAnim( (float)i / (float)finish_point );
      }

      is_painting_in_progress = false;
      callback?.Invoke();
    }
  }
}

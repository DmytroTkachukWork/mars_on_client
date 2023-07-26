using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeResourceControllerOneSide : PipeResourceController
{
  public override void fillRecource( QuadResourceType resource_type, int inner_dir = 0, HashSet<Pipe> next_pipes = null, Action<HashSet<Pipe>> callback = null )
  {
    cached_pipes = next_pipes;

    if ( _resource_type == resource_type )
    {
      if ( !is_painting_in_progress )
        callback?.Invoke( cached_pipes );

      return;
    }

    _resource_type = resource_type;

    fill_cor = fill_cor.startCoroutineAndStopPrev( impl() );

    IEnumerator impl()
    {
      int start_point  = inner_dir == 0 ? 0 : resource_entity_roots.Length;
      int finish_point = inner_dir == 0 ? resource_entity_roots.Length : 0;

      is_painting_in_progress = true;
      for ( int i = start_point; i < finish_point; i++ )
      {
        ResourceEntityController rec = spawnManager.spawnRec( resource_entity_roots[i] );
        spawned_rec.Add( rec );
        yield return rec.playAnim();
      }

      is_painting_in_progress = false;
      callback?.Invoke( cached_pipes );
    }
  }
}

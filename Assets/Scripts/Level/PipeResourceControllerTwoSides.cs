using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeResourceControllerTwoSides : PipeResourceController
{
  protected override IEnumerator fillCoroutine( int inner_dir, Action<HashSet<Pipe>> callback )
  {
    is_painting_in_progress = true;
    if ( inner_dir == 1 )
    {
      for ( int i = 0; i < resource_entity_roots.Length; i++ )
        yield return impl( i );
    }
    else
    {
      for ( int i = resource_entity_roots.Length-1; i >= 0; i-- )
        yield return impl( i );
    }

    is_painting_in_progress = false;
    callback?.Invoke( cached_pipes );

    IEnumerator impl( int i )
    {
      ResourceEntityController rec = spawnManager.spawnRec( resource_entity_roots[i] );
      spawned_rec.Add( rec );
      yield return rec.playAnim( (float)i / (float)resource_entity_roots.Length );
    }
  }
}

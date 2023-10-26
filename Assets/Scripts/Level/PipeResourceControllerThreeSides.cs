using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeResourceControllerThreeSides : PipeResourceController
{
  [SerializeField] protected Transform[] from_0_roots = null;
  [SerializeField] protected Transform[] from_1_roots = null;
  [SerializeField] protected Transform[] from_3_roots = null;

  protected override IEnumerator fillCoroutine( int inner_dir, Action callback )
  {
    is_painting_in_progress = true;

    if ( inner_dir == 0 )
    {
      for ( int i = 0; i < from_0_roots.Length; i++ )
        yield return impl( from_0_roots, i );

      for ( int i = from_3_roots.Length-1; i >= 0; i-- )
      {
        implAsync( from_1_roots, i );
        yield return impl( from_3_roots, i );
      }
    }

    if ( inner_dir == 1 )
    {
      for ( int i = 0; i < from_1_roots.Length; i++ )
        yield return impl( from_1_roots, i );

      for ( int i = from_0_roots.Length-1; i >= 0; i-- )
      {
        implAsync( from_3_roots, i );
        yield return impl( from_0_roots, i );
      }
    }

    if ( inner_dir == 3 )
    {
      for ( int i = 0; i < from_3_roots.Length; i++ )
        yield return impl( from_3_roots, i );

      for ( int i = from_0_roots.Length-1; i >= 0; i-- )
      {
        implAsync( from_1_roots, i );
        yield return impl( from_0_roots, i );
      }
    }

    is_painting_in_progress = false;
    callback?.Invoke();

    IEnumerator impl( Transform[] roots, int i )
    {
      ResourceEntityController rec = spawnManager.spawnRec( roots[i] );
      spawned_rec.Add( rec );
      yield return rec.playAnim( (float)i / (float)roots.Length );
    }

    void implAsync( Transform[] roots, int i )
    {
      ResourceEntityController rec = spawnManager.spawnRec( roots[i] );
      spawned_rec.Add( rec );
      rec.playAnimAsync( (float)i / (float)roots.Length );
    }
  }
}

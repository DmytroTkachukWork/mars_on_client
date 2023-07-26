using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeResourceController : MonoBehaviourBase
{
  [SerializeField] protected Transform[] resource_entity_roots = null;
  protected List<ResourceEntityController> spawned_rec = new List<ResourceEntityController>();
  protected IEnumerator fill_cor = null;
  protected HashSet<Pipe> cached_pipes = null;
  protected QuadResourceType _resource_type = QuadResourceType.NONE;
  protected bool is_painting_in_progress = false;

  public virtual void init()
  {

  }

  public virtual void deinit()
  {
    fill_cor?.stop();
    is_painting_in_progress = false;

    foreach ( ResourceEntityController entity in spawned_rec )
      entity.despawn();

    spawned_rec.Clear();
    _resource_type = QuadResourceType.NONE; 
  }

  public virtual void fillRecource( QuadResourceType resource_type, int inner_dir = 0, HashSet<Pipe> next_pipes = null, Action<HashSet<Pipe>> callback = null )
  {
    cached_pipes = next_pipes;

    if ( _resource_type == resource_type )
    {
      if ( !is_painting_in_progress )
        callback?.Invoke( cached_pipes );

      return;
    }

    _resource_type = resource_type;

    if ( _resource_type == QuadResourceType.NONE )
    {
      _resource_type = resource_type;
      deinit();
      return;
    }
    
    fill_cor = fill_cor.startCoroutineAndStopPrev( fillCoroutine( inner_dir, callback ) );
  }

  protected virtual IEnumerator fillCoroutine( int inner_dir, Action<HashSet<Pipe>> callback )
  {
    is_painting_in_progress = true;
    if ( inner_dir == 0 )
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
      yield return rec.playAnim();
    }
  }
}

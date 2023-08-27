using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceEntityController : MonoBehaviourPoolable
{
  [SerializeField] private Transform root_transform  = null;
  private IEnumerator anim_cor = null;

  public void init()
  {

  }

  public void deinit()
  {
    anim_cor?.stop();
    root_transform.localScale = Vector3.zero;
  }

  public override void onSpawn()
  {
    base.onSpawn();

    init();
  }

  public override void onDespawn()
  {
    deinit();

    base.onDespawn();
  }

  public IEnumerator playAnim( float filling_percent )
  {
    play( filling_percent );
    yield return anim_cor;
  }

  public void playAnimAsync( float filling_percent )
  {
    play( filling_percent );
    anim_cor.start();
  }

  private void play( float filling_percent )
  {
    anim_cor?.stop();
    anim_cor = tweener.tweenVector( 
        ( value ) => root_transform.localScale = value
      , Vector3.zero
      , Vector3.one
      , myVariables.RESOURCE_ENTITY_SCALE_TIME * tweener.getCurve( CurveType.WAVE ).Evaluate( filling_percent )
      , null
    );
  }
}

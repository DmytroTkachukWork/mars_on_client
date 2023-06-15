using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PipeRosourceController : MonoBehaviourBase
{
  #region Serialized Fields
  [SerializeField] private MeshRenderer[]    resource_renderers  = null;
  [SerializeField] private ResourceMatPair[] resources_pairs     = null;
  [SerializeField] private Transform         in_scale_transform  = null;
  [SerializeField] private Transform         out_scale_transform = null;
  #endregion

  #region Private Fields
  private const float MIN_SCALE  = 0.0f;
  private const float MAX_SCALE  = 1.0f;
  private IEnumerator scale_cor = null;
  private QuadResourceType resource_type = QuadResourceType.NONE;
  private bool is_painting_in_progress = false;
  #endregion

  #region Public Methods
  public bool isPaintInProgress()
  {
    return is_painting_in_progress;
  }

  public IEnumerator setResourceAndFill( QuadResourceType resource_type, bool is_incoming )
  {
    if ( this.resource_type == resource_type )
    {
      yield break;
    }

    this.resource_type = resource_type;

    foreach ( MeshRenderer renderer in resource_renderers )
      renderer.material = resources_pairs.FirstOrDefault( x => x.resource_type == resource_type ).material;

    in_scale_transform.localScale = setZ( in_scale_transform.localScale, MIN_SCALE );
    in_scale_transform.gameObject.SetActive( false );
    out_scale_transform.localScale = setZ( out_scale_transform.localScale, MIN_SCALE );
    out_scale_transform.gameObject.SetActive( false );

    if ( resource_type == QuadResourceType.NONE )
      yield break;

    Transform cached_transform = is_incoming ? in_scale_transform : out_scale_transform;
    cached_transform.gameObject.SetActive( true );
    is_painting_in_progress = true;

    scale_cor = tweener.tweenFloat(
        ( value ) => cached_transform.localScale = setZ( cached_transform.localScale, value )
      , MIN_SCALE
      , MAX_SCALE
      , myVariables.PIPE_SCALE_TIME
      , null
    );
    yield return scale_cor;
    is_painting_in_progress = false;
  }

  private Vector3 setZ( Vector3 vector, float value )//TODO
  {
    return new Vector3( vector.x, vector.y, value );
  }
  #endregion
}

[Serializable]
public class ResourceMatPair
{
  [SerializeField] public QuadResourceType resource_type = QuadResourceType.NONE;
  [SerializeField] public Material         material      = null;
}
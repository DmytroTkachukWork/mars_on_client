using System;
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
  private const float SCALE_TIME = 0.15f;
  private MyTask scale_task = null;
  private QuadResourceType resource_type = QuadResourceType.NONE;
  #endregion

  #region Public Methods
  public bool setResourceAndFill( QuadResourceType resource_type, bool is_incoming )
  {
    if ( this.resource_type == resource_type )
    {
      if ( resource_type == QuadResourceType.NONE )
        return false;

      in_scale_transform.localScale = setZ( in_scale_transform.localScale, MAX_SCALE );
      out_scale_transform.localScale = setZ( out_scale_transform.localScale, MAX_SCALE );
      return false;
    }

    this.resource_type = resource_type;
    scale_task?.stop();

    foreach ( MeshRenderer renderer in resource_renderers )
      renderer.material = resources_pairs.FirstOrDefault( x => x.resource_type == resource_type ).material;

    in_scale_transform.localScale = setZ( in_scale_transform.localScale, MIN_SCALE );
    in_scale_transform.gameObject.SetActive( false );
    out_scale_transform.localScale = setZ( out_scale_transform.localScale, MIN_SCALE );
    out_scale_transform.gameObject.SetActive( false );

    if ( resource_type == QuadResourceType.NONE )
      return true;

    if ( is_incoming )
    {
      in_scale_transform.gameObject.SetActive( true );
      scale_task = tweener.tweenFloat(
          ( value ) => in_scale_transform.localScale = setZ( in_scale_transform.localScale, value )
        , MIN_SCALE
        , MAX_SCALE
        , SCALE_TIME
        , null );
    }
    else
    {
      out_scale_transform.gameObject.SetActive( true );
      scale_task = tweener.waitAndDo( () => tweener.tweenFloat(
          ( value ) => out_scale_transform.localScale = setZ( out_scale_transform.localScale, value )
        , MIN_SCALE
        , MAX_SCALE
        , SCALE_TIME
        , null )
        , SCALE_TIME );
    }

    return true;
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
using System;
using System.Linq;
using UnityEngine;

public class PipeRosourceController : MonoBehaviour
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
  private const float SCALE_TIME = 1.0f;
  private MyTask scale_task = null;
  #endregion

  #region Public Methods
  public void setResourceAndFill( QuadResourceType resource_type, bool is_incoming )
  {
    foreach ( MeshRenderer renderer in resource_renderers )
      renderer.material = resources_pairs.FirstOrDefault( x => x.resource_type == resource_type ).material;

    //Transform curent_transform = is_incoming ? in_scale_transform : out_scale_transform;
    //scale_task = TweenerStatic.tween(
    //    ( value ) => curent_transform.localScale = setZ( curent_transform.localScale, value )
    //  , MIN_SCALE
    //  , MAX_SCALE
    //  , SCALE_TIME
    //  , null );
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
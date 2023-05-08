using System;
using System.Linq;
using UnityEngine;

public class QuadContentController : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private QuadMovementController movement_controller = null;
  [SerializeField] private Transform conector_root = null;
  [SerializeField] private MeshRenderer mesh_renderer = null;
  [SerializeField] private RecoutceMatPair[] recoutce_mat_pairs = null;
  #endregion

  #region Public Fields
  public Transform conectorRoot => conector_root;
  public event Action onRotate = delegate{};
  public QuadEntity quad_entity = new QuadEntity();
  #endregion


  #region Public Methods
  public virtual void init( QuadEntity quad_entity )
  {
    this.quad_entity = quad_entity;
    quad_entity.curent_rotation = quad_entity.start_rotation;
    transform.rotation = Quaternion.Euler( 0.0f, quad_entity.start_rotation, 0.0f );
    quad_entity.curent_rotation = quad_entity.start_rotation;
    paintConected( QuadResourceType.NONE, true );

    if ( movement_controller == null )
      return;

    movement_controller.init( quad_entity.start_rotation );
    movement_controller.onRotate += updateState;
  }

  public virtual void deinit()
  {
    if ( movement_controller == null )
      return;

    movement_controller.deinit();
    movement_controller.onRotate -= updateState;
  }

  public virtual void paintConected( QuadResourceType resource_type = QuadResourceType.NONE, bool force = false )
  {
    if ( quad_entity.role_type != QuadRoleType.PLAYABLE && !force )
      return;

    mesh_renderer.material = recoutce_mat_pairs.FirstOrDefault( x => x.resource_type == resource_type ).material;
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }
  #endregion

  #region Private Methods
  private void updateState( float angle )
  {
    quad_entity.curent_rotation = angle;
    onRotate.Invoke();
  }
  #endregion
}

[Serializable]
public class RecoutceMatPair
{
  [SerializeField] public QuadResourceType resource_type = QuadResourceType.NONE;
  [SerializeField] public Material         material      = null;
}
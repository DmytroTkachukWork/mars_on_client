using System;
using UnityEngine;

public class QuadContentController : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private QuadMovementController movement_controller = null;
  [SerializeField] private Transform conector_root = null;
  [SerializeField] private MeshRenderer mesh_renderer = null;
  [SerializeField] private Material conected_mat = null;
  [SerializeField] private Material disconected_mat = null;
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

    movement_controller.init( quad_entity.start_rotation );
    movement_controller.onRotate += updateState;
    paintConected( false );
  }

  public virtual void deinit()
  {
    movement_controller.deinit();
    movement_controller.onRotate -= updateState;
  }

  public virtual void paintConected( bool state )
  {
    mesh_renderer.material = state ? conected_mat : disconected_mat;
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
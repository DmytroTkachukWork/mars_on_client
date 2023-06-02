using System;
using UnityEngine;

public class QuadContentController : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private QuadMovementController movement_controller = null;
  [SerializeField] private Transform conector_root = null;
  #endregion

  #region Private Fields
  protected ConectorController conector_controller = null;
  #endregion

  #region Public Fields
  public Transform conectorRoot => conector_root;
  public event Action onRotate = delegate{};
  public event Action onBeginRotate = delegate{};
  public QuadEntity quad_entity = new QuadEntity();
  #endregion


  #region Public Methods
  public virtual void init( QuadEntity quad_entity, ConectorController conector_controller )
  {
    this.conector_controller = conector_controller;
    this.quad_entity = quad_entity;
    quad_entity.curent_rotation = quad_entity.start_rotation;
    paintConected( QuadResourceType.NONE );

    if ( movement_controller == null )
      return;

    movement_controller.init( quad_entity.start_rotation );
    movement_controller.onBeginRotate += updateState;
    movement_controller.onRotate += () => onRotate.Invoke();
  }

  public virtual void deinit()
  {
    if ( movement_controller == null )
      return;

    movement_controller.deinit();
    movement_controller.onBeginRotate -= updateState;
    movement_controller.onRotate -= () => onRotate.Invoke();
  }

  public virtual bool paintConected( QuadResourceType resource_type = QuadResourceType.NONE, int origin_dir = 0 )
  {
    return conector_controller.paintConected( resource_type, origin_dir );
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
    Debug.Log( "Quad despawned" );
  }
  #endregion

  #region Private Methods
  private void updateState( float angle )
  {
    paintConected();
    quad_entity.curent_rotation = angle;
    onBeginRotate.Invoke();
  }
  #endregion
}
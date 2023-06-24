using UnityEngine;


public class PlanetCameraContainerController : CameraContainerController
{
  #region Serialized Fields
  [SerializeField] private Collider additional_collider = null;
  #endregion


  #region Public Methods
  public override void init()
  {
    base.init();

    setUpCamera( DragType.ROTATION );
  }

  public override void deinit()
  {
    base.deinit();

    additional_collider.enabled = false;
  }
  
  public override void setUpCamera( DragType drag_type )
  {
    base.setUpCamera( drag_type );

    additional_collider.enabled = drag_type == DragType.ROTATION;
  }
  #endregion
}
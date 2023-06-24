using System.Linq;
using UnityEngine;

public class StartFinishPoint : QuadContentController
{
  #region Public Methods
  public override void init( QuadEntity quad_entity, ConectorController conector_controller )
  {
    this.conector_controller = conector_controller;
    this.quad_entity = quad_entity;

    transform.localRotation = Quaternion.Euler( 0.0f, quad_entity.start_rotation, 0.0f );
    quad_entity.curent_rotation = quad_entity.start_rotation;
  }

  public override void deinit()
  {}
  #endregion
}

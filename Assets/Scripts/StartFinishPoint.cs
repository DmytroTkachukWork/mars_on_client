using System.Linq;
using UnityEngine;

public class StartFinishPoint : QuadContentController
{
  #region Serilized Fields
  [SerializeField] protected MeshRenderer mesh_renderer = null;
  [SerializeField] protected ResourceMatPair[] resource_mat_pairs = null;
  #endregion


  #region Public Methods
  public override void init( QuadEntity quad_entity, ConectorController conector_controller )
  {
    this.conector_controller = conector_controller;
    this.quad_entity = quad_entity;
    transform.localRotation = Quaternion.Euler( 0.0f, quad_entity.start_rotation, 0.0f );
    quad_entity.curent_rotation = quad_entity.start_rotation;
    mesh_renderer.material = resource_mat_pairs.FirstOrDefault( x => x.resource_type == quad_entity.recource_type ).material;
    if ( quad_entity.role_type == QuadRoleType.STARTER )
      paintConected( quad_entity.recource_type );
  }

  public override void deinit()
  {
    
  }
  #endregion
}

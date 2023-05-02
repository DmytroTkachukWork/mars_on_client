using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFinishPoint : QuadContentController
{
  public override void init( QuadEntity quad_entity )
  {
    this.quad_entity = quad_entity;
    transform.rotation = Quaternion.Euler( 0.0f, quad_entity.start_rotation, 0.0f );
    quad_entity.curent_rotation = quad_entity.start_rotation;
  }

  public override void deinit()
  {
    
  }

  public override void paintConected( bool state )
  {
    
  }
}

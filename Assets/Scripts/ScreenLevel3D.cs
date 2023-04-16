using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLevel3D : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private FieldManager field_manager = null;
  [SerializeField] private GameObject location_3d = null;//TODO spawn
  [SerializeField] private LevelQuadMatrix[] level_matrixes = null;//TODO spawn
  #endregion


  #region Public Methods
  public void init( int level_number )
  {
    this.gameObject.SetActive( true );
    if ( level_matrixes.Length > level_number )
      field_manager.init( level_matrixes[level_number] );
  }

  public void deinit()
  {
    field_manager.deinit();
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }
  #endregion
}

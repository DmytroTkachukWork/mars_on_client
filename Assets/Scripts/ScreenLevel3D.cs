using UnityEngine;
using UnityEditor;
using System.IO;

public class ScreenLevel3D : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private FieldManager field_manager = null;
  [SerializeField] private GameObject location_3d = null;//TODO spawn
  [SerializeField] private LevelQuadMatrix[] level_matrixes = null;
  #endregion


  #region Public Methods
  public void init( int level_number )
  {
    this.gameObject.SetActive( true );
    if ( level_number >= level_matrixes.Length )
      return;

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

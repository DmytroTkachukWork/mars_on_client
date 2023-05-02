using UnityEngine;
using UnityEditor;
using System.IO;

public class ScreenLevel3D : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private FieldManager field_manager = null;
  [SerializeField] private GameObject location_3d = null;//TODO spawn
  [SerializeField] private Object[] level_matrixes = null;//TODO spawn
  #endregion


  #region Public Methods
  public void init( int level_number )
  {
    this.gameObject.SetActive( true );
    if ( level_number >= level_matrixes.Length )
      return;

    string asset_path = AssetDatabase.GetAssetPath( level_matrixes[level_number] );
    if ( string.IsNullOrEmpty( asset_path ) )
      return;
      
    LevelQuadMatrix matrix = JsonUtility.FromJson<LevelQuadMatrix>( File.ReadAllText( asset_path ) );
    field_manager.init( matrix );
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

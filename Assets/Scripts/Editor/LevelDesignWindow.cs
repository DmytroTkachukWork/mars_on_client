using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class LevelDesignWindow : EditorWindow
{
  private LevelQuadMatrix matrix = null;
  private LevelQuadMatrix matrix_local = null;
  private int x_matrix = 0;
  private int y_matrix = 0;
  private EditModeType edit_mode_type = EditModeType.TYPE;
  private string[] edit_types = { "TYPE", "ROTATE", "ROLE", "RECOURCE" };

  [MenuItem( "Assets/LevelDesignWindow", false, 2 )]
  private static void init()
  {
    LevelDesignWindow window = CreateInstance<LevelDesignWindow>();
    window.minSize = new Vector2( 500, 500 );

    window.Show();
  }

  private void OnEnable()
  {
    edit_mode_type = EditModeType.TYPE;
    edit_types = new string[]{ "TYPE", "ROTATE", "ROLE", "RECOURCE" };
    matrix_local = (LevelQuadMatrix)ScriptableObject.CreateInstance(typeof( LevelQuadMatrix ));
  }

  private void OnGUI()
  {
    //scriptable obj field
    x_matrix = EditorGUILayout.DelayedIntField("X", x_matrix);
    y_matrix = EditorGUILayout.DelayedIntField("Y", y_matrix);

    matrix = (LevelQuadMatrix)EditorGUILayout.ObjectField( matrix, typeof( LevelQuadMatrix ), true );

    if ( GUILayout.Button( "Load", GUILayout.Width( 80 ) ) )
    {
      matrix_local.matrix_size = matrix.matrix_size;
      matrix_local.quad_entities = new QuadEntity[matrix.matrix_size.x * matrix.matrix_size.y];
      matrix.quad_entities.CopyTo( matrix_local.quad_entities, 0 );
    }

    //save button
    GUILayout.BeginHorizontal();
    if ( GUILayout.Button( "Save", GUILayout.Width( 80 ) ) )
    {
      if ( matrix == null )
      {
        AssetDatabase.CreateAsset(matrix_local, "Assets/Level_0.asset");
        AssetDatabase.SaveAssets();
        GUILayout.EndHorizontal();
        return;
      }

      matrix.SetDirty();
      matrix.matrix_size = matrix_local.matrix_size;

      matrix.quad_entities = new QuadEntity[matrix_local.quad_entities.Length];

      Debug.LogError( $"{matrix.quad_entities.Length} {matrix_local.quad_entities.Length}" );
      matrix_local.quad_entities.CopyTo( matrix.quad_entities, 0 );
      AssetDatabase.SaveAssetIfDirty( matrix_local );
      AssetDatabase.SaveAssets();
    }

    if ( GUILayout.Button( "Create ", GUILayout.Width( 80 ) ) )
    {
      matrix_local = (LevelQuadMatrix)ScriptableObject.CreateInstance(typeof( LevelQuadMatrix ));
      matrix_local.matrix_size = new Vector2Int( x_matrix, y_matrix );
      matrix_local.quad_entities = new QuadEntity[x_matrix * y_matrix];
    }

    edit_mode_type = (EditModeType)EditorGUI.Popup( new Rect( 200, 70, 90, 25 ), (int)edit_mode_type, edit_types );

    GUILayout.EndHorizontal();

    if ( matrix_local == null || matrix_local.quad_entities == null)
      return;

    GUILayout.BeginVertical();

    for( int i = 0; i < matrix_local.matrix_size.x; i++ )
    {
      GUILayout.BeginHorizontal();
      for( int j = 0; j < matrix_local.matrix_size.y; j++ )
      {
        int index = matrix_local.matrix_size.y * i + j;
        if ( matrix_local.quad_entities[index] == null )
          matrix_local.quad_entities[index] = new QuadEntity();

        int type_int = (int)matrix_local.quad_entities[index].connection_type;
        int angle = (int)(matrix_local.quad_entities[index].start_rotation / 90);
        int role = (int)matrix_local.quad_entities[index].role_type;
        int recource = (int)matrix_local.quad_entities[index].recource_type;

        string button_str = getSymbol( type_int, angle );
        button_str += matrix_local.quad_entities[index].role_type == QuadRoleType.STARTER ? "s" : "";
        button_str += matrix_local.quad_entities[index].role_type == QuadRoleType.FINISHER ? "f" : "";
        button_str += matrix_local.quad_entities[index].role_type == QuadRoleType.PLAYABLE ? "p" : "";

        button_str += matrix_local.quad_entities[index].recource_type == QuadResourceType.WATER ? "w" : "";
        button_str += matrix_local.quad_entities[index].recource_type == QuadResourceType.AIR ? "a" : "";

        if ( GUILayout.Button( button_str, GUILayout.Width( 40 ), GUILayout.Height( 40 ) ) )
        {
          if ( edit_mode_type == EditModeType.TYPE )
          {
            type_int = getCycled( type_int, 7 );
            matrix_local.quad_entities[index].connection_type = (QuadConectionType)type_int;
          }

          if ( edit_mode_type == EditModeType.ROTATE )
          {
            angle = getCycled( angle, 4 );
            matrix_local.quad_entities[index].start_rotation = 90.0f * angle;
          }

          if ( edit_mode_type == EditModeType.ROLE )
          {
            role = getCycled( role, 4 );
            matrix_local.quad_entities[index].role_type = (QuadRoleType)role;
          }

          if ( edit_mode_type == EditModeType.RECOURCE )
          {
            recource = getCycled( recource, 3 );
            matrix_local.quad_entities[index].recource_type = (QuadResourceType)recource;
          }
        }
      }
      GUILayout.EndHorizontal();
    }
    GUILayout.EndVertical();
  }

  private int getCycled( int number, int max_number )
  {
    number++;
    if ( number >= max_number )
      number = 0;

    return number;
  }

  private List<Vector2Int> getAllByType( QuadRoleType role_type )
  {
    List<Vector2Int> list = new List<Vector2Int>();
    for( int i = 0; i < matrix_local.matrix_size.x; i++ )
    {
      for( int j = 0; j < matrix_local.matrix_size.y; j++ )
      {
        int index = matrix_local.matrix_size.y * i + j;
        if ( matrix_local.quad_entities[index].role_type != role_type )
          continue;

        list.Add( new Vector2Int( i, j ) );
      }
    }
    return list;
  }

  private string getSymbol( int value, int angle )
  {
    if ( angle == 0 )
    {
      switch( value )
      {
      case 1: return "-";
      case 2: return "└";
      case 3: return "┴";
      case 4: return "╗";
      case 5: return "┼";
      case 6: return "^";
      default: return "";
      }
    }

    if ( angle == 1 )
    {
      switch( value )
      {
      case 1: return "|";
      case 2: return "┌";
      case 3: return "├";
      case 4: return "╝";
      case 5: return "┼";
      case 6: return ">";
      default: return "";
      }
    }

    if ( angle == 2 )
    {
      switch( value )
      {
      case 1: return "-";
      case 2: return "┐";
      case 3: return "┬";
      case 4: return "╚";
      case 5: return "┼";
      case 6: return "v";
      default: return "";
      }
    }

    if ( angle == 3 )
    {
      switch( value )
      {
      case 1: return "|";
      case 2: return "┘";
      case 3: return "┤";
      case 4: return "╔";
      case 5: return "┼";
      case 6: return "<";
      default: return "";
      }
    }

    return "";
  }
}

public enum EditModeType
{
  TYPE = 0,
  ROTATE = 1,
  ROLE = 2,
  RECOURCE = 3
}
#endif
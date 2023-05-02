using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelDesignWindow : EditorWindow
{
  private UnityEngine.Object matrix = null;
  private LevelQuadMatrix matrix_local = new LevelQuadMatrix();
  private int x_matrix = 0;
  private int y_matrix = 0;
  private EditModeType edit_mode_type = EditModeType.TYPE;
  private string[] edit_types = { "TYPE", "ROTATE", "STARTER", "FINISHER", "PLAYABLE", "NONPLAYABLE" };

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
    edit_types = new string[]{ "TYPE", "ROTATE", "STARTER", "FINISHER", "PLAYABLE", "NONPLAYABLE" };
    matrix_local = new LevelQuadMatrix();
  }

  private void OnGUI()
  {
    //scriptable obj field
    x_matrix = EditorGUILayout.DelayedIntField("X", x_matrix);
    y_matrix = EditorGUILayout.DelayedIntField("Y", y_matrix);

    var cached_matrix = matrix;
    matrix = (UnityEngine.Object)EditorGUILayout.ObjectField( matrix, typeof( UnityEngine.Object ), true );
    if ( matrix != cached_matrix )
    {
      string asset_path = AssetDatabase.GetAssetPath( matrix );
      if ( !string.IsNullOrEmpty( asset_path ) )
        matrix_local = JsonUtility.FromJson<LevelQuadMatrix>( File.ReadAllText( asset_path ) );
    }

    if ( GUILayout.Button( "Load", GUILayout.Width( 80 ) ) )
    {
      matrix_local = JsonUtility.FromJson<LevelQuadMatrix>( File.ReadAllText( EditorUtility.OpenFilePanel( "Load level", Application.dataPath, "json" ) ) );
    }

    //save button
    GUILayout.BeginHorizontal();
    if ( GUILayout.Button( "Save", GUILayout.Width( 80 ) ) )
    {
      AssetDatabase.SaveAssets();
      matrix_local.starter_positions = getAllByType( QuadRoleType.STARTER ).ToArray();
      Debug.LogError( getAllByType( QuadRoleType.STARTER ).Count );
      matrix_local.finisher_positions = getAllByType( QuadRoleType.FINISHER ).ToArray();
      Debug.LogError( getAllByType( QuadRoleType.FINISHER ).Count );
      File.WriteAllText( EditorUtility.SaveFilePanel( "Save level", Application.dataPath, "Level_0", "json"), JsonUtility.ToJson( matrix_local, true ) );
    }

    if ( GUILayout.Button( "Create ", GUILayout.Width( 80 ) ) )
    {
      matrix_local = new LevelQuadMatrix();
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

        string button_str = getSymbol( type_int, angle );
        button_str += matrix_local.quad_entities[index].role_type == QuadRoleType.STARTER ? "s" : "";
        button_str += matrix_local.quad_entities[index].role_type == QuadRoleType.FINISHER ? "f" : "";
        button_str += matrix_local.quad_entities[index].role_type == QuadRoleType.PLAYABLE ? "p" : "";

        if ( GUILayout.Button( button_str, GUILayout.Width( 40 ), GUILayout.Height( 40 ) ) )
        {
          if ( edit_mode_type == EditModeType.TYPE )
          {
            type_int++;
            if ( type_int >= 7 )
              type_int = 0;

            matrix_local.quad_entities[index].connection_type = (QuadConectionType)type_int;
          }

          if ( edit_mode_type == EditModeType.ROTATE )
          {
            angle++;
            if ( angle >= 4 )
              angle = 0;

            matrix_local.quad_entities[index].start_rotation = 90.0f * angle;
          }

          if ( edit_mode_type == EditModeType.STARTER )
            matrix_local.quad_entities[index].role_type = QuadRoleType.STARTER;

          if ( edit_mode_type == EditModeType.FINISHER )
            matrix_local.quad_entities[index].role_type = QuadRoleType.FINISHER;

          if ( edit_mode_type == EditModeType.PLAYABLE )
            matrix_local.quad_entities[index].role_type = QuadRoleType.PLAYABLE;

          if ( edit_mode_type == EditModeType.NONPLAYABLE )
            matrix_local.quad_entities[index].role_type = QuadRoleType.NONE;
        }
      }
      GUILayout.EndHorizontal();
    }
    GUILayout.EndVertical();
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
  STARTER = 2,
  FINISHER = 3,
  PLAYABLE = 4,
  NONPLAYABLE = 5
}
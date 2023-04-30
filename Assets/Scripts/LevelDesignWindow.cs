using System;
using UnityEditor;
using UnityEngine;

public class LevelDesignWindow : EditorWindow
{
  private LevelQuadMatrix matrix = null;
  private LevelQuadMatrix matrix_local = new LevelQuadMatrix();
  private bool is_rotating = false;
  [MenuItem( "Assets/LevelDesignWindow", false, 2 )]
  private static void init()
  {
    LevelDesignWindow window = CreateInstance<LevelDesignWindow>();
    window.minSize = new Vector2( 305, 210 );

    window.Show();
  }

  private void OnGUI()
  {
    //scriptable obj field
    LevelQuadMatrix matrix_cached = matrix;
    matrix = (LevelQuadMatrix)EditorGUILayout.ObjectField( matrix, typeof( LevelQuadMatrix ), true );
    if ( matrix == null )
      return;

    if ( matrix_cached != matrix )
      matrix_local = matrix;

    //save button
    GUILayout.BeginHorizontal();
    if ( GUILayout.Button( "Save", GUILayout.Width( 80 ) ) )
      matrix = matrix_local;

    if ( GUILayout.Button( "Rotate " + is_rotating, GUILayout.Width( 80 ) ) )
      is_rotating = !is_rotating;

    GUILayout.EndHorizontal();

    //matrix
    int length = matrix_local.matrix_size.x * matrix_local.matrix_size.y;
    if ( matrix_local.quad_conection_types == null || matrix_local.quad_conection_types.Length != length )
      matrix_local.quad_conection_types = new int[length];

    if ( matrix_local.quad_conection_rotates == null || matrix_local.quad_conection_rotates.Length != length )
      matrix_local.quad_conection_rotates = new int[length];

    GUILayout.BeginVertical();
    for( int i = matrix_local.matrix_size.y - 1; i >= 0; i-- )
    {
      GUILayout.BeginHorizontal();
      for( int j = 0; j < matrix_local.matrix_size.x; j++ )
      {
        int type_int = matrix_local.quad_conection_types[matrix_local.matrix_size.y * j + i];
        int angle = matrix_local.quad_conection_rotates [matrix_local.matrix_size.y * j + i];
        if ( GUILayout.Button( getSymbol( type_int, angle ), GUILayout.Width( 20 ), GUILayout.Height( 20 ) ) )
        {
          if ( is_rotating )
          {
            angle++;
            if ( angle >= 4 )
              angle = 0;
          }
          else
          {
            type_int++;
            if ( type_int >= 6 )
              type_int = 0;
          }

          matrix_local.quad_conection_types  [matrix_local.matrix_size.y * j + i] = type_int;
          matrix_local.quad_conection_rotates[matrix_local.matrix_size.y * j + i] = angle;
        }
      }
      GUILayout.EndHorizontal();
    }
    GUILayout.EndVertical();
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
      default: return "";
      }
    }

    return "";
  }
}

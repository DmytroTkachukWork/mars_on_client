using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class QuadEntity
{
  #region Serilized Fields
  [SerializeField] public QuadConectionType connection_type = QuadConectionType.NONE;
  [SerializeField] public QuadRoleType role_type = QuadRoleType.NONE;
  [SerializeField] public QuadResourceType recource_type = QuadResourceType.NONE;
  [SerializeField] public float start_rotation = 0.0f;
  #endregion

  #region Public Fields
  public float curent_rotation = 0.0f;
  public int matrix_x = 0;
  public int matrix_y = 0;
  #endregion


  #region Public Methods
  public bool canBeAccessedFrom( int dir )
  {
    if ( role_type == QuadRoleType.NONE )
      return false;

    int input_dir = MatrixHelper.inverse4( dir );
    int[] matrix = MatrixHelper.getMatrix( connection_type );
    matrix = MatrixHelper.rotateQuadByAngle( matrix, curent_rotation );
    return matrix[input_dir] != 0;
  }

  public int getOriginDir( int dir )
  {
    int count = (int)((curent_rotation / 90)%4);
    dir = MatrixHelper.inverse4( dir );
    return (((4 - count) % 4) + dir ) % 4;
  }

  public List<int> getNextConections( int dir )
  {
    int[] conection_matrix = MatrixHelper.rotateQuadByAngle( MatrixHelper.getMatrix( connection_type ), curent_rotation );
    List<int> next_dirs = new List<int>();

    for ( int i = 0; i < 4; i++ )
    {
      if ( i == dir || conection_matrix[i] == 0 )
        continue;

      if ( dir > 4 )
      {
        next_dirs.Add( i );
        continue;
      }

      if ( conection_matrix[i] != conection_matrix[dir] && role_type != QuadRoleType.STARTER )//0
        continue;

      next_dirs.Add( i );
    }
    return next_dirs;
  }

  public int getFirstConection()
  {
    int[] conection_matrix = MatrixHelper.rotateQuadByAngle( MatrixHelper.getMatrix( connection_type ), curent_rotation );
    for( int i = 0; i < 4; i++ )
    {
      if ( conection_matrix[i] != 0 )
        return i;
    }
    return 0;
  }
  #endregion
}

public enum QuadRoleType
{
  NONE = 0,
  PLAYABLE = 1,
  STARTER = 2,
  FINISHER = 3
}

public enum QuadConectionType
{
  NONE = 0,        // no conections
  LONG = 1,        // conects 2 oposit sides
  CORNER = 2,      // conects 2 nearby sides
  LONG_CORNER = 3, // conects 2 oposit sides and 1 nearby side
  TWO_CORNERS = 4, // conects 2 nearby sides twice with no conection in the middle
  CRIST = 5,        // conects all sides
  ONE = 6
}

public enum QuadResourceType
{
  NONE = 0,
  WATER = 1,
  AIR = 2
}
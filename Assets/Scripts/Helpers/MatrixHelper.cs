using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MatrixHelper
{
  #region Public Methods
  public static int inverse4( int value )
  {
    if ( value >= 2 )
      return value - 2;
    else
      return value + 2;
  }

  public static int[] getMatrix( QuadConectionType type )
  {
    //int[] matrix_none = new int[4]{ 0, 0, 0, 0 };
    //int[] matrix_long = new int[4]{ 0, 1, 0, 1 };
    //int[] matrix_corn = new int[4]{ 1, 1, 0, 0 };
    //int[] matrix_lncr = new int[4]{ 1, 1, 0, 1 };
    //int[] matrix_twcr = new int[4]{ 1, 1, 2, 2 };
    //int[] matrix_crst = new int[4]{ 1, 1, 1, 1 };
    //int[] matrix_one  = new int[4]{ 1, 0, 0, 0 };
    switch( type )
    {
    case QuadConectionType.LONG:        return new int[4]{ 0, 1, 0, 1 };
    case QuadConectionType.CORNER:      return new int[4]{ 1, 1, 0, 0 };
    case QuadConectionType.LONG_CORNER: return new int[4]{ 1, 1, 0, 1 };
    case QuadConectionType.TWO_CORNERS: return new int[4]{ 1, 1, 2, 2 };
    case QuadConectionType.CRIST:       return new int[4]{ 1, 1, 1, 1 };
    case QuadConectionType.ONE:         return new int[4]{ 1, 0, 0, 0 };
    case QuadConectionType.NONE:
    default:                            return new int[4]{ 0, 0, 0, 0 };
    }
  }

  public static int[] rotateQuadByAngle( int[] matrix, float angle )
  {
    int count = (int)(angle / 90);
    if ( count < 0 )
      count += 4;

    for(int i = 0; i < count; i++)
      rotateQuad( matrix );

    return matrix;
  }

  public static int[] rotateQuad( int[] matrix )
  {
    int matrix0 = matrix[3];
    matrix[3]   = matrix[2];
    matrix[2]   = matrix[1];
    matrix[1]   = matrix[0];
    matrix[0]   = matrix0;
    return matrix;
  }
  #endregion
}

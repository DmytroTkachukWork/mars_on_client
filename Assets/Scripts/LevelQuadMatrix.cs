using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu( fileName = "LevelQuadMatrix", menuName = "ScriptableObjects/LevelQuadMatrix", order = 1 )]
public class LevelQuadMatrix : ScriptableObject
{
  [SerializeField] public int[] quad_conection_types = null;
  [SerializeField] public int[] quad_conection_rotates = null;
  [SerializeField] public Vector2Int matrix_size = Vector2Int.zero;
  [SerializeField] public Vector2Int input_point = Vector2Int.zero;
  [SerializeField] public int input_point_dir = 0;
  [SerializeField] public Vector2Int output_point = Vector2Int.zero;
  [SerializeField] public int output_point_dir = 0;

  public void setUpMatrix( int[] matrix )
  {
    quad_conection_types = matrix;
  }
}

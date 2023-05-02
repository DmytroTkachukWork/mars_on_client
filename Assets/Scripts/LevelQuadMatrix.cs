using System;
using UnityEngine;


[Serializable]
public class LevelQuadMatrix
{
  [SerializeField] public QuadEntity[] quad_entities = null;
  [SerializeField] public Vector2Int matrix_size = Vector2Int.zero;
  [SerializeField] public Vector2Int[] starter_positions = null;
  [SerializeField] public Vector2Int[] finisher_positions = null;
}
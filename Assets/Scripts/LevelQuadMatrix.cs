using System;
using UnityEngine;


[Serializable]
public class LevelQuadMatrix : ScriptableObject
{
  [SerializeField] public QuadEntity[] quad_entities = null;
  [SerializeField] public Vector2Int matrix_size = Vector2Int.zero;
}
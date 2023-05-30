using System;
using UnityEngine;


[Serializable]
public class LevelQuadMatrix : ScriptableObject
{
  [SerializeField] public QuadEntity[] quad_entities = null;
  [SerializeField] public Vector2Int matrix_size = Vector2Int.zero;
  [SerializeField] public int max_steps_to_lose  = 20;
}
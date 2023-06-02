using System;
using UnityEngine;


[Serializable]
public class LevelQuadMatrix : ScriptableObject
{
  [SerializeField] public QuadEntity[] quad_entities = null;
  [SerializeField] public Vector2Int matrix_size = Vector2Int.zero;
  [SerializeField] public int max_steps_to_lose = 20;
  [SerializeField] public int sector_id = 0;
  [SerializeField] public int level_id = 0;
}
using System;
using UnityEngine;


[Serializable] [CreateAssetMenu( fileName = "LevelQuadMatrix", menuName = "ScriptableObjects/LevelQuadMatrix", order = 6 )]
public class LevelQuadMatrix : ScriptableObject
{
  [SerializeField] public QuadEntity[] quad_entities = null;
  [SerializeField] public Vector2Int matrix_size = Vector2Int.zero;
  [SerializeField] public int max_steps_to_lose = 20;
  [SerializeField] public int sector_id = 0;
  [SerializeField] public int level_id = 0;

  public LevelQuadMatrix getCopy()
  {
    LevelQuadMatrix copy = new LevelQuadMatrix();
    copy.quad_entities = quad_entities;
    copy.matrix_size = matrix_size;
    copy.max_steps_to_lose = max_steps_to_lose;
    copy.sector_id = sector_id;
    copy.level_id = level_id;
    return copy;
  }
}
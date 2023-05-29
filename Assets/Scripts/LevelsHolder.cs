using UnityEngine;

public class LevelsHolder : MonoBehaviourService<LevelsHolder>
{
  [SerializeField] private LevelQuadMatrix[] level_matrixes = null;

  public LevelQuadMatrix getLevelById( int idx )
  {
    if ( idx >= level_matrixes.Length )
      return null;

    return level_matrixes[idx];
  }
}

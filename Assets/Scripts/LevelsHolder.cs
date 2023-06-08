using UnityEngine;

public class LevelsHolder : MonoBehaviourService<LevelsHolder>
{
  [SerializeField] private PlanetInfo planet_info = null;

  public LevelQuadMatrix getLevel( int sector_id, int level_id )
  {
    if ( sector_id >= planet_info.sectors_info.Length )
      return null;

    if ( level_id >= planet_info.sectors_info[sector_id].levels_info.Length )
      return null;

    return planet_info.sectors_info[sector_id].levels_info[level_id].level_matrix.getCopy();
  }
}

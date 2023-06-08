using System;
using UnityEngine;


[Serializable] [CreateAssetMenu( fileName = "PlanetInfo", menuName = "ScriptableObjects/PlanetInfo", order = 6 )]
public class PlanetInfo : ScriptableObject
{
  [SerializeField] public SectorInfo[] sectors_info = null;
}

[Serializable]
public class SectorInfo
{
  [SerializeField] public LevelInfo[] levels_info = null;
}

[Serializable]
public class LevelInfo
{
  [SerializeField] public LevelQuadMatrix level_matrix = null;
  [SerializeField] public int stars_count = 0;
}
using System;


[Serializable]
public class PlayerData
{
  public string player_name = null;
  public ushort stars_count = 0;
  public int curent_sector_num = 0;
  public int curent_level_num = 0;
  public ProgressData progress_data = null;

  public PlayerData( string player_name, ushort stars_count, int curent_sector_num, int curent_level_num, PlanetInfo planet_info )
  {
    this.player_name = player_name;
    this.stars_count = stars_count;
    this.curent_sector_num = curent_sector_num;
    this.curent_level_num = curent_level_num;

    progress_data = new ProgressData();
    progress_data.sectors_data = new SectorData[planet_info.sectors_info.Length];

    for( int i = 0; i < progress_data.sectors_data.Length; i++ )
    {
      progress_data.sectors_data[i] = new SectorData();
      progress_data.sectors_data[i].levels_data = new LevelData[planet_info.sectors_info[i].levels_info.Length];
      for( int j = 0; j < progress_data.sectors_data[i].levels_data.Length; j++ )
      {
        progress_data.sectors_data[i].levels_data[j] = new LevelData();
      }
    }
  }
}

[Serializable]
public class ProgressData
{
  public SectorData[] sectors_data = null;
}

[Serializable]
public class SectorData
{
  public LevelData[] levels_data = null;
}

[Serializable]
public class LevelData
{
  public ushort stars_count = 0;
  public bool is_card_received = false;
}
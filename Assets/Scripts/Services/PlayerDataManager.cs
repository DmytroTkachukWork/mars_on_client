using UnityEngine;

public class PlayerDataManager : MonoBehaviourService<PlayerDataManager>
{
  [SerializeField] private PlanetInfo planet_info = null;
  private PlayerData curent_player_data = null;

  public bool hasAccessToSector( int sector_num )
  {
    return sector_num <= curent_player_data.curent_sector_num;
  }

  public bool hasAccessToLevel( int sector_num, int level_num )
  {
    if ( sector_num < curent_player_data.curent_sector_num )
      return true;

    return sector_num == curent_player_data.curent_sector_num &&
      level_num <= curent_player_data.curent_level_num;
  }

  public bool isLevelComplited( int sector_num, int level_num )
  {
    if ( sector_num < curent_player_data.curent_sector_num )
      return true;

    return sector_num == curent_player_data.curent_sector_num &&
      level_num < curent_player_data.curent_level_num;
  }

  public bool isCurentLevel( int sector_num, int level_num )
  {
    return sector_num == curent_player_data.curent_sector_num &&
      level_num == curent_player_data.curent_level_num;
  }

  public void handleLevelWin( int sector_num, int level_num, ushort stars_count )
  {
    LevelData level_data = curent_player_data.progress_data.sectors_data[sector_num].levels_data[level_num];

    if ( level_data.stars_count < stars_count )
      level_data.stars_count = stars_count;

    level_data.is_card_received = true;


    if ( sector_num != curent_player_data.curent_sector_num && level_num != curent_player_data.curent_level_num )
    {
      saveProgress();
      return;
    }

    level_num++;

    if ( level_num == planet_info.sectors_info[curent_player_data.curent_sector_num].levels_info.Length
        && curent_player_data.curent_sector_num + 1 < planet_info.sectors_info.Length )
    {
      sector_num++;
      level_num = 0;
    }

    curent_player_data.curent_sector_num = sector_num;
    curent_player_data.curent_level_num = level_num;

    saveProgress();
  }

  public void saveProgress()
  {
    PlayerDataSaver.savePlayerData( curent_player_data );
  }

  public void loadProgress()
  {
    curent_player_data = PlayerDataSaver.loadPlayerData();

    if ( curent_player_data == null )//to fix WebGL bug
      resetProgress();

    if ( curent_player_data.progress_data == null )//to fix WebGL bug
      resetProgress();

    if ( curent_player_data.progress_data.sectors_data == null )//to fix WebGL bug
      resetProgress();

    if ( curent_player_data.progress_data.sectors_data.Length == 0 )//to fix WebGL bug
      resetProgress();
  }

  public void resetProgress()
  {
    curent_player_data = new PlayerData( null, 0, 0, 0, planet_info );
    saveProgress();
  }

  public void setMaxProgress()
  {
    curent_player_data = new PlayerData( null, 0, 0, 0, planet_info );
    curent_player_data.curent_sector_num = planet_info.sectors_info.Length - 1;
    curent_player_data.curent_level_num = planet_info.sectors_info[curent_player_data.curent_sector_num].levels_info.Length - 1;
    saveProgress();
  }

  public ushort getMaxStarsCount()
  {
    ushort stars_count = 0;

    foreach( SectorInfo sector in planet_info.sectors_info )
    {
      foreach( LevelInfo level in sector.levels_info )
        stars_count += 3;
    }

    return stars_count;
  }

  public ushort getStarsCount( int sector_num, int level_num )
  {
    if ( sector_num >= curent_player_data.progress_data.sectors_data.Length )
      return 0;

    if ( level_num >= curent_player_data.progress_data.sectors_data[sector_num].levels_data.Length )
      return 0;

    return curent_player_data.progress_data.sectors_data[sector_num].levels_data[level_num].stars_count;
  }

  public ushort getCurentStarsCount()
  {
    ushort stars_count = 0;

    foreach( SectorData sector in curent_player_data.progress_data.sectors_data )
    {
      foreach( LevelData level in sector.levels_data )
        stars_count += level.stars_count;
    }

    return stars_count;
  }

  public ushort getMaxCardsCount()
  {
    ushort stars_count = 0;

    foreach( SectorInfo sector in planet_info.sectors_info )
    {
      stars_count += (ushort)sector.levels_info.Length;
    }

    return stars_count;
  }

  public ushort getCurentCardsCount()
  {
    ushort cards_count = 0;

    foreach( SectorData sector in curent_player_data.progress_data.sectors_data )
    {
      foreach( LevelData level in sector.levels_data )
        cards_count += (ushort)(level.is_card_received ? 1 : 0);
    }

    return cards_count;
  }

  public int getCurentProgressPercent()
  {
    ushort max_progress = getMaxCardsCount();
    ushort curent_progress = 0;

    for ( int i = 0; i <= curent_player_data.curent_sector_num; i++ )
    {
      for ( int j = 0; j < planet_info.sectors_info[i].levels_info.Length; j++ )
      {
        if ( curent_player_data.curent_sector_num == i && curent_player_data.curent_level_num == j )
          break;

        curent_progress++;
      }
    }

    return Mathf.FloorToInt( ( (float)curent_progress / (float)max_progress ) * 100 );
  }

  public bool canReceiveCard( int sector, int level )
  {
    return !curent_player_data.progress_data.sectors_data[sector].levels_data[level].is_card_received;
  }

  public int getProgressBySector( int sector_id )
  {
    if ( sector_id < 0 || sector_id >= planet_info.sectors_info.Length )
      return 0;

    if ( curent_player_data.curent_sector_num < sector_id )
      return 0;

    if ( curent_player_data.curent_sector_num > sector_id )
      return 100;

    return Mathf.FloorToInt( (float)curent_player_data.curent_level_num / (float)planet_info.sectors_info[sector_id].levels_info.Length * 100.0f );
  }
}


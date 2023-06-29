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
      level_num < curent_player_data.curent_level_num;
  }

  public bool isCurentLevel( int sector_num, int level_num )
  {
    return sector_num == curent_player_data.curent_sector_num &&
      level_num == curent_player_data.curent_level_num;
  }

  public void handleLevelWin( int sector_num, int level_num )
  {
    if ( sector_num != curent_player_data.curent_sector_num && level_num != curent_player_data.curent_level_num)
      return;

    if ( sector_num >= planet_info.sectors_info.Length )
      return;

    level_num++;

    if ( level_num >= planet_info.sectors_info[curent_player_data.curent_sector_num].levels_info.Length
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
    curent_player_data.some_data = new SomeData();
    curent_player_data.some_data.stars_count = new int[10];
    curent_player_data.some_data.stars_count[0] = 199;
    PlayerDataSaver.savePlayerData( curent_player_data );
  }

  public void loadProgress()
  {
    if ( curent_player_data == null )
      curent_player_data = PlayerDataSaver.loadPlayerData();

    if ( curent_player_data == null )
      curent_player_data = new PlayerData();
    else
      Debug.LogError( curent_player_data.some_data?.stars_count[0] );
  }

  public void resetProgress()
  {
    curent_player_data = new PlayerData();
    saveProgress();
  }
}


using System;


[Serializable]
public class PlayerData
{
  public string player_name = null;
  public int stars_count = 0;
  public int curent_sector_num = 0;
  public int curent_level_num = 0;
  public SomeData some_data = null;
}

[Serializable]
public class SomeData
{
  public int[] stars_count = null;
}
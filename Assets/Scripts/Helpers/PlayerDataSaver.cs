using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class PlayerDataSaver
{
  private static string path = Application.persistentDataPath + "/localSave.marson";

  public static void savePlayerData( PlayerData player_data )
  {
    BinaryFormatter formater = new BinaryFormatter();
    FileStream file_stream = new FileStream( path, FileMode.Create );
    formater.Serialize( file_stream, player_data );
    file_stream.Close();
  }

  public static PlayerData loadPlayerData()
  {
    if ( !File.Exists( path ) )
      return null;

    BinaryFormatter formater = new BinaryFormatter();
    PlayerData player_data = null;
    
    FileStream file_stream = new FileStream( path, FileMode.Open );
    player_data = (PlayerData)formater.Deserialize( file_stream );
    file_stream.Close();
    return player_data;
  }
}

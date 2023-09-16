using UnityEngine;

public class MonoBehaviourBase : MonoBehaviour
{
  protected static LevelManager levelManager => Service<LevelManager>.get();
  protected static LevelsHolder levelsHolder => Service<LevelsHolder>.get();
  protected static PlayerDataManager playerDataManager => Service<PlayerDataManager>.get();
  protected static MyVariables myVariables => Service<MyVariables>.get();
  protected static Tweener tweener => Service<Tweener>.get();
  protected static SpawnManager spawnManager => Service<SpawnManager>.get();
  protected static GlobalCameraController cameraController => Service<GlobalCameraController>.get();
  protected static BlurScreenshot blurScreenshot => Service<BlurScreenshot>.get();
  protected static CardManager cardManager => Service<CardManager>.get();
}

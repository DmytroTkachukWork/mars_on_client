using UnityEngine;

public class MonoBehaviourBase : MonoBehaviour
{
  protected static SpawnManager spawnManager => Service<SpawnManager>.get();
  protected static GlobalCameraController cameraController => Service<GlobalCameraController>.get();
}

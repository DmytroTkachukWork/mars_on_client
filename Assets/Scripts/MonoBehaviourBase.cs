using UnityEngine;

public class MonoBehaviourBase : MonoBehaviour
{
  protected static Tweener tweener => Service<Tweener>.get();
  protected static SpawnManager spawnManager => Service<SpawnManager>.get();
  protected static GlobalCameraController cameraController => Service<GlobalCameraController>.get();
}

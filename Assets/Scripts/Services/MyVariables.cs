using UnityEngine;

public class MyVariables : MonoBehaviourService<MyVariables>
{
  [SerializeField] public float CAMERA_MOVE_TIME = 1.0f;
  [SerializeField] public float CAMERA_TELEPORT_TIME = 0.01f;
  [SerializeField] public float CAMERA_ROTATE_TIME = 0.6f;
  [SerializeField] public float QUAD_DISTANCE = 1.15f;
  [SerializeField] public float WIN_CHECK_DELAY = 0.2f;
  [SerializeField] public float LEVEL_WIN_FADE_TIME = 5.5f;
  [SerializeField] public float LEVEL_LOSE_FADE_TIME = 5.5f;
  [SerializeField] public float QUAD_ROTATION_TIME = 0.9f;
  [SerializeField] public float PIPE_SCALE_TIME = 0.3f;
  [SerializeField] public float RESOURCE_ENTITY_SCALE_TIME = 0.1f;
}

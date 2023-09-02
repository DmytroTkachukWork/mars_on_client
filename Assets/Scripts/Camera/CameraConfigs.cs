using System;
using UnityEngine;


[Serializable] [CreateAssetMenu( fileName = "CameraConfigs", menuName = "ScriptableObjects/CameraConfigs", order = 7 )]
public class CameraConfigs : ScriptableObject
{
  [SerializeField] private float camera_fov = 60.0f;
  [SerializeField] private float fog_start_distance = 100.0f;
  [SerializeField] private float fog_end_distance = 100.0f;
  [SerializeField] private Color fog_color = Color.white;


  public float cameraFov => camera_fov;
  public float fogStartDistance => fog_start_distance;
  public float fogEndDistance => fog_end_distance;
  public Color fogColor => fog_color;
}

public enum LocationType
{
  NONE = 0,
  PLANET = 1,
  SECTOR = 2,
  LEVEL = 3
}
using UnityEngine;

public class FieldManager : MonoBehaviour
{
  #region Serialized Fields
  [SerializeField] private Vector2Int field_size = Vector2Int.zero;
  #endregion

  #region Private Fields
  private float QUAD_DISTANCE = 1.15f;
  private SpawnManager spawn_manager = null;
  private Vector3 cached_position = Vector3.zero;
  private QuadContentController[,] quad_matrix = null;
  #endregion


  #region Public Mathods
  void Start()
  {
    init();
  }
  public void init()
  {
    //spawn quads
    spawn_manager = FindObjectOfType<SpawnManager>();
    quad_matrix = new QuadContentController[field_size.x, field_size.y];
    for ( int i = 0; i < field_size.x; i++ )
    {
      for ( int j = 0; j < field_size.y; j++ )
      {
        cached_position.x = transform.position.x + QUAD_DISTANCE * i;
        cached_position.z = transform.position.z + QUAD_DISTANCE * j;
        
        quad_matrix[i, j] = spawn_manager.spawnQuad( cached_position );
        spawn_manager.spawnConector( quad_matrix[i, j].conectorRoot, (QuadConectionType)Random.Range( 0, 6 ) );
      }
    }
  }
  #endregion
}

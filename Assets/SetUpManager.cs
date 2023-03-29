using UnityEngine;


public class SetUpManager : MonoBehaviour
{
  #region Public Fields
  [SerializeField] private LevelQuadMatrix level_quad_matrix = null;
  #endregion

  #region Private Fields
  private QuadSetUpController[,] quad_matrix = null;
  private Vector3 cached_position = Vector3.zero;
  private float QUAD_DISTANCE = 1.15f;
  #endregion


  #region Public Methods
  public void Start()
  {
    SpawnManager spawn_manager = FindObjectOfType<SpawnManager>();
    quad_matrix = new QuadSetUpController[level_quad_matrix.matrix_size.x, level_quad_matrix.matrix_size.y];
    for ( int i = 0; i < level_quad_matrix.matrix_size.x; i++ )
    {
      for ( int j = 0; j < level_quad_matrix.matrix_size.y; j++ )
      {
        cached_position.x = transform.position.x + QUAD_DISTANCE * i;
        cached_position.z = transform.position.z + QUAD_DISTANCE * j;
        
        quad_matrix[i, j] = spawn_manager.spawnQuadSetUp( cached_position );
        quad_matrix[i, j].init( level_quad_matrix.quad_conection_types.Length > 0 
        ? level_quad_matrix.quad_conection_types[level_quad_matrix.matrix_size.x * i + j]
        : UnityEngine.Random.Range( 0, 6 ) );
      }
    }
  }

  public void OnGUI()
  {
    if ( GUI.Button( new Rect( 300, 600, 80, 80 ), "save" ) )
      save();
  }

  public void save()
  {
    int[] quad_conection_types = new int[level_quad_matrix.matrix_size.x * level_quad_matrix.matrix_size.y];
    for ( int i = 0; i < level_quad_matrix.matrix_size.x; i++ )
    {
      for ( int j = 0; j < level_quad_matrix.matrix_size.y; j++ )
      {
        Debug.LogError( level_quad_matrix.matrix_size.y * i + j );
        quad_conection_types[level_quad_matrix.matrix_size.y * i + j] = (int)quad_matrix[i, j].getType();
      }
    }
    level_quad_matrix.setUpMatrix( quad_conection_types );
  }
  #endregion
}

using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
  #region Serialized Fields
  [SerializeField] private LevelQuadMatrix level_quad_matrix = null;
  #endregion

  #region Private Fields
  private float QUAD_DISTANCE = 1.15f;
  private SpawnManager spawn_manager = null;
  private Vector3 cached_position = Vector3.zero;
  private QuadContentController[,] quad_matrix = null;
  private Task wait_and_checker = Task.CompletedTask;
  private Awaiter check_awaiter = new Awaiter();
  private float check_delay = 0.5f;
  #endregion


  #region Public Mathods
  public void init()
  {
    //spawn quads
    spawn_manager = FindObjectOfType<SpawnManager>();
    quad_matrix = new QuadContentController[level_quad_matrix.matrix_size.x, level_quad_matrix.matrix_size.y];
    for ( int i = 0; i < level_quad_matrix.matrix_size.x; i++ )
    {
      for ( int j = 0; j < level_quad_matrix.matrix_size.y; j++ )
      {
        cached_position.x = transform.position.x + QUAD_DISTANCE * i;
        cached_position.z = transform.position.z + QUAD_DISTANCE * j;
        
        quad_matrix[i, j] = spawn_manager.spawnQuad( cached_position );
        QuadConectionType type = (QuadConectionType)level_quad_matrix.quad_conection_types[level_quad_matrix.matrix_size.y * i + j];
        quad_matrix[i, j].init( 0.0f, type );
        spawn_manager.spawnConector( quad_matrix[i, j].conectorRoot, type );
        quad_matrix[i, j].onRotate += waitAndCkeck;
      }
    }
  }
  #endregion

  #region Private Methods
  void Start()
  {
    init();
  }

  private void waitAndCkeck()
  {
    check_awaiter.waitAndDo( checkForWin, check_delay );
  }

  private void checkForWin()
  {
    List<QuadContentController> checked_quads = new List<QuadContentController>();

    foreach( QuadContentController quad in quad_matrix )
      quad.paintConected( false );

    quad_matrix[0, 0].paintConected( true );
    bool win = false;
    moveNext( level_quad_matrix.input_point_dir, level_quad_matrix.input_point.x, level_quad_matrix.input_point.y );

    void moveNext( int inner_dir, int x, int y )
    {
      Debug.Log( $"{x}, {y}" );
      checked_quads.Add( quad_matrix[x, y] );
      inner_dir = inverse4( inner_dir );
      int[] cur_conections = quad_matrix[x, y].conectionMatrix;
      quad_matrix[x, y].paintConected( true );

      for ( int i = 0; i < 4; i++ )
      {
        if ( inner_dir == i )
          continue;

        if ( cur_conections[i] != cur_conections[inner_dir] )
          continue;

        Vector2Int next_quad_coord = getNextPosV(i);
        next_quad_coord.x += x; 
        next_quad_coord.y += y; 

        if ( level_quad_matrix.output_point.x == x+1 && level_quad_matrix.output_point.y == y+1 && i == level_quad_matrix.output_point_dir )//win
        {
          win = true;
          return;
        }

        if ( next_quad_coord.x < 0 || next_quad_coord.x >= level_quad_matrix.matrix_size.x
          || next_quad_coord.y < 0 || next_quad_coord.y >= level_quad_matrix.matrix_size.y )
          continue;

        if ( checked_quads.Contains( quad_matrix[next_quad_coord.x, next_quad_coord.y] )
            && !quad_matrix[next_quad_coord.x, next_quad_coord.y].isTwoCornersType )
          continue;

        
        if ( quad_matrix[next_quad_coord.x, next_quad_coord.y].conectionMatrix[inverse4(i)] != 0 )
        {
          quad_matrix[next_quad_coord.x, next_quad_coord.y].paintConected( true );
          moveNext( i, next_quad_coord.x, next_quad_coord.y );
        }

        if ( win )
          return;
      }
    }
  }

  private Vector2Int getNextPosV( int dir )
  {
    Vector2Int[] dirs = new Vector2Int[4]{ 
        new Vector2Int(0, 1)
      , new Vector2Int(1, 0)
      , new Vector2Int(0, -1)
      , new Vector2Int(-1, 0)
      };
    return dirs[dir];
  }

  private int inverse4( int value )
  {
    if ( value >= 2 )
      return value - 2;
    else
      return value + 2;
  }
  #endregion
}

public class Awaiter
{
  private float ceched_check_delay = 1.0f;
  private Task awaiting_task = Task.CompletedTask;
  public void waitAndDo( Action func, float time, bool restart = true )
  {
    ceched_check_delay = time;
    if ( awaiting_task.IsCompleted )
      awaiting_task = wait();

    async Task wait()
    {
      while( ceched_check_delay > 0.0f )
      {
        await Task.Yield();
        ceched_check_delay -= Time.deltaTime;
      }
      func.Invoke();
    }
  }
}
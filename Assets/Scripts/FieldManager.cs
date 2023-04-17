using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviourBase
{
  #region Serialized Fields
  #endregion

  #region Private Fields
  private LevelQuadMatrix level_quad_matrix = null;
  private float QUAD_DISTANCE = 1.15f;
  private Vector3 cached_position = Vector3.zero;
  private QuadContentController[,] quad_matrix = null;
  private List<ConectorController> conectors = new List<ConectorController>();
  private Awaiter check_awaiter = new Awaiter();
  private float check_delay = 0.5f;
  #endregion


  #region Public Mathods
  public void init( LevelQuadMatrix level_quad_matrix )
  {
    this.level_quad_matrix = level_quad_matrix;
    quad_matrix = new QuadContentController[level_quad_matrix.matrix_size.x, level_quad_matrix.matrix_size.y];
    for ( int i = 0; i < level_quad_matrix.matrix_size.x; i++ )
    {
      for ( int j = 0; j < level_quad_matrix.matrix_size.y; j++ )
      {
        cached_position.x = transform.position.x + QUAD_DISTANCE * i;
        cached_position.z = transform.position.z + QUAD_DISTANCE * j;
        
        quad_matrix[i, j] = spawnManager.spawnQuad( cached_position, transform );
        QuadConectionType type = (QuadConectionType)level_quad_matrix.quad_conection_types[level_quad_matrix.matrix_size.y * i + j];
        quad_matrix[i, j].init( 0.0f, type );
        spawnManager.spawnConector( quad_matrix[i, j].conectorRoot ).init( type );
        quad_matrix[i, j].onRotate += waitAndCkeck;
      }
    }

    cached_position.x = transform.position.x + QUAD_DISTANCE * level_quad_matrix.input_point.x;
    cached_position.z = transform.position.z + QUAD_DISTANCE * level_quad_matrix.input_point.y;
    cached_position.x += QUAD_DISTANCE * getNextPosV( inverse4( level_quad_matrix.input_point_dir ) ).x;
    cached_position.z += QUAD_DISTANCE * getNextPosV( inverse4( level_quad_matrix.input_point_dir ) ).y;
    float rotation = 90.0f * level_quad_matrix.input_point_dir;

    spawnManager.spawnStartFinishPoint( cached_position, Quaternion.Euler( 0.0f, rotation, 0.0f ), transform );

    cached_position.x = transform.position.x + QUAD_DISTANCE * level_quad_matrix.output_point.x;
    cached_position.z = transform.position.x + QUAD_DISTANCE * level_quad_matrix.output_point.y;
    cached_position.x -= QUAD_DISTANCE * getNextPosV( inverse4( level_quad_matrix.output_point_dir ) ).x;
    cached_position.z -= QUAD_DISTANCE * getNextPosV( inverse4( level_quad_matrix.output_point_dir ) ).y;
    rotation = 90.0f * inverse4( level_quad_matrix.output_point_dir );

    spawnManager.spawnStartFinishPoint( cached_position, Quaternion.Euler( 0.0f, rotation, 0.0f ), transform );
  }

  public void deinit()
  {
    check_awaiter.stop();

    spawnManager.despawnAllConectors();
    spawnManager.despawnAllQuads();
    spawnManager.despawnAllStartFinishPoints();
  }
  #endregion

  #region Private Methods
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

    Debug.LogError( "Our victory is " + win );
    if ( !win )
      return;

    foreach( QuadContentController quad in quad_matrix )
    {
      quad.deinit();
    }

    spawnManager.spawnScreenWinUI().init( () =>
      { 
        spawnManager.despawnScreenLevelUI();
        spawnManager.despawnScreenLevel3D();
      } );

    void moveNext( int inner_dir, int x, int y )
    {
      checked_quads.Add( quad_matrix[x, y] );
      inner_dir = inverse4( inner_dir );
      int[] cur_conections = quad_matrix[x, y].conectionMatrix;
      quad_matrix[x, y].paintConected( true );

      for ( int i = 0; i < 4; i++ )
      {
        if ( win )
          return;

        if ( inner_dir == i )
          continue;

        if ( cur_conections[i] != cur_conections[inner_dir] )
          continue;

        Vector2Int next_quad_coord = getNextPosV(i);
        next_quad_coord.x += x; 
        next_quad_coord.y += y; 
        Debug.LogError( "Next coord is " + next_quad_coord );

        if ( level_quad_matrix.output_point.x == x && level_quad_matrix.output_point.y == y && i == level_quad_matrix.output_point_dir )//win
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
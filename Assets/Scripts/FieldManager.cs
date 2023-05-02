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
  private MyTask awaiter = null;
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
        int index = level_quad_matrix.matrix_size.y * i + j;

        if ( level_quad_matrix.quad_entities[index].role_type == QuadRoleType.NONE )
          continue;

        cached_position.x = transform.position.x + QUAD_DISTANCE * j;
        cached_position.z = transform.position.z - QUAD_DISTANCE * i;

        QuadConectionType type = level_quad_matrix.quad_entities[index].connection_type;
        float angle = level_quad_matrix.quad_entities[index].start_rotation;

        if ( level_quad_matrix.quad_entities[index].role_type != QuadRoleType.PLAYABLE )
        {
          quad_matrix[i, j] = (QuadContentController)spawnManager.spawnStartFinishPoint( cached_position, Quaternion.Euler( 0.0f, angle, 0.0f ), transform );
          quad_matrix[i,j].init( level_quad_matrix.quad_entities[index] );
          continue;
        }
        
        quad_matrix[i, j] = spawnManager.spawnQuad( cached_position, transform );
        spawnManager.spawnConector( quad_matrix[i, j].conectorRoot ).init( type );
        quad_matrix[i, j].init( level_quad_matrix.quad_entities[index] );
        quad_matrix[i, j].onRotate += waitAndCkeck;
      }
    }
  }

  public void deinit()
  {
    awaiter?.stop();

    spawnManager.despawnAllConectors();
    spawnManager.despawnAllQuads();
    spawnManager.despawnAllStartFinishPoints();
  }
  #endregion

  #region Private Methods
  private void waitAndCkeck()
  {
    awaiter?.stop();
    awaiter = TweenerStatic.waitAndDo( checkForWin, check_delay );
  }


  private void checkForWin()
  {
    HashSet<QuadContentController> checked_quads = new HashSet<QuadContentController>();

    foreach( QuadContentController quad in quad_matrix )
      quad?.paintConected( false );

    bool win = false;

    Vector2Int starter_pos = level_quad_matrix.starter_positions[0];
    QuadEntity starter = level_quad_matrix.quad_entities[level_quad_matrix.matrix_size.y * starter_pos.x + starter_pos.y];

    int dir = starter.getFirstConection();

    moveNext( dir, starter_pos.x, starter_pos.y );

    if ( !win )
      return;

    foreach( QuadContentController quad in quad_matrix )
      quad?.deinit();

    spawnManager.spawnScreenWinUI().init( () =>
      { 
        spawnManager.despawnScreenLevelUI();
        spawnManager.despawnScreenLevel3D();
      } );

    void moveNext( int inner_dir, int x, int y )
    {
      if ( x < 0 || y < 0 ||  x >= level_quad_matrix.matrix_size.x || y >= level_quad_matrix.matrix_size.y ) // is inside of array
        return;

      QuadContentController curent_quad = quad_matrix[x, y];
      QuadEntity curent_quad_entity = level_quad_matrix.quad_entities[level_quad_matrix.matrix_size.y * x + y];

      if ( curent_quad_entity.role_type == QuadRoleType.NONE )
        return;

      if ( !checked_quads.Add( curent_quad ) && curent_quad_entity.connection_type != QuadConectionType.TWO_CORNERS ) // wasnt pessed previuslly
        return;

      if ( !curent_quad_entity.canBeAccessedFrom( inner_dir ) && curent_quad_entity.role_type != QuadRoleType.STARTER ) // is it reacheable
        return;

      inner_dir = inverse4( inner_dir );

      List<int> next_dirs = curent_quad_entity.getNextConections( inner_dir );

      quad_matrix[x, y].paintConected( true );

      if ( curent_quad_entity.role_type ==QuadRoleType.FINISHER ) // check for win
      {
        win = true;
        return;
      }

      foreach( int dir in next_dirs )
      {
        Vector2Int vector_dir = getNextPosV( dir );
        moveNext( dir, vector_dir.x + x, vector_dir.y + y );
      }
    }
  }

  private Vector2Int getNextPosV( int dir )
  {
    Vector2Int[] dirs = new Vector2Int[4]{ 
        new Vector2Int(-1, 0)
      , new Vector2Int(0, 1)
      , new Vector2Int(1, 0)
      , new Vector2Int(0, -1)
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
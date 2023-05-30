using UnityEngine;

public class FieldManager : MonoBehaviourBase
{
  #region Private Fields
  private LevelQuadMatrix level_quad_matrix = null;
  private Vector3 cached_position = Vector3.zero;
  private QuadContentController[,] quad_matrix = null;
  private MyTask awaiter = null;
  private int cached_steps_to_lose = 0;
  #endregion


  #region Public Mathods
  public void init( LevelQuadMatrix level_quad_matrix )
  {
    deinit();

    this.level_quad_matrix = level_quad_matrix;
    cached_steps_to_lose = level_quad_matrix.max_steps_to_lose;
    quad_matrix = new QuadContentController[level_quad_matrix.matrix_size.x, level_quad_matrix.matrix_size.y];

    for ( int i = 0; i < level_quad_matrix.matrix_size.x; i++ )
    {
      for ( int j = 0; j < level_quad_matrix.matrix_size.y; j++ )
      {
        int index = level_quad_matrix.matrix_size.y * i + j;

        if ( level_quad_matrix.quad_entities[index].role_type == QuadRoleType.NONE )
          continue;

        cached_position.x = transform.localPosition.x + myVariables.QUAD_DISTANCE * j;
        cached_position.z = transform.localPosition.z - myVariables.QUAD_DISTANCE * i;

        QuadRoleType role_type = level_quad_matrix.quad_entities[index].role_type;
        QuadConectionType type = level_quad_matrix.quad_entities[index].connection_type;
        
        quad_matrix[i, j] = spawnManager.spawnQuad( cached_position, transform, role_type );
        quad_matrix[i, j].transform.localPosition = cached_position;
        quad_matrix[i, j].transform.localRotation = Quaternion.identity;

        ConectorController conector = spawnManager.spawnConector( quad_matrix[i, j].conectorRoot );
        conector.init( type );

        level_quad_matrix.quad_entities[index].matrix_x = i;
        level_quad_matrix.quad_entities[index].matrix_y = j;

        quad_matrix[i, j].init( level_quad_matrix.quad_entities[index], conector );
        quad_matrix[i, j].onRotate += waitAndCkeck;
        quad_matrix[i, j].onBeginRotate += stopWaitingForCheck;
      }
    }

    checkForWin();
    (spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL ) as ScreenLevelUI ).updateStepsCount( cached_steps_to_lose );
  }

  public void deinit()
  {
    awaiter?.stop();

    despawnMatrix();
    unsubscrube();
  }

  public void despawnMatrix()
  {
    spawnManager.despawnAllConectors();
    spawnManager.despawnAllQuads();
  }

  public void unsubscrube()
  {
    if ( quad_matrix == null )
      return;

    foreach ( QuadContentController quad in quad_matrix )
    {
      if ( quad == null )
        continue;

      quad.onRotate -= waitAndCkeck;
      quad.onBeginRotate -= stopWaitingForCheck;
    }
  }
  #endregion

  #region Private Methods
  private void stopWaitingForCheck()
  {
    awaiter?.stop();

    cached_steps_to_lose--;

    if ( cached_steps_to_lose == 0 )
    {
      unsubscrube();
      spawnManager.despawnScreenUI( ScreenUIId.LEVEL );
      ( spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL_LOSE ) as ScreenLoseUI ).init();
      return;
    }

    (spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL ) as ScreenLevelUI ).updateStepsCount( cached_steps_to_lose );

    PathFinder.stopCheck();
    PathFinder.fastRepaint( quad_matrix, level_quad_matrix );
  }

  private void waitAndCkeck()
  {
    awaiter?.stop();
    awaiter = tweener.waitAndDo( checkForWin, myVariables.WIN_CHECK_DELAY );
  }

  private void checkForWin()
  {
    PathFinder.checkForWin( quad_matrix, level_quad_matrix, () =>
    ( spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL_WIN ) as ScreenWinUI ).init( null ) );
  }
  #endregion
}
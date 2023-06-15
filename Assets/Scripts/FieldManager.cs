using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviourBase
{
  #region Serialized Fields
  [SerializeField] private WinLoseManager win_lose_manager = null;
  #endregion

  #region Private Fields
  private LevelQuadMatrix level_quad_matrix = null;
  private Vector3 cached_position = Vector3.zero;
  private QuadContentController[,] quad_matrix = null;
  private int cached_steps_to_lose = 0;
  private event Action onBeginRotate = delegate{};
  private PipeTree pipe_tree = null;
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
        quad_matrix[i, j].onRotate += continuePainting;
        quad_matrix[i, j].onBeginRotate += handleQuadRotation;
      }
    }

    pipe_tree = PathFinder.getPipeTree( quad_matrix, level_quad_matrix );
    continuePainting();

    if ( level_quad_matrix.lose_type == LevelLoseType.NONE )
      return;

    win_lose_manager.init( level_quad_matrix );
    win_lose_manager.onLose += handleLose;

    if ( level_quad_matrix.lose_type == LevelLoseType.ROTATIONS_COUNT )
      onBeginRotate += win_lose_manager.onBeginRotate;

    ( spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL ) as ScreenLevelUI ).updateStepsCount( cached_steps_to_lose );
  }

  public void deinit()
  {
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
    win_lose_manager.deinit();
    onBeginRotate -= win_lose_manager.onBeginRotate;
    win_lose_manager.onLose -= handleLose;

    if ( quad_matrix == null )
      return;

    foreach ( QuadContentController quad in quad_matrix )
    {
      if ( quad == null )
        continue;

      quad.onRotate -= continuePainting;
      quad.onBeginRotate -= handleQuadRotation;
    }
  }
  #endregion

  #region Private Methods
  private void handleQuadRotation( QuadEntity quad_entity )
  {
    onBeginRotate.Invoke();
    pipe_tree.getPipe( quad_entity )?.controller?.paintConected();
    PathFinder.fastRepaint( quad_matrix, level_quad_matrix );
  }

  private void levelCore()
  {
    pipe_tree.starter_pipe.controller.paintConected( pipe_tree.starter_pipe.pipe_resource, pipe_tree.starter_pipe.inner_dir, pipe_tree.starter_pipe.children, paintMe );

    void paintMe( List<Pipe> next_pipes_to_paint )
    {
      if ( next_pipes_to_paint == null )
        return;

      foreach( Pipe pipe in next_pipes_to_paint )
      {
        pipe.controller.paintConected( pipe.pipe_resource, pipe.inner_dir, pipe.children, paintMe );
        if ( pipe.quad.role_type == QuadRoleType.FINISHER )
          handleWin();
      }
    }
  }

  private void handleLose()
  {
    unsubscrube();

    foreach ( QuadContentController quad in quad_matrix )
      quad?.paintConected();

    spawnManager.despawnScreenUI( ScreenUIId.LEVEL );
    ( spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL_LOSE ) as ScreenLoseUI ).init();
  }

  private void handleWin()
  {
    unsubscrube();
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL );
    ( spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL_WIN ) as ScreenWinUI ).init( null );
    playerDataManager.handleLevelWin( level_quad_matrix.sector_id, level_quad_matrix.level_id );
  }


  private void continuePainting()
  {
    pipe_tree = PathFinder.getPipeTree( quad_matrix, level_quad_matrix );
    levelCore();
  }
  #endregion
}
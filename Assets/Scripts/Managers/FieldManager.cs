using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
  private event Action<bool> onBeginRotate = delegate{};
  private PipeTree pipe_tree = null;
  private Stack<QuadContentController> undo_stack = null;
  private bool has_win = false;
  private List<IEnumerator> runing_cors = new List<IEnumerator>();
  private bool is_rotating = false;
  #endregion


  #region Public Mathods
  public void init( LevelQuadMatrix level_quad_matrix = null )
  {
    deinit();

    levelManager.startLevel( this );

    undo_stack = new Stack<QuadContentController>();

    if ( level_quad_matrix != null )
      this.level_quad_matrix = level_quad_matrix;

    cached_steps_to_lose = this.level_quad_matrix.max_steps_to_lose;
    quad_matrix = new QuadContentController[this.level_quad_matrix.matrix_size.x, this.level_quad_matrix.matrix_size.y];

    for ( int i = 0; i < this.level_quad_matrix.matrix_size.x; i++ )
    {
      for ( int j = 0; j < this.level_quad_matrix.matrix_size.y; j++ )
      {
        int index = this.level_quad_matrix.matrix_size.y * i + j;

        if ( this.level_quad_matrix.quad_entities[index].role_type == QuadRoleType.NONE )
          continue;

        cached_position.x = transform.localPosition.x + myVariables.QUAD_DISTANCE * j;
        cached_position.z = transform.localPosition.z - myVariables.QUAD_DISTANCE * i;

        QuadRoleType role_type = this.level_quad_matrix.quad_entities[index].role_type;
        QuadConectionType type = this.level_quad_matrix.quad_entities[index].connection_type;
        
        quad_matrix[i, j] = spawnManager.spawnQuad( cached_position, transform, role_type );
        quad_matrix[i, j].transform.localPosition = cached_position;
        quad_matrix[i, j].transform.localRotation = Quaternion.identity;

        ConectorController conector = spawnManager.spawnConector( quad_matrix[i, j].conectorRoot );
        conector.init( type );

        this.level_quad_matrix.quad_entities[index].matrix_x = i;
        this.level_quad_matrix.quad_entities[index].matrix_y = j;

        quad_matrix[i, j].init( this.level_quad_matrix.quad_entities[index], conector );
        quad_matrix[i, j].onRotate += continuePainting;
        quad_matrix[i, j].onBeginRotate += handleQuadRotation;
      }
    }

    pipe_tree = PathFinder.getPipeTreeNew( quad_matrix, this.level_quad_matrix );
    continuePainting();

    spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL );

    if ( this.level_quad_matrix.lose_type == LevelLoseType.NONE )
      return;

    win_lose_manager.init( this.level_quad_matrix );
    win_lose_manager.onLose += handleLose;

    if ( this.level_quad_matrix.lose_type == LevelLoseType.ROTATIONS_COUNT )
      onBeginRotate += win_lose_manager.onBeginRotate;

    ( spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL ) as ScreenLevelUI ).updateStepsCount( cached_steps_to_lose );
  }

  public void deinit()
  {
    has_win = false;
    despawnMatrix();
    unsubscrube();
    spawnManager.despawnAllWaterfalls();
  }

  public void despawnMatrix()
  {
    spawnManager.despawnAllConectors();
    spawnManager.despawnAllQuads();
    undo_stack?.Clear();
  }

  public void unsubscrube()
  {
    win_lose_manager.deinit();
    onBeginRotate -= win_lose_manager.onBeginRotate;
    win_lose_manager.onLose -= handleLose;
    despawnWaterfalls();

    if ( quad_matrix == null )
      return;

    foreach ( QuadContentController quad in quad_matrix )
    {
      if ( quad == null )
        continue;

      quad.onRotate -= continuePainting;
      quad.onBeginRotate -= handleQuadRotation;
      quad.deinit();
    }
  }

  public void undoAction()
  {
    if ( undo_stack.Count == 0 )
      return;

    QuadContentController undo_quad = undo_stack.Pop();
    undo_quad.rotateBack();
  }

  public void pauseLevel()
  {
    win_lose_manager.pauseLevel();

    foreach ( QuadContentController quad in quad_matrix )
      quad?.pauseResumeClick( true );
  }

  public void resumeLevel()
  {
    win_lose_manager.resumeLevel();

    foreach ( QuadContentController quad in quad_matrix )
      quad?.pauseResumeClick( false );
  }
  #endregion

  #region Private Methods
  private void handleQuadRotation( QuadEntity quad_entity, bool is_reverce )
  {
    if ( quad_entity == null )
      return;

    is_rotating = true;
    onBeginRotate.Invoke( is_reverce );
    Pipe rotated_pipe = pipe_tree.getPipe( quad_entity );
    rotated_pipe.controller?.paintConected();
    PathFinder.fastRepaint( quad_matrix, level_quad_matrix );

    if ( is_reverce )
      return;

    QuadContentController quad = quad_matrix[ quad_entity.matrix_x, quad_entity.matrix_y ];
    
    if ( quad == null )
      return;

    undo_stack.Push( quad );
  }

  private void levelCore()
  {
    HashSet<Pipe> cached_pipes = new HashSet<Pipe>(){pipe_tree.starter_pipe};
    bool has_checked = false;
    stopCors();

    pipe_tree.starter_pipe.controller.paintConected( pipe_tree.starter_pipe.pipe_resource, pipe_tree.starter_pipe.inner_dirs.FirstOrDefault(), pipe_tree.starter_pipe.children, paintMe );

    void paintMe( HashSet<Pipe> next_pipes_to_paint )
    {
      if ( next_pipes_to_paint == null )
      {
        onWaterFall();
        return;
      }

      if ( next_pipes_to_paint.Count == 0 )
      {
        if ( has_checked )
        {
          onWaterFall();
          return;
        }

        has_checked = true;
        stopCors();
        impl();
        onWaterFall();
        return;
      }

      if ( win_lose_manager.isLevelFinished )
      {
        stopCors();
        return;
      }

      cached_pipes = next_pipes_to_paint;

      foreach( Pipe pipe in next_pipes_to_paint )
      {
        //pipe.controller.despawnWaterFalls();
        IEnumerator cor = tweener.waitNoneAndDo( () => pipe.controller.paintConected( pipe.pipe_resource, pipe.inner_dirs.FirstOrDefault(), pipe.children, paintMe ) );
        runing_cors.Add( cor );
        cor.start();
      }
    }

    void impl()
    {
      if ( PathFinder.isAllPipesConected( pipe_tree ) && !win_lose_manager.isLevelFinished )
      {
        handleWin();
      }
    }

    void stopCors()
    {
      foreach( IEnumerator cor in runing_cors )
      cor?.stop();

      runing_cors.Clear();
    }

    void onWaterFall()
    {
      if ( cached_pipes == null )
        return;

      foreach ( Pipe pipe in cached_pipes )
      {
        int inner_dir = pipe.inner_dirs.FirstOrDefault();

        foreach( int dir in pipe.quad.getLocalNextConections( inner_dir ) )
        {
          float angle = 90.0f * dir;
          Vector3 rotation = new Vector3( 0.0f, angle, 0.0f );
          pipe.controller.spawnWaterfall( Quaternion.Euler( rotation ) );
        }
      }
    }
  }

  private void handleLose()
  {
    unsubscrube();

    spawnManager.despawnScreenUI( ScreenUIId.LEVEL );
    ( spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL_LOSE ) as ScreenLoseUI ).init();
  }

  private void handleWin()
  {
    if ( has_win )
      return;

    has_win = true;
    unsubscrube();
    spawnManager.despawnScreenUI( ScreenUIId.LEVEL );


    ushort stars_count = getStarsCount( win_lose_manager.getCurentStepsCount(), level_quad_matrix.max_steps_to_lose );
    bool receive_card = playerDataManager.canReceiveCard( level_quad_matrix.sector_id, level_quad_matrix.level_id);

    playerDataManager.handleLevelWin( level_quad_matrix.sector_id, level_quad_matrix.level_id, stars_count );

    ( spawnManager.getOrSpawnScreenUI( ScreenUIId.LEVEL_WIN ) as ScreenWinUI ).init( level_quad_matrix, stars_count, receive_card );

  }

  private void despawnWaterfalls()
  {
    if ( quad_matrix == null )
      return;

    foreach ( QuadContentController quad in quad_matrix )
    {
      quad?.despawnWaterFalls();
    }
  }

  private void continuePainting()
  {
    is_rotating = false;
    pipe_tree = PathFinder.getPipeTreeNew( quad_matrix, level_quad_matrix );
    levelCore();
  }

  private ushort getStarsCount( int maden_steps, int max_steps )
  {
    float persent = (float)maden_steps / (float)max_steps;

    if ( persent >= 0.5f )
      return 3;

    if ( persent >= 0.25f )
      return 2;

    return 1;
  }
  #endregion
}
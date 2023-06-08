using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PathFinder
{
  private static MyTask check_task = new MyTask();
  private static MyTask culc_task = new MyTask();
  private static float QUAD_CALC_TIME = 0.3f;

  public static void stopCheck()
  {
    check_task.stop();
    culc_task.stop();
  }

  public static async Task fastRepaint( QuadContentController[,] quad_matrix, LevelQuadMatrix level_quad_matrix )
  {
    HashSet<QuadContentController> checked_quads = new HashSet<QuadContentController>();

    IEnumerable<QuadEntity> starters = level_quad_matrix.quad_entities.Where( x => x.role_type == QuadRoleType.STARTER );
    IEnumerable<QuadEntity> finishers = level_quad_matrix.quad_entities.Where( x => x.role_type == QuadRoleType.FINISHER );

    foreach( QuadEntity starter in starters )
    {
      QuadResourceType starter_resource = starter.recource_type;
      List<int> dirs = starter.getNextConections( 5 );

      foreach( int dir in dirs )
        moveNext( dir, starter.matrix_x, starter.matrix_y, starter_resource );
    }

    foreach( QuadContentController quad in quad_matrix )
    {
      if ( quad == null )
        continue;

      if ( !checked_quads.Contains( quad ) && quad.quad_entity.role_type != QuadRoleType.STARTER )
        quad.paintConected();
    }
    return;

    void moveNext( int inner_dir, int x, int y, QuadResourceType resource_type )
    {
      if ( x < 0 || y < 0 ||  x >= level_quad_matrix.matrix_size.x || y >= level_quad_matrix.matrix_size.y ) // is inside of array
        return;

      QuadContentController curent_quad = quad_matrix[x, y];
      QuadEntity curent_quad_entity = level_quad_matrix.quad_entities[level_quad_matrix.matrix_size.y * x + y];
      Debug.LogError( $"pointer is on {x} {y}" );

      if ( curent_quad_entity.role_type == QuadRoleType.NONE )
        return;

      if ( curent_quad_entity.role_type == QuadRoleType.STARTER )
      {
        Vector2Int vector_dir = getNextPosV( inner_dir );
        curent_quad.paintConected( resource_type, inner_dir );
        moveNext( inner_dir, vector_dir.x + x, vector_dir.y + y, resource_type );
        return;
      }

      if ( !curent_quad_entity.canBeAccessedFrom( inner_dir ) && curent_quad_entity.role_type != QuadRoleType.STARTER ) // is it reacheable
        return;

      if ( !checked_quads.Add( curent_quad ) && curent_quad_entity.connection_type != QuadConectionType.TWO_CORNERS && curent_quad_entity.role_type == QuadRoleType.PLAYABLE ) // wasnt pessed previuslly
      {
        if ( curent_quad_entity.recource_type != resource_type )
          return;
      }

      int origin_dir = curent_quad_entity.getOriginDir( inner_dir );

      if ( curent_quad_entity.role_type == QuadRoleType.FINISHER && resource_type == curent_quad_entity.recource_type ) // check for win
        return;

      curent_quad_entity.recource_type = resource_type;

      inner_dir = inverse4( inner_dir );

      List<int> next_dirs = curent_quad_entity.getNextConections( inner_dir );

      foreach( int dir in next_dirs )
      {
        Vector2Int vector_dir = getNextPosV( dir );
        moveNext( dir, vector_dir.x + x, vector_dir.y + y, resource_type );
      }
    }
  }

  public static async Task checkForWin( QuadContentController[,] quad_matrix, LevelQuadMatrix level_quad_matrix, Action callback )
  {
    if ( !check_task.curent_task.IsCompleted )
      stopCheck();

    await Task.Yield();
    await Task.Yield();

    check_task.init();
    check_task.curent_task = impl();
    
    async Task impl()
    {
      HashSet<QuadContentController> checked_quads = new HashSet<QuadContentController>();

      bool win = false;
      List<bool> wins = new List<bool>();

      IEnumerable<QuadEntity> starters = level_quad_matrix.quad_entities.Where( x => x.role_type == QuadRoleType.STARTER );
      IEnumerable<QuadEntity> finishers = level_quad_matrix.quad_entities.Where( x => x.role_type == QuadRoleType.FINISHER );
      Debug.Log( $"starters count is {starters.Count()}" );

      foreach( QuadEntity starter in starters )
      {
        QuadResourceType starter_resource = starter.recource_type;
        Debug.Log( $"starter_resource is {starter_resource}" );
        List<int> dirs = starter.getNextConections( 5 );
        Debug.Log( $"dirs count is {dirs.Count}" );
        foreach( int dir in dirs )
        {
          if ( check_task.cencel_token )
            return;

          win = false;
          await moveNext( dir, starter.matrix_x, starter.matrix_y, starter_resource );
          Debug.Log( $"Does we win? {win}" );
          wins.Add( win );
        }
      }

      foreach( QuadContentController quad in quad_matrix )
      {
        if ( quad == null )
          continue;

        if ( !checked_quads.Contains( quad ) && quad.quad_entity.role_type != QuadRoleType.STARTER )
          quad.paintConected();
      }

      if ( wins.Any( x => x == false ) )
        return;

      foreach( QuadContentController quad in quad_matrix )
        quad?.deinit();

      callback?.Invoke();

      async Task moveNext( int inner_dir, int x, int y, QuadResourceType resource_type )
      {
        if ( x < 0 || y < 0 ||  x >= level_quad_matrix.matrix_size.x || y >= level_quad_matrix.matrix_size.y ) // is inside of array
          return;

        QuadContentController curent_quad = quad_matrix[x, y];
        QuadEntity curent_quad_entity = level_quad_matrix.quad_entities[level_quad_matrix.matrix_size.y * x + y];
        Debug.LogError( $"pointer is on {x} {y}" );

        if ( curent_quad_entity.role_type == QuadRoleType.NONE )
          return;

        if ( curent_quad_entity.role_type == QuadRoleType.STARTER )
        {
          Vector2Int vector_dir = getNextPosV( inner_dir );
          curent_quad.paintConected( resource_type, inner_dir );
          await moveNext( inner_dir, vector_dir.x + x, vector_dir.y + y, resource_type );
          return;
        }

        if ( !curent_quad_entity.canBeAccessedFrom( inner_dir ) && curent_quad_entity.role_type != QuadRoleType.STARTER ) // is it reacheable
          return;

        if ( !checked_quads.Add( curent_quad ) && curent_quad_entity.connection_type != QuadConectionType.TWO_CORNERS && curent_quad_entity.role_type == QuadRoleType.PLAYABLE ) // wasnt pessed previuslly
        {
          if ( curent_quad_entity.recource_type != resource_type )
            return;
        }

        int origin_dir = curent_quad_entity.getOriginDir( inner_dir );
        if ( curent_quad.paintConected( resource_type, origin_dir ) )
        {
          culc_task = Service<Tweener>.get().waitAndDo( null, QUAD_CALC_TIME );
          await culc_task.curent_task;
        }

        if ( curent_quad_entity.role_type == QuadRoleType.FINISHER && resource_type == curent_quad_entity.recource_type ) // check for win
        {
          win = true;
          return;
        }

        curent_quad_entity.recource_type = resource_type;

        inner_dir = inverse4( inner_dir );

        List<int> next_dirs = curent_quad_entity.getNextConections( inner_dir );

        foreach( int dir in next_dirs )
        {
          Vector2Int vector_dir = getNextPosV( dir );
          
          if ( check_task.cencel_token )
            return;

          await moveNext( dir, vector_dir.x + x, vector_dir.y + y, resource_type );
        }
      }
    }
  }

  private static Vector2Int getNextPosV( int dir )
  {
    Vector2Int[] dirs = new Vector2Int[4]{ 
        new Vector2Int(-1, 0)
      , new Vector2Int(0, 1)
      , new Vector2Int(1, 0)
      , new Vector2Int(0, -1)
      };
    return dirs[dir];
  }

  private static int inverse4( int value )
  {
    if ( value >= 2 )
      return value - 2;
    else
      return value + 2;
  }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{

  public static void fastRepaint( QuadContentController[,] quad_matrix, LevelQuadMatrix level_quad_matrix )
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

    void moveNext( int inner_dir, int x, int y, QuadResourceType resource_type )
    {
      if ( x < 0 || y < 0 ||  x >= level_quad_matrix.matrix_size.x || y >= level_quad_matrix.matrix_size.y ) // is inside of array
        return;

      QuadContentController curent_quad = quad_matrix[x, y];
      QuadEntity curent_quad_entity = level_quad_matrix.quad_entities[level_quad_matrix.matrix_size.y * x + y];

      if ( curent_quad_entity.role_type == QuadRoleType.NONE )
        return;

      if ( curent_quad_entity.role_type == QuadRoleType.STARTER )
      {
        Vector2Int vector_dir = getNextPosV( inner_dir );
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

  public static PipeTree getPipeTree( QuadContentController[,] quad_matrix, LevelQuadMatrix level_quad_matrix )
  {
    PipeTree pipe_tree = new PipeTree();
    IEnumerable<QuadEntity> starters = level_quad_matrix.quad_entities.Where( x => x.role_type == QuadRoleType.STARTER );
    IEnumerable<QuadEntity> finishers = level_quad_matrix.quad_entities.Where( x => x.role_type == QuadRoleType.FINISHER );

    foreach( QuadEntity starter in starters )
    {
      pipe_tree.starter_pipe = pipe_tree.getPipe( starter );
      QuadResourceType starter_resource = starter.recource_type;
      List<int> dirs = starter.getNextConections( 5 );

      foreach( int dir in dirs )
        moveNext( dir, starter.matrix_x, starter.matrix_y, starter_resource );
    }
    return pipe_tree;

    void moveNext( int inner_dir, int x, int y, QuadResourceType resource_type )
    {
      QuadEntity curent_quad_entity = level_quad_matrix.quad_entities[level_quad_matrix.matrix_size.y * x + y];

      curent_quad_entity.recource_type = resource_type;
      pipe_tree.getPipe( curent_quad_entity ).inner_dir = curent_quad_entity.getOriginDir( inner_dir );
      pipe_tree.getPipe( curent_quad_entity ).pipe_resource = resource_type;
      pipe_tree.getPipe( curent_quad_entity ).controller = quad_matrix[x, y];

      inner_dir = inverse4( inner_dir );
      List<int> next_dirs = curent_quad_entity.getNextConections( inner_dir );

      foreach( int dir in next_dirs )
      {
        Vector2Int vector_dir = getNextPosV( dir );

        int next_idx = (level_quad_matrix.matrix_size.y * (vector_dir.x + x)) + vector_dir.y + y;
        if ( next_idx >= level_quad_matrix.quad_entities.Length || next_idx < 0 )
          continue;

        QuadEntity next_entity = level_quad_matrix.quad_entities[next_idx];
        if ( !next_entity.canBeAccessedFrom( dir ) )
          continue;

        if ( next_entity.recource_type != QuadResourceType.NONE && next_entity.recource_type != curent_quad_entity.recource_type )
          continue;

        pipe_tree.getPipe( curent_quad_entity ).addChildren( pipe_tree.getPipe( next_entity ) );
        moveNext( dir, vector_dir.x + x, vector_dir.y + y, resource_type );
      }
    }
  }
}

public class PipeTree
{
  public Pipe starter_pipe = null;
  public List<Pipe> all_pipes = new List<Pipe>();

  public Pipe getPipe( QuadEntity quad )
  {
    Pipe pipe = all_pipes.FirstOrDefault( x => x.quad == quad );

    if ( pipe == null )
    {
      pipe = new Pipe( quad );
      all_pipes.Add( pipe );
    }

    return pipe;
  }

  public void clear()
  {
    starter_pipe = null;
    all_pipes.Clear();
  }
}

public class Pipe
{
  public List<Pipe> ancestors = new List<Pipe>();
  public List<Pipe> children = new List<Pipe>();
  public QuadEntity quad = null;
  public QuadContentController controller = null;
  public bool is_pained = false;
  public QuadResourceType pipe_resource = QuadResourceType.NONE;
  public int inner_dir = 0;
  
  public Pipe( QuadEntity quad )
  {
    this.quad = quad;
  }

  public void addChildren( Pipe child )
  {
    children.Add( child );
    child.ancestors.Add( this );
  }
}
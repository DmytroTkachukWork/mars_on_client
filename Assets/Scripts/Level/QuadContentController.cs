using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuadContentController : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private QuadMovementController movement_controller = null;
  [SerializeField] private Transform conector_root = null;
  #endregion

  #region Private Fields
  protected ConectorController conector_controller = null;
  protected List<WaterFallController> spawnedWaterfalls = new List<WaterFallController>();
  protected Dictionary<int, WaterFallController> dir_waterfall = new Dictionary<int, WaterFallController>();
  #endregion

  #region Public Fields
  public Transform conectorRoot => conector_root;
  public event Action onRotate = delegate{};
  public event Action<QuadEntity, bool> onBeginRotate = delegate{};
  public QuadEntity quad_entity = null;
  #endregion


  #region Public Methods
  public virtual void init( QuadEntity quad_entity, ConectorController conector_controller )
  {
    this.conector_controller = conector_controller;
    this.quad_entity = quad_entity;
    quad_entity.curent_rotation = quad_entity.start_rotation;
    quad_entity.recource_type = QuadResourceType.NONE;
    paintConected();

    if ( movement_controller == null )
      return;

    movement_controller.init( quad_entity.start_rotation );
    movement_controller.onBeginRotate += updateState;
    movement_controller.onRotate += onRotateHandle;
  }

  public virtual void deinit()
  {
    if ( movement_controller == null )
      return;

    movement_controller.deinit();
    movement_controller.onBeginRotate -= updateState;
    movement_controller.onRotate -= onRotateHandle;
  }

  public void pauseResumeClick( bool is_pause )
  {
    if ( is_pause )
      movement_controller?.pauseClick();
    else
      movement_controller?.resumeClick();
  }

  public void rotateBack()
  {
    movement_controller.rotateOverTime( true );
  }

  public virtual void paintConected( Pipe pipe = null, QuadResourceType resource_type = QuadResourceType.NONE, int origin_dir = 0, HashSet<Pipe> next_pipes = null, Action<HashSet<Pipe>> callback = null )
  {
    if ( resource_type == QuadResourceType.NONE )
      despawnWaterFalls();

    if ( next_pipes?.Count != 0 )
      despawnWaterFalls();

    calcWaterfall( pipe );

    conector_controller.paintConected( resource_type, origin_dir, next_pipes, callback );
  }

  public void spawnWaterfall( int dir )
  {
    if ( dir_waterfall.ContainsKey( dir ) )
      return;

    float angle = 90.0f * dir;
    Vector3 rotation = new Vector3( 0.0f, angle, 0.0f );

    WaterFallController new_waterfall = spawnManager.spawnWaterFall( conectorRoot, Quaternion.Euler( rotation )  );

    spawnedWaterfalls.Add( new_waterfall );
    dir_waterfall.Add( dir, new_waterfall );
  }

  public void despawnWaterFalls()
  {
    foreach( WaterFallController waterfall in spawnedWaterfalls )
      waterfall.despawn();

    spawnedWaterfalls.Clear();
    dir_waterfall.Clear();
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }
  #endregion

  #region Private Methods
  private void updateState( float angle, bool is_reverce )
  {
    quad_entity.curent_rotation = angle;
    onBeginRotate.Invoke( quad_entity, is_reverce );
  }

  private void onRotateHandle()
  {
    onRotate.Invoke();
  }

  private void calcWaterfall( Pipe pipe )
  {
    if ( pipe == null )
      return;

    int inner_dir = pipe.inner_dirs.FirstOrDefault();

    foreach( int dir in pipe.quad.getLocalNextConections( inner_dir ) )
    {
      if ( pipe.dir_children.ContainsKey( dir ) )
        continue;

      float angle = 90.0f * dir;
      Vector3 rotation = new Vector3( 0.0f, angle, 0.0f );
      spawnWaterfall( dir );
    }
  }
  #endregion
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class CellControllerBase : MonoBehaviourPoolable
{
  [SerializeField] private CellMovementController movement_controller = null;
  [SerializeField] private PipeResourceController pipe_resource_controller = null;
  [SerializeField] private ClickableBase3D clickable_base = null;

  private CellData cell_data = null;
  private bool is_interactible = true;

  public event Action onBeginRotate = delegate{};
  public event Action onRotate = delegate{};

  public virtual void init( CellData cell_data )
  {
    deinit();
    this.cell_data = cell_data;
    is_interactible = true;
    clickable_base.onClick += rotateCellForward;
    movement_controller.init( cell_data.curent_rotation );
    movement_controller.onRotate += finishRotation;
  }

  public virtual void deinit()
  {
    movement_controller.deinit();
    clickable_base.onClick -= rotateCellForward;
    movement_controller.onRotate -= finishRotation;
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }

  public virtual void setInteractible( bool state )
  {
    is_interactible = state;
  }

  public virtual void rotateCellForward()
  {
    if ( !is_interactible )
      return;

    onBeginRotate.Invoke();
    cell_data.curent_rotation = (RotationStage)(((int)cell_data.curent_rotation + 1) % 4);
    movement_controller.rotateOverTime( cell_data.curent_rotation );
    CellDataHelper.updateCellData( cell_data );
  }

  public virtual void rotateCellBackward()
  {
    if ( !is_interactible )
      return;

    onBeginRotate.Invoke();
    cell_data.curent_rotation = (RotationStage)(((int)cell_data.curent_rotation - 1) % 4);
    movement_controller.rotateOverTime( cell_data.curent_rotation );
    CellDataHelper.updateCellData( cell_data );
  }

  public void finishRotation()
  {
    onRotate.Invoke();
  }

  public virtual void fillFrom( ResourceType resource_type, CellPipeDirection direction, HashSet<Pipe> next_pipes = null, Action<HashSet<Pipe>> callback = null ) // call to start filling pipe from direction
  {
    //pipe_resource_controller.fillRecource( resource_type, (int)direction, next_pipes, callback );
  }

  public event Action<CellPipeDirection, Action> onFinishFill = delegate{}; // invoke after filling and return free dirs and callback for spawnWaterFall()

  public virtual void spawnWaterFall( CellPipeDirection[] directions )
  {
  }

  public virtual void killFilling() // on pipe disconection
  {
    //pipe_resource_controller.fillRecource( ResourceType.NONE );
  }
}

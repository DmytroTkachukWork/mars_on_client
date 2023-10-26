using System;
using System.Collections.Generic;
using UnityEngine;

public class ConectorController : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private ConectorTypePair[] conector_type_pairs = null;
  #endregion

  #region Private Fields
  private QuadConectionType conection_type = QuadConectionType.NONE;
  private ConectorPipesController curent_conector_pipes_controller = null;
  private bool is_painting_finished = false;
  private Action<HashSet<Pipe>> cached_callback = null;
  private HashSet<Pipe> cached_pipes = null;
  #endregion

  #region Public Fields
  public QuadConectionType conectionType => conection_type;
  #endregion


  #region Public Methods
  public void init( QuadConectionType conection_type )
  {
    this.conection_type = conection_type;

    foreach( ConectorTypePair pair in conector_type_pairs )
    {
      if ( pair.conector_pipes_controller.conectionType == conection_type )
      {
        curent_conector_pipes_controller = pair.conector_pipes_controller;
        curent_conector_pipes_controller.gameObject.SetActive( true );
        continue;
      }

      pair.conector_pipes_controller.gameObject.SetActive( false );
    }
  }

  public void deinit()
  {
    foreach( ConectorTypePair pair in conector_type_pairs )
      pair.conector_pipes_controller.deinit();
  }

  public bool paintConected( QuadResourceType resource_type, int origin_dir, HashSet<Pipe> next_pipes = null, Action<HashSet<Pipe>> callback = null )
  {
    cached_pipes = next_pipes;
    cached_callback = callback;
    is_painting_finished = false;
    return curent_conector_pipes_controller.paintConectedCor( resource_type, origin_dir, onPaintFinished ) && next_pipes != null;
  }

  public bool isAlreadyPainted( QuadResourceType resource_type )
  {
    return is_painting_finished;
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }
  #endregion

  #region Private Methods
  private void onPaintFinished()
  {
    is_painting_finished = true;
    cached_callback?.Invoke( cached_pipes );
  }
  #endregion
}

[Serializable]
public class ConectorTypePair
{
  [SerializeField] public ConectorPipesController conector_pipes_controller = null;
  [SerializeField] public QuadConectionType type = QuadConectionType.NONE;
}
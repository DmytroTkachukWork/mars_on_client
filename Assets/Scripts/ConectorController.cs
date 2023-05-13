using System;
using UnityEngine;

public class ConectorController : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private ConectorTypePair[] conector_type_pairs = null;
  #endregion

  #region Private Fields
  private QuadConectionType conection_type = QuadConectionType.NONE;
  private ConectorPipesController curent_conector_pipes_controller = null;
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
      pair.conector_pipes_controller.gameObject.SetActive( false );
  }

  public void paintConected( QuadResourceType resource_type, int origin_dir )
  {
    curent_conector_pipes_controller.paintConected( resource_type, origin_dir );
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }
  #endregion
}

[Serializable]
public class ConectorTypePair
{
  [SerializeField] public ConectorPipesController conector_pipes_controller = null;
  [SerializeField] public QuadConectionType type = QuadConectionType.NONE;
}
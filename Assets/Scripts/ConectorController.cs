using System;
using UnityEngine;

public class ConectorController : MonoBehaviourPoolable
{
  #region Serialized Fields
  [SerializeField] private ConectorTypePair[] conector_type_pairs = null;
  #endregion

  #region Private Fields
  private QuadConectionType conection_type = QuadConectionType.NONE;
  #endregion

  #region Public Fields
  public QuadConectionType conectionType => conection_type;
  #endregion


  #region Public Methods
  public void init( QuadConectionType conection_type )
  {
    this.conection_type = conection_type;

    foreach( ConectorTypePair pair in conector_type_pairs )
      pair.game_object.SetActive( pair.type == conection_type );
  }

  public void deinit()
  {
    foreach( ConectorTypePair pair in conector_type_pairs )
      pair.game_object.SetActive( false );
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
  [SerializeField] public GameObject game_object = null;
  [SerializeField] public QuadConectionType type = QuadConectionType.NONE;
}
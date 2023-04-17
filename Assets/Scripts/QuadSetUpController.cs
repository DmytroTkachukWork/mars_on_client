using UnityEngine;


public class QuadSetUpController : MonoBehaviourBase
{
  #region Public Fields
  public Transform spawn_root = null;
  #endregion

  #region Private Fields
  private ConectorController spawned_conector = null;
  private int type_int = 0;
  #endregion


  #region Public Methods
  public void init( int type )
  {
    type_int = type;

    spawnConector();
  }

  public QuadConectionType getType()
  {
    return (QuadConectionType)type_int;
  }
  #endregion

  #region Private Methods
  private void OnMouseDown()
  {
    type_int++;
    spawnConector();
  }

  private void spawnConector()
  {
    if ( spawned_conector != null )
      spawned_conector.onDespawn();

    if ( type_int >= 6 )
      type_int = 0;

    spawned_conector = spawnManager.spawnConector( spawn_root );
    spawned_conector.init( (QuadConectionType)type_int );
  }
  #endregion
}

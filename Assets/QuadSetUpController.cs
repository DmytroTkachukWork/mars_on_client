using UnityEngine;


public class QuadSetUpController : MonoBehaviour
{
  #region Public Fields
  public Transform spawn_root = null;
  #endregion

  #region Private Fields
  private ConectorController spawned_conector = null;
  private int type_int = 0;
  private SpawnManager spawn_manager = null;
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
      Destroy( spawned_conector.gameObject );

    if ( spawn_manager == null )
      spawn_manager = FindObjectOfType<SpawnManager>();

    if ( type_int >= 6 )
      type_int = 0;

    spawned_conector = spawn_manager.spawnConector( spawn_root, (QuadConectionType)type_int );
  }
  #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadContentController : MonoBehaviour
{
  #region Serialized Fields
  [SerializeField] private QuadMovementController movement_controller = null;
  [SerializeField] private Transform conector_root = null;
  #endregion

  #region Private Fields
  private QuadState quad_state = new QuadState();
  #endregion

  #region Public Fields
  public Transform conectorRoot => conector_root;
  #endregion


  #region Public Methods
  public void init( float angle, QuadConectionType conection_type )
  {
    quad_state.angle = angle;
    quad_state.conection_type = conection_type;
    movement_controller.onRotate += updateState;
  }

  public void deinit()
  {
    movement_controller.onRotate -= updateState;
  }
  #endregion

  #region Private Methods
  private void updateState( float angle )
  {
    quad_state.angle = angle;
  }
  #endregion
}

public enum QuadConectionType
{
  NONE = 0,        // no conections
  LONG = 1,        // conects 2 oposit sides
  CORNER = 2,      // conects 2 nearby sides
  LONG_CORNER = 3, // conects 2 oposit sides and 1 nearby side
  TWO_CORNERS = 4, // conects 2 nearby sides twice with no conection in the middle
  CRIST = 5        // conects all sides
}

public class QuadState
{
  public float angle = 0.0f;
  public QuadConectionType conection_type = QuadConectionType.NONE;
  public int[] conection_matrix = new int[4];
}
using System;
using System.Threading.Tasks;
using UnityEngine;

public class QuadContentController : MonoBehaviour
{
  #region Serialized Fields
  [SerializeField] private QuadMovementController movement_controller = null;
  [SerializeField] private Transform conector_root = null;
  [SerializeField] private MeshRenderer renderer = null;
  [SerializeField] private Material conected_mat = null;
  [SerializeField] private Material disconected_mat = null;
  #endregion

  #region Private Fields
  private QuadState quad_state = new QuadState();
  #endregion

  #region Public Fields
  public Transform conectorRoot => conector_root;
  public int[] conectionMatrix => quad_state.conection_matrix;
  public bool isTwoCornersType => quad_state.conection_type == QuadConectionType.TWO_CORNERS;
  public event Action onRotate = delegate{};
  #endregion


  #region Public Methods
  public void init( float angle, QuadConectionType conection_type )
  {
    quad_state.angle = angle;
    quad_state.conection_type = conection_type;
    quad_state.conection_matrix = quad_state.getMatrix( conection_type );
    movement_controller.onRotate += updateState;
  }

  public void deinit()
  {
    movement_controller.onRotate -= updateState;
  }

  public void paintConected( bool state )
  {
    renderer.material = state ? conected_mat : disconected_mat;
  }
  #endregion

  #region Private Methods
  private void updateState( float angle )
  {
    quad_state.conection_matrix = quad_state.rotateQuadByAngle( quad_state.conection_matrix, angle );
    quad_state.angle = angle;
    onRotate.Invoke();
    Debug.Log( $"{this.gameObject.name}" );
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

  public int[] getMatrix( QuadConectionType type )
  {
    // 0 - no conection, 1 - has conection, 2 - has uniq conection
    int[] matrix_none = new int[4]{ 0, 0, 0, 0 };
    int[] matrix_long = new int[4]{ 0, 1, 0, 1 };
    int[] matrix_corn = new int[4]{ 1, 1, 0, 0 };
    int[] matrix_lncr = new int[4]{ 1, 1, 0, 1 };
    int[] matrix_twcr = new int[4]{ 1, 1, 2, 2 };
    int[] matrix_crst = new int[4]{ 1, 1, 1, 1 };

    switch( type )
    {
    case QuadConectionType.LONG:        return matrix_long;
    case QuadConectionType.CORNER:      return matrix_corn;
    case QuadConectionType.LONG_CORNER: return matrix_lncr;
    case QuadConectionType.TWO_CORNERS: return matrix_twcr;
    case QuadConectionType.CRIST:       return matrix_crst;
    case QuadConectionType.NONE:
    default:                            return matrix_none;
    }
  }

  public int[] rotateQuadByAngle( int[] matrix, float angle )
  {
    angle = Mathf.Round(angle - this.angle);
    int count = (int)(angle / 90.0f);
    if ( count < 0 )
      count += 4;

    for(int i = 0; i < count; i++)
      rotateQuad( matrix );

    return matrix;
  }


  public int[] rotateQuad( int[] matrix )
  {
    int matrix0 = matrix[3];
    matrix[3]   = matrix[2];
    matrix[2]   = matrix[1];
    matrix[1]   = matrix[0];
    matrix[0]   = matrix0;
    return matrix;
  }
}
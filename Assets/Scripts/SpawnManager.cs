using UnityEngine;
using System.Linq;


public class SpawnManager : MonoBehaviour
{
  #region Serialized Fields
  [SerializeField] private Transform screen_ui = null;
  [SerializeField] private Transform screen_3d = null;
  [SerializeField] private QuadContentController quad_prefab = null;
  [SerializeField] private ConectorController[] conector_prefabs = null;
  [SerializeField] private QuadSetUpController quad_setup_prefab = null;
  [SerializeField] private ScreenMainUI screen_main_ui = null;
  [SerializeField] private ScreenLevelsUI screen_levels_ui = null;
  [SerializeField] private ScreenLevelUI screen_level_ui = null;
  [SerializeField] private ScreenLevel3D screen_level_3d = null;
  #endregion

  #region Private Fields
  private ScreenMainUI inst_screen_main_ui = null;
  private ScreenLevelsUI inst_screen_levels_ui = null;
  private ScreenLevelUI inst_screen_level_ui = null;
  private ScreenLevel3D inst_screen_level_3d = null;
  #endregion


  #region Public Methods
  public QuadContentController spawnQuad( Vector3 position, Transform parent_transform )
  {
    return Instantiate( quad_prefab, position, Quaternion.identity, parent_transform );
  }

  public QuadSetUpController spawnQuadSetUp( Vector3 position )
  {
    return Instantiate( quad_setup_prefab, position, Quaternion.identity );
  }

  public ConectorController spawnConector( Transform root_transform, QuadConectionType conection_type )
  {
    ConectorController conector_controller = conector_prefabs.FirstOrDefault( x => x.conectionType == conection_type );
    return Instantiate( conector_controller, root_transform );
  }

  public ScreenMainUI spawnScreenMainUI()
  {
    if ( inst_screen_main_ui == null )
      inst_screen_main_ui = Instantiate( screen_main_ui, screen_ui );

    return inst_screen_main_ui;
  }

  public ScreenLevelsUI spawnScreenLevelsUI()
  {
    if ( inst_screen_levels_ui == null )
      inst_screen_levels_ui = Instantiate( screen_levels_ui, screen_ui );

    return inst_screen_levels_ui;
  }

  public ScreenLevelUI spawnScreenLevelUI()
  {
    if ( inst_screen_level_ui == null )
      inst_screen_level_ui = Instantiate( screen_level_ui, screen_ui );

    return inst_screen_level_ui;
  }

  public ScreenLevel3D spawnScreenLevel3D()
  {
    if ( inst_screen_level_3d == null )
      inst_screen_level_3d = Instantiate( screen_level_3d, screen_3d );

    return inst_screen_level_3d;
  }
  #endregion
}

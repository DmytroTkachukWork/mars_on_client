using UnityEngine;

public class LevelController : MonoBehaviourBase
{
  [SerializeField] private GameObject sector_level_content = null;
  [SerializeField] private GameObject level_content = null;
  [SerializeField] private ClickableBase3D clickable_base = null;
  [SerializeField] private LevelCameraContainerController camera_container = null;

  [SerializeField] private FieldManager field_manager = null;
  [SerializeField] private LevelQuadMatrix[] level_matrixes = null;

  public LevelCameraContainerController cameraContainer => camera_container;

  public void showFar()// like from sector view
  {
    clickable_base.gameObject.SetActive( true );
    clickable_base.onClick -= moveToLevel;
    clickable_base.onClick += moveToLevel;

    level_content.SetActive( false );
    sector_level_content.SetActive( true );

    camera_container.deinit();
    field_manager.deinit();
    spawnManager.despawnScreenLevelUI();
  }

  public void showClose()// like from sector view
  {
    clickable_base.onClick -= moveToLevel;
    clickable_base.gameObject.SetActive( false );

    sector_level_content.SetActive( false );
    level_content.SetActive( true );

    camera_container.init();
    initLevel();
  }

  public void hide()
  {
    clickable_base.onClick -= moveToLevel;
    clickable_base.gameObject.SetActive( false );

    level_content.SetActive( false );
    sector_level_content.SetActive( false );

    camera_container.deinit();
    field_manager.deinit();
    spawnManager.despawnScreenLevelUI();
  }

  public void moveToLevel()
  {
    //start move camera
    Debug.LogError( "moveToLevel" );
    cameraController.moveCameraToLevel( this );
  }

  public void initLevel()
  {
    Debug.LogError( "initLevel" );
    field_manager.init( level_matrixes[0] );
    spawnManager.despawnScreenLevelsUI();
    spawnManager.spawnScreenLevelUI().init();
  }
}

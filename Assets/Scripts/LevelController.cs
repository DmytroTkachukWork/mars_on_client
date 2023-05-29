using UnityEngine;

public class LevelController : MonoBehaviourBase
{
  [SerializeField] private GameObject sector_level_content = null;
  [SerializeField] private GameObject level_content = null;
  [SerializeField] private ClickableBase3D clickable_base = null;
  [SerializeField] private LevelCameraContainerController camera_container = null;

  [SerializeField] private FieldManager field_manager = null;
  [SerializeField] private int level_id = 0;

  public LevelCameraContainerController cameraContainer => camera_container;

  public void startShowClose()// like from sector view
  {
    clickable_base.onClick -= moveToLevel;
    clickable_base.gameObject.SetActive( false );

    level_content.SetActive( true );
    sector_level_content.SetActive( true );

    initLevel();
  }

  public void finishShowClose()// like from sector view
  {
    camera_container.init();
    sector_level_content.SetActive( false );
  }

  public void startShowFar()// like from sector view
  {
    level_content.SetActive( true );
    sector_level_content.SetActive( true );

    camera_container.deinit();
    spawnManager.despawnScreenLevelUI();
  }

  public void finishShowFar()// like from sector view
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
    field_manager.init( levelsHolder.getLevelById( level_id ) );
    spawnManager.despawnScreenLevelsUI();
    spawnManager.spawnScreenLevelUI().init();
  }
}

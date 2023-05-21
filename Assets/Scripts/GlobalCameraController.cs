using UnityEngine;

public class GlobalCameraController : MonoBehaviourService<GlobalCameraController>
{
  [SerializeField] private Camera main_camera = null;
  [SerializeField] private PlanetCameraContainerController planet_camera_controller = null;
  [SerializeField] private PlanetController curent_planet_controller = null;
  private SectorController curent_sector_controller = null;
  private LevelController curent_level_controller = null;

  private MyTask camera_move = null;
  private float CAMERA_MOVE_TIME = 1.5f;

  public void moveCameraToSector( SectorController sector = null )
  {
    if ( sector != null )
      curent_sector_controller = sector;

    if ( curent_sector_controller == null )
      return;

    main_camera.transform.SetParent( curent_sector_controller.camera_container.cameraRoot );
    camera_move?.stop();
    camera_move = TweenerStatic.tweenTransform( main_camera.transform, curent_sector_controller.camera_container.cameraRoot, CAMERA_MOVE_TIME );

    TweenerStatic.waitAndDo( () =>
    {
      curent_planet_controller.hide();
      curent_sector_controller.showClose();
    }
    , CAMERA_MOVE_TIME/4 );

    Debug.LogError( "TweenerStatic.tweenTransform" );
  }

  public void moveCameraToLevel( LevelController level )
  {
    if ( level != null )
      curent_level_controller = level;

    if ( curent_level_controller == null )
      return;

    main_camera.transform.SetParent( curent_level_controller.cameraContainer.cameraRoot );
    curent_sector_controller?.hide();
    camera_move?.stop();
    camera_move = TweenerStatic.tweenTransform( main_camera.transform, curent_level_controller.cameraContainer.cameraRoot, CAMERA_MOVE_TIME );

    TweenerStatic.waitAndDo( () =>
    {
      curent_level_controller.showClose();
    }
    , CAMERA_MOVE_TIME/4 );

    Debug.LogError( "TweenerStatic.tweenTransform" );
  }

  public void moveCameraToPlanet()
  {
    main_camera.transform.SetParent( planet_camera_controller.cameraRoot );
    curent_sector_controller?.showFar();
    camera_move?.stop();
    camera_move = TweenerStatic.tweenTransform( main_camera.transform, planet_camera_controller.cameraRoot, CAMERA_MOVE_TIME);

    TweenerStatic.waitAndDo( () =>
    {
      curent_planet_controller.showClose();
    }
    , CAMERA_MOVE_TIME/4 );
  }
}

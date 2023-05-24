using UnityEngine;

public class GlobalCameraController : MonoBehaviourService<GlobalCameraController>
{
  [SerializeField] private Camera main_camera = null;
  [SerializeField] private PlanetCameraContainerController planet_camera_controller = null;
  [SerializeField] private PlanetController curent_planet_controller = null;
  private SectorController curent_sector_controller = null;
  private LevelController curent_level_controller = null;

  private MyTask camera_move = null;
  private float CAMERA_MOVE_TIME = 1f;

  public void moveCameraToSectorFromPlanet( SectorController sector = null )
  {
    if ( sector != null )
      curent_sector_controller = sector;

    if ( curent_sector_controller == null )
      return;

    main_camera.transform.SetParent( curent_sector_controller.camera_container.cameraRoot );
    camera_move?.stop();
    curent_sector_controller.startShowClose();
    camera_move = Service<Tweener>.get().tweenTransform( main_camera.transform, curent_sector_controller.camera_container.cameraRoot, CAMERA_MOVE_TIME, null, CurveType.EASE_IN_OUT );

    Service<Tweener>.get().waitAndDo( () =>
    {
      curent_sector_controller.finishShowClose();
      curent_planet_controller.hide();
    }
    , CAMERA_MOVE_TIME );

    Debug.LogError( "tweener.tweenTransform" );
  }

  public void moveCameraToSectorFromLevel( SectorController sector = null )
  {
    if ( sector != null )
      curent_sector_controller = sector;

    if ( curent_sector_controller == null )
      return;

    main_camera.transform.SetParent( curent_sector_controller.camera_container.cameraRoot );
    camera_move?.stop();
    curent_level_controller.startShowFar();
    curent_sector_controller.startShowClose();
    camera_move = Service<Tweener>.get().tweenTransform( main_camera.transform, curent_sector_controller.camera_container.cameraRoot, CAMERA_MOVE_TIME, null, CurveType.EASE_IN_OUT );

    Service<Tweener>.get().waitAndDo( () =>
    {
      curent_level_controller.finishShowFar();
      curent_sector_controller.finishShowClose();
    }
    , CAMERA_MOVE_TIME );

    Debug.LogError( "tweener.tweenTransform" );
  }

  public void moveCameraToLevel( LevelController level )
  {
    if ( level != null )
      curent_level_controller = level;

    if ( curent_level_controller == null )
      return;

    main_camera.transform.SetParent( curent_level_controller.cameraContainer.cameraRoot );
    camera_move?.stop();
    curent_level_controller.startShowClose();
    camera_move = Service<Tweener>.get().tweenTransform( main_camera.transform, curent_level_controller.cameraContainer.cameraRoot, CAMERA_MOVE_TIME, null, CurveType.EASE_IN_OUT );

    Service<Tweener>.get().waitAndDo( () =>
    {
      curent_level_controller.finishShowClose();
      curent_sector_controller?.hide();
    }
    , CAMERA_MOVE_TIME );

    Debug.LogError( "tweener.tweenTransform" );
  }

  public void moveCameraToPlanet()
  {
    main_camera.transform.SetParent( planet_camera_controller.cameraRoot );
    camera_move?.stop();
    curent_planet_controller.showClose();
    curent_sector_controller?.startShowFar();
    camera_move = Service<Tweener>.get().tweenTransform( main_camera.transform, planet_camera_controller.cameraRoot, CAMERA_MOVE_TIME, null, CurveType.EASE_IN_OUT );

    Service<Tweener>.get().waitAndDo( () =>
    {
      curent_sector_controller?.finishShowFar();
    }
    , CAMERA_MOVE_TIME );
  }
}

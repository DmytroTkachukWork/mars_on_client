using UnityEngine;

public class GlobalCameraController : MonoBehaviourService<GlobalCameraController>
{
  #region Serialized Fields
  [SerializeField] private Camera main_camera = null;
  #endregion

  #region Private Fields
  private PlanetController curent_planet_controller = null;
  private SectorController curent_sector_controller = null;
  private LevelController curent_level_controller = null;
  private MyTask camera_move = null;
  private MyVariables my_variables = null;
  private Tweener tweener = null;
  #endregion


  #region Public Methosd
  public void init( PlanetController planet_controller )
  {
    my_variables = Service<MyVariables>.get();
    tweener = Service<Tweener>.get();
    curent_planet_controller = planet_controller;
    main_camera.transform.SetParent( curent_planet_controller.cameraContainer.cameraRoot );
    main_camera.transform.localPosition = Vector3.zero;
    main_camera.transform.localRotation = Quaternion.identity;
  }

  public void moveCameraToSectorFromPlanet( SectorController sector = null )
  {
    if ( sector != null )
      curent_sector_controller = sector;

    if ( curent_sector_controller == null )
      return;

    main_camera.transform.SetParent( curent_sector_controller.cameraContainer.cameraRoot );
    camera_move?.stop();
    curent_planet_controller.startHide();
    curent_sector_controller.startShowClose();
  
    camera_move = tweener.tweenTransform( main_camera.transform, curent_sector_controller.cameraContainer.cameraRoot, my_variables.CAMERA_MOVE_TIME, callback, CurveType.EASE_IN_OUT );

    void callback()
    {
      curent_planet_controller.hide( curent_sector_controller );
      curent_sector_controller.finishShowClose();
    }
  }

  public void moveCameraToSectorFromLevel( SectorController sector = null )
  {
    if ( sector != null )
      curent_sector_controller = sector;

    if ( curent_sector_controller == null )
      return;

    main_camera.transform.SetParent( curent_sector_controller.cameraContainer.cameraRoot );
    camera_move?.stop();

    curent_level_controller.startShowFar();
    curent_sector_controller.startShowClose();

    camera_move = tweener.tweenTransform( main_camera.transform, curent_sector_controller.cameraContainer.cameraRoot, my_variables.CAMERA_MOVE_TIME, callback, CurveType.EASE_IN_OUT );

    void callback()
    {
      curent_level_controller.finishShowFar();
      curent_sector_controller.finishShowClose();
    }
  }

  public void moveCameraToLevel( LevelController level )
  {
    if ( level != null )
      curent_level_controller = level;

    if ( curent_level_controller == null )
      return;

    main_camera.transform.SetParent( curent_level_controller.cameraContainer.cameraRoot );
    camera_move?.stop();

    curent_sector_controller?.cameraContainer.deinit();
    curent_level_controller.startShowClose();

    camera_move = tweener.tweenTransform( main_camera.transform, curent_level_controller.cameraContainer.cameraRoot, my_variables.CAMERA_MOVE_TIME, callback, CurveType.EASE_IN_OUT );

    void callback()
    {
      curent_sector_controller?.hide();
      curent_level_controller.finishShowClose();
    }
  }

  public void moveCameraToPlanet()
  {
    main_camera.transform.SetParent( curent_planet_controller.cameraContainer.cameraRoot );
    camera_move?.stop();

    curent_planet_controller.startShowClose();
    curent_sector_controller?.startShowFar();

    camera_move = tweener.tweenTransform( main_camera.transform, curent_planet_controller.cameraContainer.cameraRoot, my_variables.CAMERA_MOVE_TIME, callback, CurveType.EASE_IN_OUT );

    void callback()
    {
      curent_sector_controller?.finishShowFar();
      curent_planet_controller.finishShowClose();
    }
  }
  #endregion 
}

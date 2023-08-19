using System.Collections;
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
  private MyVariables my_variables = null;
  private Tweener tweener = null;
  private IEnumerator camera_tweener = null;
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

    curent_planet_controller.startHide();
    curent_sector_controller.startShowClose();

    camera_tweener = camera_tweener.startCoroutineAndStopPrev( tweener.tweenTransform(
        main_camera.transform
      , curent_sector_controller.cameraContainer.cameraRoot
      , my_variables.CAMERA_MOVE_TIME
      , callback
      , CurveType.EASE_IN_OUT
    ) );

    void callback()
    {
      curent_planet_controller.hide( curent_sector_controller );
      curent_sector_controller.finishShowClose();
    }
  }

  public void moveCameraToSectorFromLevel()
  {
    if ( curent_sector_controller == null )
      return;

    main_camera.transform.SetParent( curent_sector_controller.cameraContainer.cameraRoot );

    curent_sector_controller.startShowClose();

    camera_tweener = camera_tweener.startCoroutineAndStopPrev( tweener.tweenTransform(
        main_camera.transform
      , curent_sector_controller.cameraContainer.cameraRoot
      , my_variables.CAMERA_MOVE_TIME
      , callback
      , CurveType.EASE_IN_OUT
    ) );

    void callback()
    {
      curent_sector_controller.finishShowClose();
    }
  }

  public void moveCameraToLevel( LevelController level )
  {
    if ( level != null )
      curent_level_controller = level;

    if ( curent_level_controller == null || curent_sector_controller == null )
      return;

    main_camera.transform.SetParent( curent_level_controller.cameraContainer.cameraRoot );
    curent_sector_controller.cameraContainer.deinit();

    curent_level_controller.startShowClose();

    camera_tweener = camera_tweener.startCoroutineAndStopPrev( tweener.tweenTransform(
        main_camera.transform
      , curent_level_controller.cameraContainer.cameraRoot
      , my_variables.CAMERA_MOVE_TIME
      , callback
      , CurveType.EASE_IN_OUT
    ) );

    void callback()
    {
      curent_sector_controller.hide();
      curent_level_controller.finishShowClose();
    }
  }

  public bool teleportCameraToNextLevel()
  {
    if ( curent_level_controller == null || curent_sector_controller == null )
      return false;

    LevelController next_level_controller = curent_sector_controller.getNextLevel( curent_level_controller );

    if ( next_level_controller == null )
    {
      teleportCameraToNextSectorFromPlanet();
      return false;
    }

    curent_level_controller = next_level_controller;

    main_camera.transform.SetParent( curent_level_controller.cameraContainer.cameraRoot );
    main_camera.transform.localPosition = new Vector3( 0.0f, 0.0f, -10.0f );
    main_camera.transform.localRotation = Quaternion.identity;

    curent_sector_controller.cameraContainer.deinit();
    curent_sector_controller.showCloseVisual();

    curent_level_controller.startShowClose();

    camera_tweener = camera_tweener.startCoroutineAndStopPrev( tweener.tweenTransform(
        main_camera.transform
      , curent_level_controller.cameraContainer.cameraRoot
      , my_variables.CAMERA_MOVE_TIME
      , callback
      , CurveType.EASE_IN_OUT
    ) );

    return true;

    void callback()
    {
      curent_sector_controller.hide();
      curent_level_controller?.finishShowClose();
    }
  }

  public bool teleportCameraToNextSectorFromPlanet()
  {
    if ( curent_sector_controller == null )
      return false;

    SectorController next_sector = curent_planet_controller.getNextSector();

    if ( next_sector == null )
      return false;

    curent_sector_controller = next_sector;

    main_camera.transform.SetParent( curent_sector_controller.cameraContainer.cameraRoot );
    main_camera.transform.localPosition = new Vector3( 0.0f, 0.0f, -300.0f );
    main_camera.transform.localRotation = Quaternion.identity;

    curent_planet_controller.startHide();
    curent_sector_controller.startShowClose();

    camera_tweener = camera_tweener.startCoroutineAndStopPrev( tweener.tweenTransform(
        main_camera.transform
      , curent_sector_controller.cameraContainer.cameraRoot
      , my_variables.CAMERA_MOVE_TIME
      , callback
      , CurveType.EASE_IN_OUT
    ) );

    return true;

    void callback()
    {
      curent_planet_controller.hide( curent_sector_controller );
      curent_sector_controller.finishShowClose();
    }
  }

  public void moveCameraToPlanet()
  {
    main_camera.transform.SetParent( curent_planet_controller.cameraContainer.cameraRoot );

    curent_planet_controller.startShowClose();
    curent_sector_controller?.startShowFar();

    camera_tweener = camera_tweener.startCoroutineAndStopPrev( tweener.tweenTransform(
        main_camera.transform
      , curent_planet_controller.cameraContainer.cameraRoot
      , my_variables.CAMERA_MOVE_TIME
      , callback
      , CurveType.EASE_IN_OUT
    ) );

    void callback()
    {
      curent_sector_controller?.finishShowFar();
      curent_planet_controller.finishShowClose();
    }
  }

  public void rotateCameraToNextSector()
  {
    rotateCameraToSector( curent_planet_controller.getNextSector() );
  }

  public void rotateCameraToPrevSector()
  {
    rotateCameraToSector( curent_planet_controller.getPrevSector() );
  }

  public void rotateCameraToSector( SectorController sector_controller )
  {
    if ( sector_controller == null )
      return;

    Vector3 rot = sector_controller.transform.rotation.eulerAngles;
    rot.x += 90.0f;  
    curent_planet_controller.cameraContainer.rotateTo( Quaternion.Euler( rot ), my_variables.CAMERA_ROTATE_TIME );
  }

  public Vector3 getCameraPos()
  {
    return main_camera.transform.position;
  }
  #endregion
}

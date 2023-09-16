using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class LevelManager : MonoBehaviourService<LevelManager>
{
  private FieldManager curent_field_manager = null;

  public void startLevel( FieldManager field_manager )
  {
    curent_field_manager = field_manager;
  }

  public void undoAction()
  {
    curent_field_manager.undoAction();
  }
  
  public void stopLevel()
  {
    curent_field_manager?.deinit();
  }

  public void pauseLevel()
  {
    curent_field_manager?.pauseLevel();
  }

  public void resumeLevel()
  {
    curent_field_manager?.resumeLevel();
  }

  public void restartLevel()
  {
    curent_field_manager?.deinit();
    Service<Tweener>.get().waitFrameAndDo( () => curent_field_manager?.init() ).start();
  }
}

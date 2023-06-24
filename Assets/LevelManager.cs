using System.Collections;
using System.Collections.Generic;
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
  
}

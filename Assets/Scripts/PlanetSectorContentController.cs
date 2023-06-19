using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSectorContentController : MonoBehaviourBase
{
  [SerializeField] private GameObject content = null;
  [SerializeField] private GameObject selected_vfx = null;

  public void init()
  {
    content.SetActive( true );
    selected_vfx.SetActive( false );
  }

  public void deinit()
  {
    content.SetActive( false );
    selected_vfx.SetActive( false );
  }

  public void markSelected( bool state )
  {
    selected_vfx.SetActive( state );
  }
}

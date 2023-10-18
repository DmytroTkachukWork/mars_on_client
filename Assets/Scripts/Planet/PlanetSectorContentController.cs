using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSectorContentController : MonoBehaviourBase
{
  [SerializeField] private GameObject selected_vfx = null;
  [SerializeField] private GameObject unselected_vfx = null;

  public void init()
  {
    selected_vfx.SetActive( false );
  }

  public void deinit()
  {
    selected_vfx.SetActive( false );
  }

  public void markSelected( bool state )
  {
    selected_vfx.SetActive( state );
    unselected_vfx.SetActive( !state );
  }
}

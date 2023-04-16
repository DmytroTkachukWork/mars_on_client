using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMain3D : MonoBehaviourPoolable
{
  #region Serilized Fields
  [SerializeField] private Spiner[] spiners = null;
  #endregion


  #region Public Methods
  public void init()
  {
    foreach( Spiner spiner in spiners )
      spiner.init();
  }

  public void deinit()
  {
    foreach( Spiner spiner in spiners )
      spiner.deinit();
  }

  public override void onSpawn()
  {
    base.onSpawn();

    init();
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }
  #endregion
}

using UnityEngine;

public class ScreenLevels3D : MonoBehaviourPoolable
{
  #region Serilized Fields
  [SerializeField] private MapLevel3DController[] levels = null;
  #endregion


  #region Public Methods
  public void init()
  {
    foreach( MapLevel3DController controller in levels )
    {
      controller.init();
      controller.onClick += startLevel;
    }
  }

  public void deinit()
  {
    foreach( MapLevel3DController controller in levels )
    {
      controller.onClick -= startLevel;
      controller.deinit();
    }
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

  #region Private Methods
  private void startLevel( int level_number )
  {
    spawnManager.spawnScreenLevel3D().init( level_number );
    spawnManager.spawnScreenLevelUI();
    spawnManager.despawnScreenLevelsUI();
    spawnManager.despawnScreenLevels3D();
  }
  #endregion
}
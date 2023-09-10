using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelInfoUI : MonoBehaviourPoolable
{
  [SerializeField] private TMP_Text level_number = null;
  [SerializeField] private Image access_indicator = null;
  [SerializeField] private Image[] stars = null;
  private LevelController root_level = null;
  private Transform root = null;
  private IEnumerator pos_cor = null;

  public void init( LevelController level )
  {
    deinit();
    root_level = level;
    root = level.transform;
    pos_cor = tweener.updateUntil( updatePosition );
    pos_cor.start();

    level_number.text = $"0{level.levelID+1}";

    access_indicator.enabled = playerDataManager.hasAccessToLevel( level.sectorID, level.levelID );

    foreach( Image star in stars )
      star.enabled = false;

    for( int i = 0; i < playerDataManager.getStarsCount( level.sectorID, level.levelID ); i++ )
      stars[i].enabled = true;
  }

  public void deinit()
  {
    pos_cor?.stop();
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }

  private void updatePosition()
  {
    Vector3 screen_pos = Camera.main.WorldToScreenPoint( root.position );
    screen_pos.z = 0;
    transform.position = screen_pos;
  }
}

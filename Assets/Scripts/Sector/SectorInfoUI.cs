using System.Collections;
using UnityEngine;
using TMPro;

public class SectorInfoUI : MonoBehaviourPoolable
{
  [SerializeField] private CanvasGroup canvas_group = null;
  [SerializeField] private TMP_Text sector_number = null;
  [SerializeField] private TMP_Text percent = null;
  [SerializeField] private Color selected_color = Color.yellow;
  [SerializeField] private Color unselected_color = Color.white;
  private SectorController root_sector = null;
  private Transform root = null;
  private IEnumerator pos_cor = null;

  public void init( SectorController sector )
  {
    deinit();
    root_sector = sector;
    root = sector.transform;
    root_sector.onMarkSelected += onSectorMarkSelected;
    pos_cor = tweener.updateUntil( updatePosition );
    pos_cor.start();

    sector_number.text = $"0{sector.sectorID+1}";
    percent.text = $"{playerDataManager.getProgressBySector( sector.sectorID )}%";
  }

  public void deinit()
  {
    pos_cor?.stop();

    if ( root_sector != null )
      root_sector.onMarkSelected -= onSectorMarkSelected;
  }

  public override void onDespawn()
  {
    base.onDespawn();

    deinit();
  }

  private void updatePosition()
  {
    Vector3 screen_pos = Camera.main.WorldToScreenPoint( root.position );
    float distance = Vector3.Distance( Camera.main.transform.position, root.position );

    distance = distance - myVariables.SECTOR_INFO_OFFSET_FACTOR;
    distance = distance / myVariables.SECTOR_INFO_SCALE_FACTOR;
    distance = 1 - distance;
    distance = distance * 2;

    float scale = distance;

    canvas_group.alpha = scale;
    Vector3 local_scale = new Vector3( scale, scale, 1.0f );
    transform.localScale = local_scale;
    screen_pos.z = 0;
    transform.position = screen_pos;
  }

  private void onSectorMarkSelected( bool state )
  {
    Color cached_color = state ? selected_color : unselected_color;
    sector_number.color = cached_color;
    percent.color = cached_color;
  }
}

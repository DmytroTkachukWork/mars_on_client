using System;
using UnityEngine;

public class LeftRightButtonsController : MonoBehaviour
{
  [SerializeField] private ButtonBase left_button = null;
  [SerializeField] private ButtonBase right_button = null;

  public event Action onLeftClick = delegate{};
  public event Action onRightClick = delegate{};

  public void init( bool is_left_interactive = true, bool is_right_interactive = true )
  {
    deinit();
    left_button.setInteractive( is_left_interactive );
    right_button.setInteractive( is_right_interactive );

    left_button.onClick += onLeftClickImpl;
    right_button.onClick += onRightClickImpl;
  }

  public void deinit()
  {
    left_button.onClick -= onLeftClickImpl;
    right_button.onClick -= onRightClickImpl;
  }

  private void onLeftClickImpl()
  {
    onLeftClick.Invoke();
  }

  private void onRightClickImpl()
  {
    onRightClick.Invoke();
  }
}

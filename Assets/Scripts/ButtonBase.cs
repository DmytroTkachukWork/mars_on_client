using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviourBase
{
  #region Serialized Fields
  [SerializeField] private Button button = null;
  [SerializeField] private int int_value = 0;
  #endregion

  #region Private Fields
  private int cached_int_value = 0;
  #endregion

  #region Public Fields
  public event Action onClick = delegate{};
  public event Action<int> onClickInt = delegate{};
  #endregion


  #region Public Methods
  private void Start()
  {
    init();
  }

  public void init( int value = -1 )
  {
    Debug.LogError( "init ButtonBase" );
    cached_int_value = value < 0 ? int_value : value;
    button.onClick.AddListener( onButtonClick );
  }

  public void deinit()
  {
    button.onClick.RemoveListener( onButtonClick );
  }
  #endregion

  #region Private Methods
  private void onButtonClick()
  {
    Debug.LogError( "onButtonClick" );
    onClick.Invoke();
    onClickInt.Invoke( cached_int_value );
  }
  #endregion
}

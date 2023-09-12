using System;
using UnityEngine;


[Serializable] [CreateAssetMenu( fileName = "CardInfo", menuName = "ScriptableObjects/CardInfo", order = 8 )]
public class CardInfo : ScriptableObject
{
  [SerializeField] private int    card_number    = 0;
  [SerializeField] private string header_text    = null;
  [SerializeField] private string body_text      = null;
  [SerializeField] private Sprite card_avatar    = null;
  [SerializeField] private Sprite card_lendscape = null;

  public int    cardNumber    => card_number;
  public string headerText    => header_text;
  public string bodyText      => body_text;
  public Sprite cardAvatar    => card_avatar;
  public Sprite cardLendscape => card_lendscape;
}

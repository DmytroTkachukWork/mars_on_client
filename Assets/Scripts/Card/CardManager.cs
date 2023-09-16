using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviourService<CardManager>
{
  [SerializeField] private CardInfo[] card_infos = null;

  public CardInfo getCardInfoByIndex( int index )
  {
    if ( index >= card_infos.Length )
      return null;

    return card_infos[index];
  }
}

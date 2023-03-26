using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectorController : MonoBehaviour
{
  #region Serialized Fields
  [SerializeField] private QuadConectionType conection_type = QuadConectionType.NONE;
  #endregion

  #region Public Fields
  public QuadConectionType conectionType => conection_type;
  #endregion
}

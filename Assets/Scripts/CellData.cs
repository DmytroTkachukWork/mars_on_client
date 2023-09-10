using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CellData
{
  [SerializeField]public CellRoleType role = CellRoleType.NONE;
  [SerializeField]public ResourceType resource_type = ResourceType.NONE;
  [SerializeField]public CellPipeType pipe_type = CellPipeType.NONE;

  public bool isConected = false;
  public CellPipeDirection[] all_diractions = null;
  public CellPipeDirection[] inner_diractions = null;
  public CellPipeDirection[] outer_diractions = null;
}

public enum CellPipeDirection
{
  TOP = 0,
  RIGHT = 1,
  BOTTOM = 2,
  LEFT = 3
}

public enum ResourceType
{
  NONE = 0,
  WATER = 1,
  AIR = 2
}

public enum CellPipeType
{
  NONE = 0,        // no conections
  LONG = 1,        // conects 2 oposit sides
  CORNER = 2,      // conects 2 nearby sides
  LONG_CORNER = 3, // conects 2 oposit sides and 1 nearby side
  TWO_CORNERS = 4, // conects 2 nearby sides twice with no conection in the middle
  CRIST = 5,        // conects all sides
  ONE = 6
}

public enum CellRoleType
{
  NONE = 0,
  PLAYABLE = 1,
  STARTER = 2,
  FINISHER = 3
}
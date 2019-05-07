using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StateData
{
    public int StateId;
    public int RequiredFame;
    public List<LevelData> LevelsList;
}

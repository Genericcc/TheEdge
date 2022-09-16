using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Squad", menuName = "TheEdge/Squad", order = 0)]
public class SquadCardSO : ScriptableObject 
{
    public int initiative;
    public int attack;
    public int defence;
    public int move;
    public int shields;
}


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

    public int GetDesiredStat(string name)
    {
        switch(name)
        {
            case "Initiative":
                return initiative;
            case "Attack":
                return attack;
            case "Defence":
                return defence;
            case "Move":
                return move;
            case "Shields":
                return shields;
            default:
                Debug.Log("Coś nie bangla");
                return 0;
        }
    }

}


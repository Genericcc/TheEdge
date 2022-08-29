using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "TheEdge/Card", order = 0)]
public class Card : ScriptableObject 
{
    public int initiative;
    public int attack;
    public int defence;
    public int move;
    public int shield;
}


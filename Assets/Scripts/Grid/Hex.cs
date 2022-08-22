using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Hex : MonoBehaviour
{
    private GridPosition hexPosition;

    public GridPosition GetHexPosition => hexPosition; 

    public GridPosition GetHexPositionnnnn() => hexPosition;
    
    private void Awake() 
    {
        hexPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }



}

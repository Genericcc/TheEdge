using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Hex : MonoBehaviour
{
    private GridPosition hexPosition;

    public GridPosition GetHexPosition => hexPosition; 

    //TODO delete the above, and change the name below to the above

    public GridPosition GetHexPositionnnnn() => hexPosition;
    
    private void Awake() 
    {
        hexPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class LargeHex : MonoBehaviour
{
    [SerializeField] private GameObject NorthWestHex; 
    [SerializeField] private GameObject NorthEastHex; 
    [SerializeField] private GameObject WestHex; 
    [SerializeField] private GameObject MiddleHex; 
    [SerializeField] private GameObject EastHex; 
    [SerializeField] private GameObject SouthWestHex; 
    [SerializeField] private GameObject SouthEastHex; 

    private GridPosition hexPosition;

    private void Awake() 
    {
        hexPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetHexPosition() => hexPosition;
    


}

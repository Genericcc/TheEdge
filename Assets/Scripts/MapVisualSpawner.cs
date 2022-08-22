using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVisualSpawner : MonoBehaviour
{
    [SerializeField] private Transform largeHexPrefab;

    private void Awake() 
    {
        for(int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for(int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);
                Transform largeHexSingleTransform = Instantiate(largeHexPrefab,
                                                                LevelGrid.Instance.GetWorldPosition(gridPosition),
                                                                Quaternion.Euler(0,10.9f,0)
                                                                );                                                              
            }
        }
    }
   
}

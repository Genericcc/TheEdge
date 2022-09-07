// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class HexSmallSystem<THex>
// {
// //     private int width;
// //     private int height;
// //     private float cellSizeX;
// //     private float cellSizeY;
// //     private THex[,] hexObjectArray;

// //     public HexSmallSystem(int width, int height, float cellSizeX, float cellSizeY, 
// //         Func< HexSmallSystem<THex>, GridPosition, THex> createHexGridObject) //We need a delegate which creates NEW GridObject 
// //     {                                                                        //that takes 2 parameters 
// //         this.width = width;
// //         this.height = height;
// //         this.cellSizeX = cellSizeX;
// //         this.cellSizeY = cellSizeY;

// //         hexObjectArray = new THex[width, height];

// //         for (int x = 0; x < width; x++) 
// //         {
// //             for (int z = 0; z < height; z++)
// //             {
// //                 GridPosition gridPosition = new GridPosition(x,z);
// //                 hexObjectArray[x,z] = createHexGridObject(this, gridPosition);
// //             }
// //         }
// //     }

// //     public Vector3 GetWorldPositionFromSmallHex(GridPosition gridPosition)
// //     {
// //         if(gridPosition.z % 2 == 0)
// //         {
// //             return new Vector3(gridPosition.x * cellSizeX, 0, gridPosition.z * cellSizeY);
// //         } 
// //         else
// //         {
// //             return new Vector3(gridPosition.x * cellSizeX + (cellSizeX/2), 0, gridPosition.z * cellSizeY);
// //         }
// //     }

// //     public GridPosition GetGridPositionFromSmallHex(Vector3 worldPosition)
// //     {
// //         return new GridPosition(
// //             Mathf.FloorToInt(worldPosition.x / cellSizeX), 
// //             Mathf.RoundToInt(worldPosition.z / cellSizeY)
// //             );

// //         //Check if GridPosition is on an uneven row (Z value between 0.5 cellSizeY and 1.5 cellSizeY)
// //         //float oneAndHalfcellSizeY = 1.5f * cellSizeY;
// //         //
// //         // if(worldPosition.z % oneAndHalfcellSizeY > cellSizeY/2)
// //         // {
// //         //     return new GridPosition(
// //         //         Mathf.FloorToInt(worldPosition.x / cellSizeX), 
// //         //         Mathf.RoundToInt(worldPosition.z / cellSizeY)
// //         //         );
// //         // } 
// //         // else 
// //         // {
// //         //     //even row
// //         //     return new GridPosition(
// //         //         Mathf.RoundToInt(worldPosition.x / cellSizeX),
// //         //         Mathf.RoundToInt(worldPosition.z / cellSizeY)
// //         //         );
// //         // }

// //     }

// //     public void CreateDebugObjectsForSmallHex(Transform debugPrefab) 
// //     {
// //         for (int x = 0; x < width; x++) 
// //         {
// //             for (int z = 0; z < height; z++)
// //             {
// //                 GridPosition gridPosition = new GridPosition(x,z);

// //                 Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPositionFromSmallHex(gridPosition), Quaternion.identity);

// //                 //Grabs a reference to gridDebugObject, so we can use SetGridObject
// //                 GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
// //                 gridDebugObject.SetGridObject(GetGridObjectFromSmallHex(gridPosition));
// //             }
// //         }
// //     }

// //     public THex GetGridObjectFromSmallHex(GridPosition gridPosition)
// //     {
// //         return hexObjectArray[gridPosition.x, gridPosition.z];
// //     }

// //     public bool IsValidGridPositionOnSmallHex(GridPosition gridPosition)
// //     {
// //         return  gridPosition.x >= 0 && 
// //                 gridPosition.z >= 0 &&
// //                 gridPosition.x < width &&
// //                 gridPosition.z < height;
// //     }

// //     public int GetWidthOfSmallHexSystem() => width;
    
// //     public int GetHeightOfSmallHexSystem() => height;


// }

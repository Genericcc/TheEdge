using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    public event EventHandler OnAnyUnitMovedGridPosition;

    [SerializeField] private LayerMask smallHexLayerMask;
    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSizeX;
    [SerializeField] private float cellSizeY;
    
    private GridSystem<GridObject> gridSystem;

    Dictionary<GridPosition, LargeHex> hexDict = new Dictionary<GridPosition, LargeHex>();

    private void Awake() 
    {
        if(Instance != null) 
        {
            Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gridSystem = new GridSystem<GridObject>(width, height, cellSizeX, cellSizeY, 
            (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition) );
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab); 
    }

    private void Start() 
    {
        Pathfinding.Instance.Setup(width, height, cellSizeX, cellSizeY);

        foreach(LargeHex hex in FindObjectsOfType<LargeHex>())
        {
            hexDict[hex.GetHexPosition()] = hex;
        }
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);

        AddUnitAtGridPosition(toGridPosition, unit);

        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public GridSystem<GridObject> GetGridSystem() => gridSystem;

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    // public Unit GetUnitAtSmallHex(Hex smallHex)
    // {
    //     GridObject gridObject = gridSystem.GetGridObject(gridPosition);
    //     return gridObject.GetUnit();
    // }

    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetInteractable();
    }

    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }

    public List<LargeHex> GetNeighbourList(LargeHex currentNode)
    {
        List<LargeHex> neighbourList = new List<LargeHex>();

        GridPosition gridPosition = currentNode.GetHexPosition();

        //even row 
        if(gridPosition.z % 2 == 0)
        {
            if(gridPosition.x - 1 >= 0)
            {
                //Left Node
                neighbourList.Add(GetNeighbourOrSelf(gridPosition, -1, 0));
                
                //Left Down Node
                if(gridPosition.z - 1 >= 0)
                {
                    neighbourList.Add(GetNeighbourOrSelf(gridPosition, -1, -1));
                }

                //Left Up Node
                if(gridPosition.z + 1 < gridSystem.GetHeight())
                {            
                    neighbourList.Add(GetNeighbourOrSelf(gridPosition, -1, 1));    
                }        
            }
            
            if(gridPosition.x + 1 < gridSystem.GetWidth())
            {
                //Right Node
                neighbourList.Add(GetNeighbourOrSelf(gridPosition, 1, 0));
            }

            if(gridPosition.z - 1 >= 0)
            {            
                //Right Down Node
                neighbourList.Add(GetNeighbourOrSelf(gridPosition, 0, - 1));
            }

            if(gridPosition.z + 1 < gridSystem.GetHeight())
            {               
                //Right Up Node
                neighbourList.Add(GetNeighbourOrSelf(gridPosition, 0, 1)); 
            }       
        }

        //uneven row 
        if(gridPosition.z % 2 != 0)
        {
            if(gridPosition.x - 1 >= 0)
            {
                //Left Node
                neighbourList.Add(GetNeighbourOrSelf(gridPosition, -1, 0));
            }
            
            if(gridPosition.z - 1 >= 0)
            {
                //Left Down Node
                neighbourList.Add(GetNeighbourOrSelf(gridPosition, 0, -1));
            }
            
            if(gridPosition.z + 1< gridSystem.GetHeight())
            {   
                //Left Up Node         
                neighbourList.Add(GetNeighbourOrSelf(gridPosition, 0, 1));    
            }        
            
            if(gridPosition.x + 1 < gridSystem.GetWidth())
            {
                //Right Node
                neighbourList.Add(GetNeighbourOrSelf(gridPosition, 1, 0));
  
                //Right Down Node
                if(gridPosition.z - 1 >= 0)
                {
                    neighbourList.Add(GetNeighbourOrSelf(gridPosition, 1, -1));
                }

                //Right Up Node
                if(gridPosition.z + 1 < gridSystem.GetHeight())
                {   
                    neighbourList.Add(GetNeighbourOrSelf(gridPosition, 1, 1)); 
                }     
            }    
        }

        return neighbourList;
    }

    private LargeHex GetNeighbourOrSelf(GridPosition gridPosition, int xShift, int zShift)
    {
        return GetHexFromCoordinates(new GridPosition(gridPosition.x + xShift, gridPosition.z + zShift)) 
            ?? GetHexFromCoordinates(gridPosition);

        //It's the same as the above, but longer
        //
        // if(GetHexFromCoordinates(new GridPosition(gridPosition.x + xShift, gridPosition.z + zShift)) != null)
        // {
        //     return GetHexFromCoordinates(new GridPosition(gridPosition.x + xShift, gridPosition.z + zShift));
        // }
        // else
        // {
        //     return GetHexFromCoordinates(gridPosition);
        // }
    }
    
    public LargeHex GetHexFromCoordinates(GridPosition hexPosition)
    {
        LargeHex result = null;
        hexDict.TryGetValue(hexPosition, out result);

        return result;
    }

    //Old version of GetNeighbourOrSelf
    // private LargeHex GetLargeHex(int x, int z)
    // {
    //     return GetHexFromCoordinates(new GridPosition(x, z)); 
    // }

    public List<Hex> GetSmallNeighbours(Hex currentSmallHex)
    {
        List<Hex> smallNeighbourList = new List<Hex>();

        int overlapSphereRadius = 10;
        Vector3 hexWorldPosition = currentSmallHex.transform.position;

        Collider[] colliderArray = Physics.OverlapSphere(hexWorldPosition, overlapSphereRadius, smallHexLayerMask);

        foreach(Collider collider in colliderArray)
        {
            Hex neighbour;
            collider.transform.parent.TryGetComponent<Hex>(out neighbour);
            smallNeighbourList.Add(neighbour);
            Debug.Log(neighbour);
        }

        return  smallNeighbourList;
    }
}

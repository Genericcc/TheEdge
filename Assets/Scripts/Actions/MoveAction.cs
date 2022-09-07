using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField] private int maxMoveDistance = 4;
    [SerializeField] private float moveSpeed = 2f;

    private List<Vector3> positionList;
    private int currentPositionIndex;
    bool isLargeHexMoveCompleted = false;

    void Update()
    {
        if(!isActive) 
        { 
            return; 
        }

        Vector3 targetPosition;

        if(!isLargeHexMoveCompleted)
        {
            targetPosition = positionList[currentPositionIndex];   
        }
        else    
        {
            targetPosition = HexSelectionManager.Instance.GetSelectedSmallHex().transform.position;
        }  
         
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        float stoppingDistance = .1f;
        if(Vector3.Distance(transform.position, targetPosition) > stoppingDistance) 
        {
            //moveSpeed = 4f;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        } 
        else 
        {
            if(!isLargeHexMoveCompleted)
            {
                currentPositionIndex++;
            }
            
            if(currentPositionIndex >= positionList.Count)
            {
                if(isLargeHexMoveCompleted)
                {   
                    isLargeHexMoveCompleted = false;

                    OnStopMoving?.Invoke(this, EventArgs.Empty);

                    ActionComplete(); 
                }
            
                isLargeHexMoveCompleted = true; //TODO: it always ends up as true...
                
            }
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) 
    {
        List<GridPosition> pathGridPositionList = 
            Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach(GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        LargeHex unitLargeHex = HexSelectionManager.Instance.GetLargeHexBeneath(unit.transform.position);
        Hex unitHex = HexSelectionManager.Instance.GetSmallHexBeneath(unit.transform.position);

        List<LargeHex> neighbourList = LevelGrid.Instance.GetNeighbourList(unitLargeHex);

        for(int i = maxMoveDistance; i > 1; i--)
        {
            foreach(LargeHex neighbour in neighbourList)
            {
                neighbourList = neighbourList.Concat(LevelGrid.Instance.GetNeighbourList(neighbour)).ToList<LargeHex>();
                //TODO: remove duplicates from the list
            }

            maxMoveDistance--;
        }

        neighbourList = neighbourList.Distinct().ToList<LargeHex>();

        foreach(LargeHex hex in neighbourList)
        {
            Debug.Log(hex.GetHexPosition());
        }


        GridPosition unitGridPosition = new GridPosition(0,0);

        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++) 
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    //Outside of the map
                    continue;
                }
                
                if(unitGridPosition == unitGridPosition)
                {
                    //Same GridPosition where the unit already is
                    continue;
                }

                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //GridPosition already occupied with another unit
                    continue;
                }

                if(!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    //Debug.Log("Not walkable");
                    continue;
                }

                if(!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                {
                    //Debug.Log("No path");
                    continue;
                }

                int pathfindingDistanceMultiplier = 10;

                if(Pathfinding.Instance.
                    GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier
                    )
                {
                    //Path length is too long
                    //Debug.Log("Too far");
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }
    
    public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<SwordAction>().GetTargetCountAtPosition(gridPosition);

        //Debug.Log("Target count: " + targetCountAtGridPosition);

        return new EnemyAIAction 
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }


}

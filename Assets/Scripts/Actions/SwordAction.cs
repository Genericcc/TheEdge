using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwordAction : BaseAction
{
    public static event EventHandler OnAnySwordHit;
    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;

    private enum State {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }

    private Unit targetUnit;    
    private State state;
    private float stateTimer;
    private int maxSwordDistance = 1;
    
    private void Update()
    {
        if(!isActive) 
        { 
            return; 
        }

        stateTimer -= Time.deltaTime;

        switch(state)
        {
            case State.SwingingSwordBeforeHit:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;

                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.SwingingSwordAfterHit:

                break;
        }   
  
        if(stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch(state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
                targetUnit.Damage();
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public  bool IsValidActionSmallHex(Hex smallHex)
    {
        List<Hex> validSmallHexList = GetValidActionSmallHexList();
        return validSmallHexList.Contains(smallHex);
    }

    public override List<Hex> GetValidActionSmallHexList()
    {
        List<Hex> validSmallHexList = new List<Hex>();
        List<Hex> smallNeighbourList = LevelGrid.Instance.GetSmallNeighbours(unit.GetSmallHex());

        foreach(Hex neighbour in smallNeighbourList)
        {
            //Debug.Log(neighbour.name);

            if(!LevelGrid.Instance.HasAnyUnitOnSmallHex(neighbour))
            {
                //GridPosition is empty, no unit
                continue;
            }

            Unit targetUnit = LevelGrid.Instance.GetUnitAtSmallHex(neighbour);

            if(targetUnit.IsEnemy() == unit.IsEnemy())
            {
                //Both Units are on the same 'team'
                continue;
            }

            validSmallHexList.Add(neighbour);
            
        }
        return validSmallHexList;
    }
    
    public void TakeActionOnSmallHex(Hex smallHex, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtSmallHex(smallHex);

        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> emptyList = new List<GridPosition>();

        return emptyList;

        // List<GridPosition> validGirdPositionList = new List<GridPosition>();

        // List<Hex> neighbourList = LevelGrid.Instance.GetSmallNeighbours(unit.GetSmallHex());

        // foreach (Hex neighbour in neighbourList)
        // {
        //     GridPosition gridPosition = neighbour.GetComponentInParent<LargeHex>().GetHexPosition();

        //     Debug.Log(gridPosition);

        //     validGirdPositionList.Add(gridPosition);
        // }

        // return validGirdPositionList; //.Distinct<GridPosition>().ToList();
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for(int x = -maxSwordDistance; x <= maxSwordDistance; x++) 
        {
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = offsetGridPosition + gridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    //Outside of the map
                    continue;
                }

                if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //GridPosition is empty, no unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if(targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    //Both Units are on the same 'team'
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction{
            gridPosition = gridPosition,
            actionValue = 200,
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

    public int GetMaxSwordDistance() => maxSwordDistance;

    public override string GetActionName()
    {
        return "Sword";
    }

    public override EnemyAIAction GetBestEnemyAIAction(Hex targetSmallHex)
    {       
        return new EnemyAIAction{
            gridPosition = targetSmallHex.GetComponentInParent<LargeHex>().GetHexPosition(),
            smallHex = targetSmallHex,
            actionValue = 200,
        };
    }

    public int GetTargetCountAtSmallHex(Hex smallHex)
    {
        return GetValidActionSmallHexList(smallHex).Count;
    }
    
    //function for AI to find enemies on potential target smallHex
    public List<Hex> GetValidActionSmallHexList(Hex targetSmallHex)
    {
        List<Hex> validSmallHexList = new List<Hex>();
        List<Hex> smallNeighbourList = LevelGrid.Instance.GetSmallNeighbours(targetSmallHex);

        foreach(Hex neighbour in smallNeighbourList)
        {
            //Debug.Log(neighbour.name);

            if(!LevelGrid.Instance.HasAnyUnitOnSmallHex(neighbour))
            {
                //GridPosition is empty, no unit
                continue;
            }

            Unit targetUnit = LevelGrid.Instance.GetUnitAtSmallHex(neighbour);

            if(targetUnit.IsEnemy() == unit.IsEnemy())
            {
                //Both Units are on the same 'team'
                continue;
            }

            validSmallHexList.Add(neighbour);
            
        }
        return validSmallHexList;
    }

}

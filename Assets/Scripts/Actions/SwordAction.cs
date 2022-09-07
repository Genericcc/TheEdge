using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    [SerializeField] LayerMask hexLayerMask;

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
                targetUnit.Damage(100);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        Vector3 maxSphereCastDistance = new Vector3(10, 10, 10);
        Vector3 unitWorldPosition = unit.GetWorldPosition();

        Collider[] colliderArray = Physics.OverlapSphere(unitWorldPosition, maxSwordDistance, hexLayerMask);

        GridPosition testGridPosition = new GridPosition();

        foreach(Collider collider in colliderArray)
        {
            collider.transform.parent.TryGetComponent<Hex>(out Hex neighbourHex);
            Debug.Log(neighbourHex.name);

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
        return validGridPositionList;
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

    public int GetMaxSwordDistance() => maxSwordDistance;

    public override string GetActionName()
    {
        return "Sword";
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    //public delegate void SpinCompleteDelegate();

    private float totalSpinAmount;
    
    private void Update()
    {
        if(!isActive) 
        { 
            return; 
        }
        
        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpinAmount += spinAddAmount;
        if(totalSpinAmount >= 360f)
        {
            ActionComplete();
        }  
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        totalSpinAmount = 0f;

        ActionStart(onActionComplete);
    }

     public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition> //A list containing only the unitGridPosition
        {
            unitGridPosition
        };
    }

    public override int GetActionPointCost()
    {
        return 2;
    }

    public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction 
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }

    public override EnemyAIAction GetBestEnemyAIAction(Hex smallHex)
    {
        throw new NotImplementedException();
    }

    public override List<Hex> GetValidActionSmallHexList()
    {
        throw new NotImplementedException();
    }
}

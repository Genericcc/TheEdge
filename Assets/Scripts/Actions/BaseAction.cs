using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    protected Action onActionComplete;
    protected Unit unit;
    protected bool isActive; 

    protected virtual void Awake() 
    {
        unit = GetComponent<Unit>();    
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public abstract List<Hex> GetValidActionSmallHexList();

    public virtual int GetActionPointCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        isActive = true;  

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }
    
    public Unit GetUnit()
    {
        return unit;
    }

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        List<Hex> validForSwordActionSmallHexList = GetValidActionSmallHexList();

        foreach(GridPosition gridPosition in validGridPositionList)
        {
            LargeHex largeHex = HexSelectionManager.Instance.GetLargeHexBeneath(LevelGrid.Instance.GetWorldPosition(gridPosition));

            List<Hex> childrenSmallHexList = largeHex.GetComponentsInChildren<Hex>().ToList();

            foreach(Hex smallHex in childrenSmallHexList)
            {
                //Debug.Log(smallHex);
                EnemyAIAction enemyAIAction = GetBestEnemyAIAction(smallHex);
                enemyAIActionList.Add(enemyAIAction);
            }  
        }

        foreach(Hex smallHex in validForSwordActionSmallHexList)
        {
            //Debug.Log(smallHex);
            EnemyAIAction enemyAIAction = GetBestEnemyAIAction(smallHex);
            enemyAIActionList.Add(enemyAIAction);
        }

        if(enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActionList[0];
        } 
        else 
        {
            //No posible Enemy AI Actions
            return null;
        }
    }

    public abstract EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition);

    public abstract EnemyAIAction GetBestEnemyAIAction(Hex smallHex);
    

}

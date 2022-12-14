using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    private State state;
    private float timer;

    private void Awake() 
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start() 
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update() 
    {
        if(TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch(state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if(timer <= 0f)
                {
                    if(TryTakeEnemyAIAction(SetStateToTakingTurn))
                    {
                        state = State.Busy;
                    } else 
                    {
                        //No more enemies have actions to take
                        TurnSystem.Instance.NextTurn();
                    }  
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void SetStateToTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIAcionComplete)
    {
        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIAcionComplete))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIAcionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach(BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            if(!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
            {
                //Enemy cannot afford this action
                continue;
            }

            if(bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                //Debug.Log($"Testing Action with the value of: {bestEnemyAIAction.actionValue}");

                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                    
                }
            }
        }

        if(bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            if(bestBaseAction is MoveAction)
            {
                //TODO: set selectedHex for MoveAction
                //Debug.Log(bestEnemyAIAction.smallHex);
                HexSelectionManager.Instance.RemoteSetSelectedSmallHex(bestEnemyAIAction.smallHex);

                bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIAcionComplete);
                return true;
            }
            else if(bestBaseAction is SwordAction)
            {
                SwordAction swordAction = bestBaseAction as SwordAction;

                swordAction.TakeActionOnSmallHex(bestEnemyAIAction.smallHex, onEnemyAIAcionComplete);
                return true;
            }

            Debug.Log("Fuckup here");
            return false;
        }
        else
        {
            return false;
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public static event EventHandler OnDiceRollStarted;
    public static event EventHandler OnDiceRollFinished;
    public static event EventHandler OnDiceRoll;

    private enum State {
        BeforeDiceRoll,
        DiceRolling,
        AfterDiceRoll,
        Battle,
    }

    private Unit cacheAttacker; 
    private Unit cacheDefender; 
    private SwordAction cacheSwordAction;
    private Action cacheClearBusy;

    private State state;
    private float stateTimer;
    private bool isBattleStarted;
    private int friendlyDiceResult;
    private int enemyDiceResult;

    private void Awake() 
    {
        if(Instance != null) 
        {
            Debug.LogError("There's more than one BattleManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        state = State.Battle;
    }

    private void Update() 
    {
        if(!isBattleStarted)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        if(stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch(state)
        {
            case State.BeforeDiceRoll:
                OnDiceRoll?.Invoke(this, EventArgs.Empty);

                SetState(State.DiceRolling, 2f);
                break;
            case State.DiceRolling:
                AddRollsToStats();
                OnDiceRollFinished?.Invoke(this, EventArgs.Empty);

                SetState(State.AfterDiceRoll, .5f);
                break;
            case State.AfterDiceRoll:            
                Battle();

                SetState(State.Battle, .1f);
                break;
            case State.Battle:
                isBattleStarted = false;
                ClearStats();
                break;
        }
    }
    private void SetState(State newState, float newStateTime)
    {
        state = newState;
        stateTimer = newStateTime;
    }

    private void AddRollsToStats()
    {
        //SquadCardSO originalStats = cacheAttacker.GetStats();
        SquadCardSO friendlyStats = cacheAttacker.GetStats();
        SquadCardSO enemyStats = cacheDefender.GetStats();

        friendlyStats.attack += friendlyDiceResult;
        friendlyStats.defence += friendlyDiceResult;

        enemyStats.attack += enemyDiceResult;
        enemyStats.defence += enemyDiceResult;

        Debug.Log(enemyStats.attack);
        Debug.Log(enemyStats.defence);
    }

    private void ClearStats()
    {
        //SquadCardSO originalStats = cacheAttacker.GetStats();
        SquadCardSO modifiedFriendlyStats = cacheAttacker.GetStats();
        SquadCardSO modifiedEnemyStats = cacheDefender.GetStats();

        modifiedFriendlyStats.attack -= friendlyDiceResult;
        modifiedFriendlyStats.defence -= friendlyDiceResult;

        modifiedEnemyStats.attack -= enemyDiceResult;
        modifiedEnemyStats.defence -= enemyDiceResult;

        Debug.Log(modifiedEnemyStats.attack);
        Debug.Log(modifiedEnemyStats.defence);
    }

    public void BattleSetup(Unit attacker, Unit defender, SwordAction swordAction, Action ClearBusy)
    {
        SetState(State.BeforeDiceRoll, .5f);
        isBattleStarted = true;
        OnDiceRollStarted?.Invoke(this, EventArgs.Empty);

        cacheAttacker = attacker;
        cacheDefender = defender;
        cacheSwordAction = swordAction;
        cacheClearBusy = ClearBusy;
    }   

    private void Battle()
    {
        if(cacheAttacker.GetStats().initiative >= cacheDefender.GetStats().initiative)
        {
            if(cacheAttacker.GetStats().attack > cacheDefender.GetStats().defence)
            {
                cacheSwordAction.TakeActionOnSmallHex(cacheDefender.GetSmallHex(), cacheClearBusy);
            } 
        }
        else //Attacker has lower initiative
        {
            Debug.Log("Attacker's initiative is lower thatn the defender's.");
            //Preemptive strike from  defender's higher initiative

            if(cacheDefender.GetStats().attack > cacheAttacker.GetStats().defence)
            {
                SwordAction preemptiveAttackAction = cacheDefender.GetAction<SwordAction>();
                preemptiveAttackAction.TakeActionOnSmallHex(cacheAttacker.GetSmallHex(), cacheClearBusy);
            }
            else //Preemtive attack didn't hit the attacker 
            {
                Debug.Log("You've defended, and are now contrattacking.");

                cacheSwordAction.TakeActionOnSmallHex(cacheDefender.GetSmallHex(), cacheClearBusy);
            }
        }
    }

    public void SetFriendlyDiceResults(int result)
    {
        friendlyDiceResult = result;
    }

    public void SetEnemyDiceResults(int result)
    {
        enemyDiceResult = result;
    }

    // private bool CheckForRollWinner(int contestantOneScore, int contestantTwoScore)
    // {       
    //     if(contestantOneScore >= contestantTwoScore)
    //     {
    //         return contestantOneScore
    //     }
    // }
}

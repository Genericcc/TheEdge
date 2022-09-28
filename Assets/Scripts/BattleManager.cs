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

    private Unit cachedAttacker; 
    private Unit cachedDefender; 
    private SwordAction cachedSwordAction;
    private Action cachedClearBusy;

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

                SetState(State.DiceRolling, 4f);
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
        SquadCardSO friendlyStats = cachedAttacker.GetStats();
        SquadCardSO enemyStats = cachedDefender.GetStats();

        friendlyStats.attack += friendlyDiceResult;
        friendlyStats.defence += friendlyDiceResult;

        enemyStats.attack += enemyDiceResult;
        enemyStats.defence += enemyDiceResult;
    }

    private void ClearStats()
    {
        SquadCardSO modifiedFriendlyStats = cachedAttacker.GetStats();
        SquadCardSO modifiedEnemyStats = cachedDefender.GetStats();

        modifiedFriendlyStats.attack -= friendlyDiceResult;
        modifiedFriendlyStats.defence -= friendlyDiceResult;

        modifiedEnemyStats.attack -= enemyDiceResult;
        modifiedEnemyStats.defence -= enemyDiceResult;
    }

    public void BattleSetup(Unit attacker, Unit defender, SwordAction swordAction, Action ClearBusy)
    {
        SetState(State.BeforeDiceRoll, .5f);
        isBattleStarted = true;
        OnDiceRollStarted?.Invoke(this, EventArgs.Empty);

        cachedAttacker = attacker;
        cachedDefender = defender;
        cachedSwordAction = swordAction;
        cachedClearBusy = ClearBusy;
    }   

    private void Battle()
    {
        if(cachedAttacker.GetStats().initiative >= cachedDefender.GetStats().initiative)
        {
            if(cachedAttacker.GetStats().attack > cachedDefender.GetStats().defence)
            {
                cachedSwordAction.TakeActionOnSmallHex(cachedDefender.GetSmallHex(), cachedClearBusy);
            } 
        }
        else //Attacker has lower initiative
        {
            Debug.Log("Attacker's initiative is lower thatn the defender's.");
            
            //Preemptive strike from  defender's higher initiative
            if(cachedDefender.GetStats().attack > cachedAttacker.GetStats().defence)
            {
                SwordAction preemptiveAttackAction = cachedDefender.GetAction<SwordAction>();
                preemptiveAttackAction.TakeActionOnSmallHex(cachedAttacker.GetSmallHex(), cachedClearBusy);
            }
            else //Preemtive attack didn't hit the attacker 
            {

                Debug.Log("You've defended, and are now contrattacking.");

                cachedSwordAction.TakeActionOnSmallHex(cachedDefender.GetSmallHex(), cachedClearBusy);
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

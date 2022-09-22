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
    private int attackerDiceResult;
    private int defenderDiceResult;

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
                SetState(State.AfterDiceRoll, .5f);
                OnDiceRollFinished?.Invoke(this, EventArgs.Empty);
                break;
            case State.AfterDiceRoll:
                SetState(State.Battle, .1f);
                Battle();
                break;
            case State.Battle:
                isBattleStarted = false;
                break;
        }
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

    private void SetState(State newState, float newStateTime)
    {
        state = newState;
        stateTimer = newStateTime;
    }

    private void Battle()
    {
        if(cacheAttacker.GetStats().initiative >= cacheDefender.GetStats().initiative)
        {
            if(cacheAttacker.GetStats().attack + attackerDiceResult > cacheDefender.GetStats().defence + defenderDiceResult)
            {
                cacheSwordAction.TakeActionOnSmallHex(cacheDefender.GetSmallHex(), cacheClearBusy);
            } 
        }
        else //Attacker has lower initiative
        {
            Debug.Log("Attacker's initiative is lower thatn the defender's.");
            //Preemptive strike from  defender's higher initiative

            if(cacheDefender.GetStats().attack  + attackerDiceResult > cacheAttacker.GetStats().defence  + defenderDiceResult)
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

    public void SetDiceResults(int resultOne, int resultTwo)
    {
        attackerDiceResult = resultOne;
        defenderDiceResult = resultTwo;
    }

    // private bool CheckForRollWinner(int contestantOneScore, int contestantTwoScore)
    // {       
    //     if(contestantOneScore >= contestantTwoScore)
    //     {
    //         return contestantOneScore
    //     }
    // }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    private void Awake() 
    {
        if(Instance != null) 
        {
            Debug.LogError("There's more than one BattleManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Battle(Unit attacker, Unit defender, SwordAction swordAction, Action ClearBusy)
    {
        if(attacker.GetStats().initiative >= defender.GetStats().initiative)
        {
            //TODO: Roll(); and add rollNumber to attack and defence

            if(attacker.GetStats().attack > defender.GetStats().defence)
            {
                swordAction.TakeActionOnSmallHex(defender.GetSmallHex(), ClearBusy);
            }
            
        }
        else //Attacker has lower initiative
        {
            Debug.Log("Attacker's initiative is lower thatn the defender's.");
            //Preemptive strike from  defender's higher initiative

            if(defender.GetStats().attack > attacker.GetStats().defence)
            {
                SwordAction preemptiveAttackAction = defender.GetAction<SwordAction>();
                preemptiveAttackAction.TakeActionOnSmallHex(attacker.GetSmallHex(), ClearBusy);
            }
            else //Preemtive attack didn't hit the attacker 
            {
                Debug.Log("You've defended, and are now contrattacking.");

                swordAction.TakeActionOnSmallHex(defender.GetSmallHex(), ClearBusy);
            }



        }


    }   

    // private bool CheckForRollWinner(int contestantOneScore, int contestantTwoScore)
    // {       
    //     if(contestantOneScore >= contestantTwoScore)
    //     {
    //         return contestantOneScore
    //     }
    // }
}

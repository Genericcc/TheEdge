using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZone : MonoBehaviour
{
    Vector3 diceVelocity;
    Dice dice;
    DiceNumberText diceNumberText;

	private void Start() 
    {
       diceNumberText = FindObjectOfType<DiceNumberText>(); 
       dice = FindObjectOfType<Dice>();
    }

	void Update () 
    {
		diceVelocity = dice.GetDiceVelocity();
	}

	void OnTriggerStay(Collider col)
	{
		if (diceVelocity.x < 0.1f && diceVelocity.y < 0.1f && diceVelocity.z < 0.1f)
		{
            int rolledNumber = 0;
            //Debug.Log(col.gameObject.name);
			switch (col.gameObject.name) 
            {
                case "Side1":
                    rolledNumber = 6;
                    break;
                case "Side2":
                    rolledNumber = 5;
                    break;
                case "Side3":
                    rolledNumber = 4;
                    break;
                case "Side4":
                    rolledNumber = 3;
                    break;
                case "Side5":
                    rolledNumber = 2;
                    break;
                case "Side6":
                    rolledNumber = 1;
                    break;
			}

            diceNumberText.SetDiceNumber(rolledNumber);

            BattleManager.Instance.SetDiceResults(rolledNumber, 2);
		}
	}
}

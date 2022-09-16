using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZone : MonoBehaviour
{
    Vector3 diceVelocity;

	// Update is called once per frame
	void Update () 
    {
		diceVelocity = Dice.diceVelocity;
	}

	void OnTriggerStay(Collider col)
	{
		if (diceVelocity.x < 0.1f && diceVelocity.y < 0.1f && diceVelocity.z < 0.1f)
		{
            //Debug.Log(col.gameObject.name);
			switch (col.gameObject.name) 
            {
                case "Side1":
                    DiceNumberText.diceNumber = 6;
                    break;
                case "Side2":
                    DiceNumberText.diceNumber = 5;
                    break;
                case "Side3":
                    DiceNumberText.diceNumber = 4;
                    break;
                case "Side4":
                    DiceNumberText.diceNumber = 3;
                    break;
                case "Side5":
                    DiceNumberText.diceNumber = 2;
                    break;
                case "Side6":
                    DiceNumberText.diceNumber = 1;
                    break;
			}
		}
	}
}

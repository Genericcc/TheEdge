using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DiceNumberText : MonoBehaviour
{
    TextMeshProUGUI text;
	private int diceNumber;

	void Start () 
    {
		text = GetComponent<TextMeshProUGUI> ();
		SetDiceNumber(0);
	}
	
	void Update () 
    {
		text.text = diceNumber.ToString ();
	}

	public void SetDiceNumber(int number)
	{
		diceNumber = number;
	}

	public int GetDiceNumber() => diceNumber;

    
}

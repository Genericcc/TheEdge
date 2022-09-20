using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DiceNumberText : MonoBehaviour
{
    TextMeshProUGUI text;
	public static int diceNumber;

	void Start () 
    {
		text = GetComponent<TextMeshProUGUI> ();
	}
	
	void Update () 
    {
		text.text = diceNumber.ToString ();
	}

    
}

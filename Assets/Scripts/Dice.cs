using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    static Rigidbody rb;
	private Vector3 diceVelocity;
	DiceNumberText diceNumberText;

	void Start () 
    {
		rb = GetComponent<Rigidbody> ();
		rb.gameObject.SetActive(false);

		BattleManager.OnDiceRoll += BattleManager_OnDiceRoll;
	}

    void Update () 
    {
		diceVelocity = rb.velocity;
	}

    private void BattleManager_OnDiceRoll(object sender, System.EventArgs e)
    {
		rb.gameObject.SetActive(true);
		float dirX = Random.Range (200, 500);
		float dirY = Random.Range (200, 500);
		float dirZ = Random.Range (200, 500);
		transform.position = new Vector3 (-15, 5, 25);
		transform.rotation = Random.rotation;
		rb.AddForce (transform.up * 500);
		//rb.AddTorque (dirX, dirY, dirZ);
    }

	public Vector3 GetDiceVelocity()
	{
		return diceVelocity;
	}
}

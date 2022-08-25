using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardDisplay : MonoBehaviour
{
    public Card card;

    public Text initiativeText;
    public Text attackText;
    public Text defenceText;
    public Text moveText;
    public Text shieldText;

    void Start()
    {
        initiativeText.text = card.initiative.ToString();
        attackText.text = card.attack.ToString();
        defenceText.text = card.defence.ToString();
        moveText.text = card.move.ToString();
        shieldText.text = card.shield.ToString();
    }

}

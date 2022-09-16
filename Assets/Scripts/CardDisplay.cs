using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardDisplay : MonoBehaviour
{
    public SquadCardSO card;

    public TextMeshProUGUI initiativeText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenceText;
    public TextMeshProUGUI moveText;
    public TextMeshProUGUI shieldText;

    void Start()
    {
        initiativeText.text = card.initiative.ToString();
        attackText.text = card.attack.ToString();
        defenceText.text = card.defence.ToString();
        moveText.text = card.move.ToString();
        shieldText.text = card.shields.ToString();
    }

}

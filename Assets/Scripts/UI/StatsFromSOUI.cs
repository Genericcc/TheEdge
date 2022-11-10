using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsFromSOUI : MonoBehaviour
{
    [SerializeField] private SquadCardSO squadCardSO;
    [SerializeField] private RectTransform attack;
    [SerializeField] private RectTransform defence;
    [SerializeField] private RectTransform initiative;
    [SerializeField] private RectTransform move;
    [SerializeField] private RectTransform shields;

    private void Start() 
    {
        initiative.GetComponentInChildren<TextMeshProUGUI>().text = squadCardSO.initiative.ToString();
        attack.GetComponentInChildren<TextMeshProUGUI>().text = squadCardSO.attack.ToString();
        defence.GetComponentInChildren<TextMeshProUGUI>().text = squadCardSO.defence.ToString();
        move.GetComponentInChildren<TextMeshProUGUI>().text = squadCardSO.move.ToString();
        shields.GetComponentInChildren<TextMeshProUGUI>().text = squadCardSO.shields.ToString();
    }
}

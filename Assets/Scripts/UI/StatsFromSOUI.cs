using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StatsFromSOUI : MonoBehaviour
{
    [SerializeField] private SquadCardSO squadCardSO;
    [SerializeField] private List<RectTransform> rectList;
    [SerializeField] private bool isInverted;

    private Transform cameraTransform;
    

    private void Start() 
    {
        cameraTransform = Camera.main.transform;

        BattleManager.OnDiceRollStarted += BattleManager_OnDiceRollStarted; 
        BattleManager.OnClearStats += BattleManager_OnClearStats; 

        UpdateDisplay();
    }

    private void LateUpdate() 
    {
        if(isInverted)
        {
            Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + dirToCamera * -1f);
        }
        else
        {
            transform.LookAt(cameraTransform);
        }
        
    }

    private void UpdateDisplay()
    {
        foreach(RectTransform element in rectList)
        {
            element.GetComponentInChildren<TextMeshProUGUI>().text = squadCardSO.GetDesiredStat(element.name).ToString();
        }
    }

    private void BattleManager_OnDiceRollStarted(object sender, EventArgs e)
    {
        UpdateDisplay();
    }

    private void BattleManager_OnClearStats(object sender, EventArgs e)
    {
        UpdateDisplay();
    }
}

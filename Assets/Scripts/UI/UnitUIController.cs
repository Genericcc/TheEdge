using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUIController : MonoBehaviour
{
    [SerializeField] private Canvas UnitStatsWorldUI;

    private void Start() 
    {
        UnitStatsWorldUI.gameObject.SetActive(false);
    }

    private void OnMouseEnter() 
    {
        UnitStatsWorldUI.gameObject.SetActive(true);
    }

    private void OnMouseExit() 
    {
        UnitStatsWorldUI.gameObject.SetActive(false);
    }

}

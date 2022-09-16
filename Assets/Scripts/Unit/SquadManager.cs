using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : MonoBehaviour
{
    [SerializeField] public SquadCardSO squadCard;
    //[SerializeField] Unit unit1;
    //[SerializeField] Unit unit2;

    int shields;

    private void Start() 
    {

    }

    private void SetShields()
    {
        shields = squadCard.shields;
    }

    public int GetShields() => shields;

    public void DamageShields()
    {
        //TODO: Animation, VFX

        shields--;
    }

    public SquadCardSO GetSquadCard() => squadCard;

}

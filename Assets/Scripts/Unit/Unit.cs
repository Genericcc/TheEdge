using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{   
    private const int ACTION_POINTS_MAX = 4;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] private bool isEnemy;

    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private BaseAction[] baseActionArray;

    [SerializeField] private int actionPoints = ACTION_POINTS_MAX;

    private void Awake() 
    {
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start() 
    {
        Hex hex = HexSelectionManager.Instance.GetHexBeneath(transform.position + Vector3.up * 1);
        
        if(hex != null)
        {
            gridPosition = hex.GetHexPositionnnnn();
        }
        else 
        {
            Debug.Log("Hex is null");
        }

        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update() 
    {
        Hex hex;

        if(HexSelectionManager.Instance.GetHexBeneath(transform.position + Vector3.up * 1) != null)
        {
            hex = HexSelectionManager.Instance.GetHexBeneath(transform.position + Vector3.up * 1);
        }
        else
        {   //Temporary safeguard against getting null Hex while passing between large hexes (there is a tiny gap)
            hex = HexSelectionManager.Instance.GetHexBeneath(transform.position + Vector3.up * 1 + Vector3.back * 2);
        }

        GridPosition newGridPosition = hex.GetHexPositionnnnn(); 

        if(newGridPosition != gridPosition)
        {
            //Unit moved Grid Position
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach(BaseAction baseAction in baseActionArray)
        {
            if(baseAction is T) 
            { 
                return (T)baseAction;
            }
        }
        return null;
    } 

    public GridPosition GetGridPosition() => gridPosition;

    public Vector3 GetWorldPosition() 
    {
        return transform.position;
    } 

    public BaseAction[] GetBaseActionArray() => baseActionArray;

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return (actionPoints >= baseAction.GetActionPointCost()); 
    }

    private void SpendActionPoints(int amountToSpend)
    {
        actionPoints -= amountToSpend;
        
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointCost());
            return true;
        } else
        {
            return false;
        }
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || 
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINTS_MAX;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);    
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        
        Destroy(gameObject);

        OnAnyUnitDead.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }

}

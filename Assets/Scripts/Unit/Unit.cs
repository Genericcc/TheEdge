using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{   
    private const int MOVE_ACTION_POINT = 1;
    private const int SWORD_ACTION_POINT = 1;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] private bool isEnemy;
    private SquadCardSO stats;

    private GridPosition gridPosition;
    private LargeHex largeHex;
    private Hex smallHex;

    private SquadManager squadManager;
    private HealthSystem healthSystem;
    private BaseAction[] baseActionArray;

    private int moveActionPoint = MOVE_ACTION_POINT;
    private int swordActionPoint = SWORD_ACTION_POINT;

    private void Awake() 
    {
        squadManager = GetComponentInParent<SquadManager>();
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start() 
    {
        largeHex = HexSelectionManager.Instance.GetLargeHexBeneath(transform.position);
        smallHex = HexSelectionManager.Instance.GetSmallHexBeneath(transform.position);
        stats = squadManager.GetSquadCard();
        
        if(largeHex != null)
        {
            gridPosition = largeHex.GetHexPosition();
        }
        else 
        {
            Debug.Log("Hex is null");
        }

        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        LevelGrid.Instance.AddUnitAtSmallHex(smallHex, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

        
    }

    private void Update() 
    {
        LargeHex newLargeHex = HexSelectionManager.Instance.GetLargeHexBeneath(transform.position) 
                            ?? HexSelectionManager.Instance.GetLargeHexBeneath(transform.position + Vector3.back * 2);
        
        GridPosition newGridPosition = newLargeHex.GetHexPosition(); 
        
        if(newGridPosition != gridPosition)
        {
            //Unit moved Grid Position
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }

        Hex newSmallHex =  HexSelectionManager.Instance.GetSmallHexBeneath(transform.position) 
                        ?? HexSelectionManager.Instance.GetSmallHexBeneath(transform.position + Vector3.back * 1);

        if(newSmallHex != smallHex)
        {
            //Unit moved Grid Position
            Hex oldSmallHex = smallHex;
            smallHex = newSmallHex;

            LevelGrid.Instance.UnitMovedSmallHex(this, oldSmallHex, newSmallHex);
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
    
    public LargeHex GetLargeHex() => largeHex;
    
    public Hex GetSmallHex() => smallHex;

    public Vector3 GetWorldPosition() 
    {
        return transform.position;
    } 

    public BaseAction[] GetBaseActionArray() => baseActionArray;

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(baseAction is MoveAction)
        {
            return (moveActionPoint >= baseAction.GetActionPointCost());
        }

        if (baseAction is SwordAction)
        {
            return (swordActionPoint >= baseAction.GetActionPointCost());
        }
         
        return false;
    }

    private void SpendMoveActionPoint(int amountToSpend)
    {
        moveActionPoint -= amountToSpend;
        
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SpendSwordActionPoint(int amountToSpend)
    {
        swordActionPoint -= amountToSpend;
        
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(CanSpendActionPointsToTakeAction(baseAction))
        {
            if(baseAction is MoveAction)
            {
                SpendMoveActionPoint(baseAction.GetActionPointCost());
            }

            if (baseAction is SwordAction)
            {
                SpendSwordActionPoint(baseAction.GetActionPointCost());
            }

            return true;
        } 
        else
        {
            return false;
        }
    }

    public int GetMoveActionPoints()
    {
        return moveActionPoint;
    }

    public int GetSwordActionPoints()
    {
        return swordActionPoint;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || 
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            moveActionPoint = MOVE_ACTION_POINT;
            swordActionPoint = SWORD_ACTION_POINT;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage()
    {
        healthSystem.Damage();    
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

    public SquadCardSO GetStats() => stats;

}

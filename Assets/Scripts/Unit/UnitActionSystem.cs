using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;
   
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask; 

    private BaseAction selectedAction;
    private bool isBusy;

    private void Awake() 
    {
        if(Instance != null) 
        {
            Debug.LogError("There's more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() 
    {
        SetSelectedUnit(selectedUnit);
        
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Update() 
    {
        if(isBusy)
        {
            return;
        }

        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        if(EventSystem.current.IsPointerOverGameObject()) 
        {
            return;
        }
      
        if(TryHandleUnitSelection()) 
        {
            return;
        }

        HandleSelectedAction();
    }

    private bool TryHandleUnitSelection() 
    {
        if(InputManager.Instance.IsMouseButtonDown()) 
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if(raycastHit.transform.TryGetComponent<Unit>(out Unit unit)) 
                {
                    if(unit == selectedUnit)
                    {
                        //Unit already selected;
                        return false;
                    }

                    if(unit.IsEnemy())
                    {
                        //clicked on an Enemy
                        return false;
                    }

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }   
        return false;
    }

    private void HandleSelectedAction()
    {
        if(InputManager.Instance.IsMouseButtonDown())
        {
            LargeHex hex = HexSelectionManager.Instance.GetSelectedHex();
            Hex targetSmallHex = HexSelectionManager.Instance.GetSelectedSmallHex();

            if(hex == null)
            {
                return;
            }

            GridPosition mouseGridPosition = hex.GetHexPosition();
            //LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if(selectedAction is MoveAction)
            {
                if(!selectedAction.IsValidActionGridPosition(mouseGridPosition))
                {
                    //Position not in the ValidGridPositionList
                    return;
                }
    
                if(!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
                {
                    //Debug.Log("Not enough action points");
                    return;
                }

                SetBusy();

                selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            }

            if(selectedAction is SwordAction)
            {
            
                SwordAction swordAction = selectedAction as SwordAction; //This is random 

                if(!swordAction.IsValidActionSmallHex(targetSmallHex))
                {
                    //Position not in the ValidGridPositionList
                    return;
                }

                if(!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
                {
                    //Debug.Log("Not enough action points");
                    return;
                }
                
                SetBusy();

                BattleManager.Instance.BattleSetup(selectedUnit, targetSmallHex.GetUnit(), swordAction, ClearBusy);
            }            

            OnActionStarted?.Invoke(this, EventArgs.Empty); //UI visual, ActionPointUI update
        }
    }

    private void SetBusy() 
    {
        isBusy = true;

        OnBusyChanged?.Invoke(this, isBusy);
    }
    
    private void ClearBusy() 
    {
        isBusy = false;

        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void SetSelectedUnit(Unit unit) 
    {
        selectedUnit = unit;

        SetSelectedAction(unit.GetAction<MoveAction>());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit() 
    {
        return selectedUnit;
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
    
    //Automatic selection of a different Unit if the one previously selected dies
    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit deadUnit = sender as Unit; 

        if(!deadUnit.IsEnemy())
        {
            List<Unit> friendlyUnitList = UnitManager.Instance.GetFriendlyUnitList();

            if(friendlyUnitList[0] != null)
            {
                SetSelectedUnit(friendlyUnitList[0]);
                Debug.Log($"Selected unit is: {selectedUnit}");

            }
        }
        
    }

}

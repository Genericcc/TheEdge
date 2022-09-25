using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    [SerializeField] private GameObject visualSingleSmallHexGameObject;

    private List<Unit> unitList;

    private void Awake() 
    {
        unitList = new List<Unit>();

        Hide();
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList() => unitList;

    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }

    public bool HasEnemyUnit()
    {
        bool hasEnemy = false;

        if(HasAnyUnit())
        {
            if(unitList[0].IsEnemy())
            {
                hasEnemy = true;
            }
        }

        return hasEnemy;
    }

    public Unit GetUnit()
    {
        if(HasAnyUnit())
        {
            return unitList[0];
        } else
        {
            return null;
        }
    }

    public void Show()
    {
        visualSingleSmallHexGameObject.SetActive(true);
    }

    public void Hide()
    {
        visualSingleSmallHexGameObject.SetActive(false);
    }

    public GameObject GetVisual() => visualSingleSmallHexGameObject;
}

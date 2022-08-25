using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexSelectionManager : MonoBehaviour
{
    public static HexSelectionManager Instance { get; private set; }

    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask selectionMask;

    Hex selectedHex;

    private void Awake() 
    {
        if(Instance != null) 
        {
            Debug.LogError("There's more than one HexSelectionManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() 
    {
        InputManager.Instance.OnMouseClicked += InputManager_OnMouseClicked;
    }

    private void InputManager_OnMouseClicked(object sender, Vector3 mousePosition)
    {
        //After mouse click, it sets the clicked hex as the selectedHex
        SetSelectedHex(mousePosition);
    }

    private void SetSelectedHex(Vector3 mousePosition)
    {
        GameObject result;

        if(TryFindHexAt(mousePosition, out result))
        {
            if(result != null)
            {
                selectedHex = result.GetComponent<Hex>();
            }
            else
            {
                Debug.Log("Hex not set properly.");
            }
        }
    }

    private bool TryFindHexAt(Vector3 mousePosition, out GameObject result)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if(Physics.Raycast(ray, out hit, selectionMask))
        {
            result = hit.collider.gameObject;
            return true;
        }

        result = null;
        return false;
    }

    private bool TryFindHexBeneath(Vector3 raycastOrigin, out GameObject result)
    {
        RaycastHit hit;

        if(Physics.Raycast(raycastOrigin, Vector3.down, out hit, selectionMask))
        {
            result = hit.collider.gameObject;
            //Debug.Log("Hit result is: " + result.name);
            return true;
        }

        result = null;
        return false;
    }

    public Hex GetSelectedHex() => selectedHex;

    public Hex GetHexBeneath(Vector3 worldPosition)
    {
        GameObject result;
        
        if(TryFindHexBeneath(worldPosition, out result))
        {
            Hex hexBeneath = result.GetComponent<Hex>();
            return hexBeneath;
        }
        else 
        {
            Debug.Log("Hex not selected properly");
            return null;
        }
    }

    
    
}

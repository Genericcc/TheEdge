using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexSelectionManager : MonoBehaviour
{
    public static HexSelectionManager Instance { get; private set; }

    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask selectionMaskLargeHex;
    [SerializeField] LayerMask selectionMaskSmallHex;

    LargeHex selectedHex;
    Hex selectedSmallHex;

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
        GameObject resultLargeHex;
        GameObject resultSmallHex;

        if(TryFindHexAt(mousePosition, selectionMaskLargeHex, out resultLargeHex))
        {
            if(resultLargeHex != null)
            {
                selectedHex = resultLargeHex.GetComponent<LargeHex>();
            }
            else
            {
                Debug.Log("Large Hex not set properly.");
            }
        }
        
        if(TryFindHexAt(mousePosition, selectionMaskSmallHex, out resultSmallHex))
        {
            if(resultSmallHex != null)
            {
                selectedSmallHex = resultSmallHex.transform.parent.GetComponent<Hex>();
            }
            else
            {
                Debug.Log("Small Hex not set properly.");
            }
        }
    }

    private bool TryFindHexAt(Vector3 mousePosition, LayerMask selectionMask, out GameObject result)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, selectionMask))
        {
            result = hit.collider.gameObject;
            return true;
        }

        result = null;
        return false;
    }

    private bool TryFindHexBeneath(Vector3 raycastOrigin, LayerMask selectionMask, out GameObject result)
    {
        RaycastHit hit;

        if(Physics.Raycast(raycastOrigin + Vector3.up * 1, Vector3.down, out hit, Mathf.Infinity, selectionMask))
        {
            result = hit.collider.gameObject;
            //Debug.Log("Hit result is: " + result.name);
            return true;
        }
       
        result = null;
        return false;
    }

    public LargeHex GetSelectedHex() => selectedHex;
    
    public Hex GetSelectedSmallHex() => selectedSmallHex;

    public LargeHex GetLargeHexBeneath(Vector3 worldPosition)
    {
        GameObject result;
        
        if(TryFindHexBeneath(worldPosition, selectionMaskLargeHex, out result))
        {
            LargeHex hexBeneath = result.GetComponent<LargeHex>();
            return hexBeneath;
        }
        else 
        {
            //Debug.Log("Hex not selected properly");
            return null;
        }
    }

    public Hex GetSmallHexBeneath(Vector3 worldPosition)
    {
        GameObject result;
        
        if(TryFindHexBeneath(worldPosition, selectionMaskSmallHex, out result))
        {
            Hex hexBeneath = result.transform.parent.GetComponent<Hex>();
            return hexBeneath;
        }
        else 
        {
            //Debug.Log("Hex not selected properly");
            return null;
        }
    }

    public void RemoteSetSelectedSmallHex(Hex newSelectedSmallHex)
    {
        selectedSmallHex = newSelectedSmallHex;
    }

    
    
}

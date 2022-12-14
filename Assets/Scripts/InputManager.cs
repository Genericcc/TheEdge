using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event EventHandler<Vector3> OnMouseClicked;

    private void Awake() 
    {
        if(Instance != null) 
        {
            Debug.LogError("There's more than one InputManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Vector2 GetMouseScreenPosition()
    {
        //OnMouseClicked?.Invoke(this, Input.mousePosition);

        return Input.mousePosition;
    }

    public bool IsMouseButtonDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnMouseClicked?.Invoke(this, Input.mousePosition);

            return true;
        }
        
        return false;
    }

    public Vector2 GetCameraMoveVector()
    {
        Vector2 inputMoveDir = new Vector2(0, 0);

        if(Input.GetKey(KeyCode.W))
        {
            inputMoveDir.y = +1f;
        }   
        if(Input.GetKey(KeyCode.S))
        {
            inputMoveDir.y = -1f;
        } 
        if(Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        } 
        if(Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }  

        return inputMoveDir;
    }

    public float GetCameraRotateAmount()
    {
        float rotateAmount = 0f;

        if(Input.GetKey(KeyCode.Q))
        {
            rotateAmount = +1f;
        }

        if(Input.GetKey(KeyCode.E))
        {
            rotateAmount = -1f;
        }

        return rotateAmount;
    }

    public float GetCameraZoomAnout()
    {
        float zoomAmount = 0f;

        if(Input.mouseScrollDelta.y > 0)
        {
            zoomAmount = -1f;
        }
        if(Input.mouseScrollDelta.y < 0)
        {
            zoomAmount = +1f;
        }

        return zoomAmount;
    }
}

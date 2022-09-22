using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;
    [SerializeField] private Transform diceTray;

    private void Start() 
    {
        BattleManager.OnDiceRollStarted += BattleManager_OnDiceRollStarted;
        BattleManager.OnDiceRollFinished += BattleManager_OnDiceRollFinished;

        HideActionCamera();
    }

    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }
    
    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }    
    
    private void BattleManager_OnDiceRollStarted(object sender, EventArgs e)
    {
        Vector3 diceTrayOffset = Vector3.up * 10f;
        Vector3 actionCameraPosition = diceTray.position + diceTrayOffset;
          
        actionCameraGameObject.transform.position = actionCameraPosition;
        actionCameraGameObject.transform.LookAt(diceTray);
        ShowActionCamera();
        
    }
    
    private void BattleManager_OnDiceRollFinished(object sender, EventArgs e)
    {
        HideActionCamera();
    }

}

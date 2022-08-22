using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [SerializeField] private LayerMask mousePlaneLayerMask;
    [SerializeField] private Transform sphereThatFollows;
    
    private static MouseWorld instance;
    
    private void Awake() 
    {
        instance = this;
    }


    private void Update() 
    {
        // float moveSpeed = .5f;
        //sphereThatFollows.transform.position = Vector3.Lerp(
        //    sphereThatFollows.transform.position, GetPosition(), moveSpeed * Time.deltaTime);
    }

    public static Vector3 GetPosition() 
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask);

        return raycastHit.point;
    }
}
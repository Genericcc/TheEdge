using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    [SerializeField] private GameObject visualGameObject;

    Unit occupingUnit;

    public GameObject GetVisual() => visualGameObject;

 

}

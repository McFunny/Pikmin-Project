using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public CarryObject carryObject;
    public string pelletColor;
   
    
    void Awake()
    {
        //carryObject = GetComponent<CarryObject>();
    }

   
    void Update()
    {
        if (carryObject.seedColor == pelletColor)
        {
            carryObject.seedWorth = 2;
        }
        else
        {
            carryObject.seedWorth = 1;
        }
    }
}

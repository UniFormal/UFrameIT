﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFacts : MonoBehaviour
{
    public InventoryObject inventory;

    public GameObject prefab;

    public int x_Start;
    public int y_Start;
    public int X_Pacece_Between_Items;
    public int y_Pacece_Between_Items;
    public int number_of_Column;
    // Start is called before the first frame update
    void Start()
    {
        //CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
       UpdateDisplay();
    }

    public void UpdateDisplay()
    {
         for( int i = 0; i< inventory.Facts.Count; i++){
            if(! inventory.Facts[i].isDisplayed){
                var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<Text>().text = inventory.Facts[i].item.Description;
                inventory.Facts[i].isDisplayed = true;
            }
            
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(x_Start+ (X_Pacece_Between_Items * (i % number_of_Column)), y_Start + (-y_Pacece_Between_Items * (i / number_of_Column)), 0f);
    }
}

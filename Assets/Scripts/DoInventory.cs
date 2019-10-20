using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class DoInventory : MonoBehaviour
{
    private CalculateInventory CalculateInventory;

    private GameObject InventoryCOntroller;
    //this dictionary is the player inventory

    //a list that stores UI location
    private Sprite currentSprite;
    
    
    
   

    // Start is called before the first frame update
    void Start()
    {
        InventoryCOntroller = GameObject.Find("---InventoryController");
        CalculateInventory = InventoryCOntroller.GetComponent<CalculateInventory>();


        currentSprite = GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        //print("items in inventory = " + CalculateInventory.inventory.Count);
    }

    public void AddClothToInventory()
    {
        //if the current button is labeled as top
        if (CompareTag("Top"))
        {
            CalculateInventory.top.image.sprite = currentSprite;
        }
        else if(CompareTag("Shoe"))
        {
            CalculateInventory.shoe.image.sprite = currentSprite;
        }
        else if(CompareTag("Other"))
        {
            CalculateInventory.other.image.sprite = currentSprite;
        }
        

    }
}

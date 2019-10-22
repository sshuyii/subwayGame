using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class DoInventory : MonoBehaviour
{
    private CalculateInventory CalculateInventory;

    private GameObject InventoryController;
    //this dictionary is the player inventory

    //a list that stores UI location
    public Sprite currentSprite;
    public Button selfButton;
    
    
    
   

    // Start is called before the first frame update
    void Start()
    {
        InventoryController = GameObject.Find("---InventoryController");
        CalculateInventory = InventoryController.GetComponent<CalculateInventory>();

//        selfButton.onClick.AddListener(AddClothToInventory);

    }

    // Update is called once per frame
    void Update()
    {
        //print("items in inventory = " + CalculateInventory.inventory.Count);



    }

    public void AddClothToInventory()
    {
        print("pressed");
        selfButton = GetComponent<Button>();
        currentSprite = selfButton.image.sprite;
        print("currentSpriteName = " + currentSprite.name);



        

        Sprite buttonSprite = currentSprite;
        print("SpriteName = " + currentSprite.name);

        

        //if the current button is labeled as top
        if (buttonSprite.name.Contains("Top")
        )
        {
            print("change to top");
            CalculateInventory.top.image.sprite = currentSprite;
        }
        else if(buttonSprite.name.Contains("shoe"))
        {
            print("change to shoe");

            CalculateInventory.shoe.image.sprite = currentSprite;
        }
        else if(buttonSprite.name.Contains("Bottom"))
        {
            print("change to bottom");

            CalculateInventory.other.image.sprite = currentSprite;
        }

        
        //selfButton.enabled = false;

    }
}

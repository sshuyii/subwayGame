using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class DoInventory : MonoBehaviour
{
    private CalculateInventory CalculateInventory;

    private FinalCameraController FinalCameraController;
    public WasherController WasherController;
    
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
        
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();


//        selfButton.onClick.AddListener(AddClothToInventory);

    }

    // Update is called once per frame
    void Update()
    {
        //print("items in inventory = " + CalculateInventory.inventory.Count);



    }

    public void AddClothToInventory()
    {
        
        
        //print("pressed");
        selfButton = GetComponent<Button>();
        currentSprite = selfButton.image.sprite;
        print("currentSpriteName = " + currentSprite.name);


        Sprite buttonSprite = currentSprite;
        print("SpriteName = " + currentSprite.name);


        print("occupiedI = " + CalculateInventory.occupiedI);
        
        //inventory used to be buttons
        CalculateInventory.inventory[CalculateInventory.occupiedI].GetComponent<Image>().sprite = buttonSprite;
        CalculateInventory.inventory[CalculateInventory.occupiedI].tag = this.tag;
        
        if(CalculateInventory.occupiedI < 6)
        {
            CalculateInventory.occupiedI++;

        }
        else
        {
            return;
            CalculateInventory.occupiedI = 1;
            
        }
        
        //for tutorial
        if (FinalCameraController.isTutorial)
        {
            FinalCameraController.TutorialManager.tutorialNumber = 10;
            //in tutorial, if click a cloth, cloth the entire ui interface
            WasherController.clickMachine();

        }

        
        //CalculateInventory.inventory[CalculateInventory.occupiedI].GetComponent<SpriteRenderer>().sprite = buttonSprite;
        

        
        
/*three fixed inventory: top shoe bottom
//        //if the current button is labeled as top
//        if (buttonSprite.name.Contains("Top")
//        )
//        {
////            print("change to top");
//            CalculateInventory.top.image.sprite = currentSprite;
//        }
//        else if(buttonSprite.name.Contains("shoe"))
//        {
////            print("change to shoe");
//
//            CalculateInventory.shoe.image.sprite = currentSprite;
//        }
//        else if(buttonSprite.name.Contains("Bottom"))
//        {
////            print("change to bottom");
//
//            CalculateInventory.other.image.sprite = currentSprite;
//        }
*/

        //image disappear
        selfButton.image.enabled = false;
        //selfButton.enabled = false;

        //selfButton.enabled = false;

    }
}

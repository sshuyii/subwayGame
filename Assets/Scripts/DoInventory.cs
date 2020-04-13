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

    }

    public void AddClothToInventory()
    {
        
        //print("pressed");
        //get the machine this cloth belongs to
        //if all clothes in the machine are taken, close door
        GetComponentInParent<WasherController>().clothNum--;    
        
        
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
        }
        
        //for tutorial
        if (FinalCameraController.isTutorial && FinalCameraController.isSwipping == false)//打开了洗衣机的门，真的拿了衣服
        {

//            if (FinalCameraController.TutorialManager.tutorialNumber == 9)//拿了第一件衣服
//            {
//                FinalCameraController.TutorialManager.tutorialNumber = 12;
//            }
            if(FinalCameraController.TutorialManager.tutorialNumber == 9)//拿了第二件衣服
            {
                FinalCameraController.TutorialManager.tutorialNumber = 12;
                //in tutorial, if click a cloth, cloth the entire ui interface
                WasherController.clickMachine();
                StartCoroutine(FinalCameraController.TutorialManager.AnimateText(
                    FinalCameraController.TutorialManager.kararaText, "put on",
                    true, FinalCameraController.TutorialManager.closet, new Vector2(-81, 37)));
                FinalCameraController.TutorialManager.tutorialDialogueState = TutorialManager.DialogueState.karara;
                FinalCameraController.clickKarara();
                FinalCameraController.Hide(FinalCameraController.TutorialManager.arrowButton);
            }
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

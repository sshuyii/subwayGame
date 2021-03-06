﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
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

        if (FinalCameraController.isSwipping == false)
        {            
            WasherController = GetComponentInParent<WasherController>();

            if (CalculateInventory.occupiedI < 6)
            {
                //print("pressed");
                //get the machine this cloth belongs to
                //if all clothes in the machine are taken, close door
                WasherController.clothNum--; //洗衣机里的衣服少一

                //如果洗衣机里没衣服了，那么直接关上
                if (WasherController.clothNum == 0)
                {
                    print("should close machine door");
                    StartCoroutine(WasherController.MachineFold());
                    WasherController.DoorImage.sprite = WasherController.AllMachines.closedDoor;
                }



                selfButton = GetComponent<Button>();
                currentSprite = selfButton.image.sprite;
                print("currentSpriteName = " + currentSprite.name);

                Sprite buttonSprite = currentSprite;
                print("SpriteName = " + currentSprite.name);

                print("occupiedI = " + CalculateInventory.occupiedI);

                int firstEmptyInventory = new int();
                //这里应该找到第一个空着的inventory位置
                for (int i = 0; i < CalculateInventory.inventory.Count; i++)
                {
                    if (CalculateInventory.inventory[i].CompareTag("Untagged"))
                    {
                        firstEmptyInventory = i;
                        break;
                    }

                }

                //inventory used to be buttons
                CalculateInventory.inventory[firstEmptyInventory].GetComponent<Image>().sprite = buttonSprite;
                CalculateInventory.inventory[firstEmptyInventory].tag = this.tag;
                CalculateInventory.occupiedI++;



                //for tutorial
                if (FinalCameraController.isTutorial) //打开了洗衣机的门，真的拿了衣服
                {
                    if (FinalCameraController.TutorialManager.tutorialNumber == 9 ||
                        FinalCameraController.TutorialManager.tutorialNumber == 11) //拿了第二件衣服
                    {
                        FinalCameraController.TutorialManager.tutorialNumber = 12;
                        //in tutorial, if click a cloth, cloth the entire ui interface
                        WasherController.clickMachine();
                        StartCoroutine(FinalCameraController.TutorialManager.AnimateText(
                            FinalCameraController.TutorialManager.kararaText, "put on",
                            true, FinalCameraController.TutorialManager.closet, new Vector2(-81, 37)));
                        FinalCameraController.TutorialManager.tutorialDialogueState =
                            TutorialManager.DialogueState.karara;
//                        FinalCameraController.clickKarara();
                        FinalCameraController.Hide(FinalCameraController.TutorialManager.arrowButton);
                    }
                }

                //image disappear
                selfButton.image.enabled = false;
                //selfButton.enabled = false;
            }

            else if (CalculateInventory.occupiedI > 5 && CalculateInventory.fulltemp == false)
            {
                FinalCameraController.Hide(WasherController.ClothUI);
                FinalCameraController.Show(CalculateInventory.InventoryFull);
                CalculateInventory.fulltemp = true;

                //close all machine doors
                WasherController.DoorImage.sprite = WasherController.AllMachines.closedDoor;
                StartCoroutine(WasherController.MachineFold());

            }
        }


    }
}

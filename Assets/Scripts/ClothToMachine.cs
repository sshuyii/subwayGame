using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.UI.Extensions;

public class ClothToMachine : MonoBehaviour
{
    private HorizontalScrollSnap myHSS;
    private GameObject ClothInMachineController;

    private AllMachines AllMachines;
    private SubwayMovement SubwayMovement;


    private List<WasherController> WasherControllerList;

    private WasherController WasherController;

    private FinalCameraController FinalCameraController;
    private CalculateInventory CalculateInventory;


    private bool alreadyWashed;
    private bool isOverdue;
    private int hitTime;

    private Image myImage;

    public int myBagPosition;

    
    //a timer to record how much time has passed since the bag is on the car
    private float bagTimer = 0f;
    public bool isReady;

    // Start is called before the first frame update
    void Start()
    {
        //find the horizontal scroll snap script
        myHSS = GameObject.Find("Horizontal Scroll Snap").GetComponent<HorizontalScrollSnap>();


        myImage = GetComponent<Image>();
        hitTime = 0;
        
        ClothInMachineController = GameObject.Find("---ClothInMachineController");
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();
        CalculateInventory = GameObject.Find("---InventoryController").GetComponent<CalculateInventory>();
        SubwayMovement = GameObject.Find("---StationController").GetComponent<SubwayMovement>();
        

        AllMachines = ClothInMachineController.GetComponent<AllMachines>();
        WasherControllerList = new List<WasherController>();
        
        //get every machine's script
        for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
        {
            WasherControllerList.Add(AllMachines.WashingMachines[i].GetComponent<WasherController>());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!FinalCameraController.isTutorial)
        {
            //如果还没转够整整一圈，enable timer
            if (bagTimer < 3 * SubwayMovement.moveTime + 4 * SubwayMovement.stayTime)
            {
                bagTimer += Time.deltaTime;
            }
            else //if the train arrives the station for the second time
            {
                //应该再检测一下是不是在离开站台的瞬间
                //检测是否有一个finish的洗衣机，里面装着这个tag的衣服
                if (alreadyWashed) //if this bag is already washed
                {
                    AllMachines.currentBag = this.gameObject;
                    returnClothYes();

                    //enable fish comic image
                    FinalCameraController.lateReturnComic = true;
                }
                else if (!isOverdue) //没有洗好的衣服不要还
                {
                    //instantiate an overdue label
                    GameObject overdue = Instantiate(AllMachines.Overdue, this.gameObject.transform.position,
                        Quaternion.identity);
                    overdue.transform.SetParent(this.gameObject.transform);
                    isOverdue = true;
                }
            }
        }
    }

   
    
    private void ChangeToUnderwear()
    {
         
        CalculateInventory.blackPants.enabled = true;
        CalculateInventory.whiteShirt.enabled = true;

        CalculateInventory.whiteShirtS.enabled = true;
        CalculateInventory.blackPantsS.enabled = true;

        CalculateInventory.whiteShirtASR.sprite = CalculateInventory.allAdCloth["WhiteShirt"];
        CalculateInventory.blackPantsASR.sprite = CalculateInventory.allAdCloth["BlackPants"];
    }
    

    public void returnClothYes()
    {
        for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
        {
            if (WasherControllerList[i].CompareTag(AllMachines.currentBag.tag))
            {
                print("AllMachines.currentBag.tag = " + AllMachines.currentBag.tag);

                //对所有inventory中带着这个洗衣机tag的衣服，把它们放回洗衣机
                for (int u = 0; u < CalculateInventory.inventory.Count; u++)
                {
                    if (CalculateInventory.inventory[u].CompareTag(AllMachines.currentBag.transform.gameObject.tag))
                    {
                        //change the inventory button image back to start
                        CalculateInventory.inventory[u].GetComponent<Image>().sprite = FinalCameraController.startSprite;
                        
                        print("returnrnrnrnrnrnrn");
                        //then 
                        CalculateInventory.occupiedI = CalculateInventory.occupiedI - 1;
                    }
                }
                //todo: can be simpler
                //如果karara身上正好穿着这件衣服，那么在地铁、换装、广告界面都要脱下来
                if (CalculateInventory.everythingSR.CompareTag(AllMachines.currentBag.transform.gameObject.tag))
                {
                    CalculateInventory.everythingSR.sprite = CalculateInventory.transparent;
                    CalculateInventory.everythingASR.sprite = CalculateInventory.transparent;
                    CalculateInventory.everythingSSR.sprite = CalculateInventory.transparent;
                    
                    ChangeToUnderwear();
                }
                if (CalculateInventory.topSR.CompareTag(AllMachines.currentBag.transform.gameObject.tag))
                {
                    CalculateInventory.topSR.sprite = CalculateInventory.transparent;
                    CalculateInventory.topASR.sprite = CalculateInventory.transparent;
                    CalculateInventory.topSSR.sprite = CalculateInventory.transparent;
                    
                    ChangeToUnderwear();
                }
                if (CalculateInventory.otherSR.CompareTag(AllMachines.currentBag.transform.gameObject.tag))
                {
                    CalculateInventory.otherSR.sprite = CalculateInventory.transparent;
                    CalculateInventory.otherASR.sprite = CalculateInventory.transparent;
                    CalculateInventory.otherSSR.sprite = CalculateInventory.transparent;
                    
                    ChangeToUnderwear();
                }
                if (CalculateInventory.shoeSR.CompareTag(AllMachines.currentBag.transform.gameObject.tag))
                {
                    CalculateInventory.shoeSR.sprite = CalculateInventory.transparent;
                    CalculateInventory.shoeASR.sprite = CalculateInventory.transparent;
                    CalculateInventory.shoeSSR.sprite = CalculateInventory.transparent;
                    
                    ChangeToUnderwear();
                }
                
                
                Destroy(AllMachines.currentBag);

                //reset the machine's variables
                WasherControllerList[i].transform.tag ="Untagged";
                WasherControllerList[i].myMachineState = AllMachines.MachineState.empty;
                Destroy(FinalCameraController.generatedNotice);
                
                //reset hitTime so the machine contain different bag of clothes next time
                hitTime = 0;
                WasherControllerList[i].isFirstOpen = true;

                if (FinalCameraController.isTutorial && FinalCameraController.TutorialManager.tutorialNumber == 16)
                {
                    FinalCameraController.TutorialManager.tutorialNumber = 17;
                }
                break;
            }
        }
    }


    public void returnClothNo()
    {
        
        Destroy(FinalCameraController.generatedNotice);
        FinalCameraController.alreadyNotice = false;

    }


 
    public void putClothIn()
    {
        if(FinalCameraController.isSwipping == false)
        {
            if (hitTime == 0)
            {
                SubwayMovement.bagNum -= 1;
                for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
                {
                    //disable karara image in tutorial
                    if (FinalCameraController.isTutorial)
                    {
                        FinalCameraController.TutorialManager.KararaStandingImage.enabled = false;
                        FinalCameraController.TutorialManager.tutorialNumber = 3;
                        FinalCameraController.TutorialManager.scrollControl(true);
                    }
                    
                    //get machine start washing
                    if (WasherControllerList[i].myMachineState == AllMachines.MachineState.empty)
                    {
                        WasherControllerList[i].myMachineState = AllMachines.MachineState.bagUnder;
                        //change machine tags to character
                        WasherControllerList[i].transform.gameObject.tag = this.transform.gameObject.tag;

                        this.gameObject.transform.SetParent(WasherControllerList[i].gameObject.transform);

                        transform.position =
                            AllMachines.WashingMachines[i].transform.position + new Vector3(0, -2.9f, 0);
                        if (i == 0)
                        {
                            myHSS.GetComponent<HorizontalScrollSnap>().GoToScreen(2);
                        }
                        else
                        {
                            myHSS.GetComponent<HorizontalScrollSnap>().GoToScreen(3);
                        }
                        
                        //change the bag position to be empty again
                        SubwayMovement.bagPosAvailable[myBagPosition] = false;

                        hitTime++;
                        break;
                    }
                }
            }
            
            else if (hitTime == 1)
            {
                //点第二次换成打开的包
                print("tag = " + tag);
                myImage.sprite = AllMachines.openBagsDic[this.tag];
                alreadyWashed = true;
                
                //in tutorial
                if (FinalCameraController.isTutorial)
                {
                    FinalCameraController.TutorialManager.tutorialNumber = 4;
                    FinalCameraController.TutorialManager.bag.GetComponent<Image>().material.DisableKeyword("SHAKEUV_ON");
                }

                for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
                {
                    //get machine start washing
                    if (WasherControllerList[i].myMachineState == AllMachines.MachineState.bagUnder)
                    {
                        if(WasherControllerList[i].transform.CompareTag(this.transform.gameObject.tag))
                        {
                            WasherControllerList[i].myMachineState = AllMachines.MachineState.full;
//                        //change machine tags to character
//                        WasherControllerList[i].transform.gameObject.tag = this.transform.gameObject.tag;

                            hitTime++;
                            break;
                        }
                    }
                }
                
            }
            //return clothes
            else if (hitTime > 1)
            {
//                else
//                {
                    //get the machine with clothes from this bag
                    for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
                    {
                        if (WasherControllerList[i].CompareTag(this.transform.gameObject.tag))
                        {
                            if (WasherControllerList[i].myMachineState == AllMachines.MachineState.finished)
                            {
                                //WasherControllerList[i].myMachineState = AllMachines.MachineState.empty;

                                if (FinalCameraController.alreadyNotice == false)
                                {
                                    AllMachines.currentBag = this.gameObject;

                                    print("returnClothhhhhh");
                                    if (FinalCameraController.isTutorial)
                                    {
                                        if(FinalCameraController.TutorialManager.tutorialNumber == 16)
                                        {
                                            //then return all clothes in the machine
                                            returnClothYes();
                                        }
                                        else if (FinalCameraController.TutorialManager.tutorialNumber == 9)
                                        {
                                            FinalCameraController.TutorialManager.chooseBag = true;
                                            FinalCameraController.TutorialManager.clicktime = 7;
                                        }
                                    }
                                    else
                                    {
                                        //generate the notice
                                        FinalCameraController.generatedNotice = Instantiate(AllMachines.returnNotice,
                                            new Vector3(0, 0, 0),
                                            Quaternion.identity, WasherControllerList[i].gameObject.transform);
                                        if (i == 1)
                                        {
                                            FinalCameraController.generatedNotice.transform.SetParent(WasherControllerList[2].gameObject.transform);
                                        }
                                        else
                                        {
                                            FinalCameraController.generatedNotice.transform.SetParent(WasherControllerList[i].gameObject.transform);

                                        }
                                        FinalCameraController.alreadyNotice = true;

                                    }
                                }

                                //print("AllMachines.currentBag.tag = " + AllMachines.currentBag.tag);

                                hitTime++;
                                break;
                            }
                        }
                    }
//                }
            }
        }
        
    }
   
}

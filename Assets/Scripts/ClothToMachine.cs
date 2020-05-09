using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
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
    public int hitTime;//can be made private

    private Image myImage;

    public int myBagPosition;

    public bool isNotice;
    public Image secondImage;
    public bool isNoticePrefab;

    
    //a timer to record how much time has passed since the bag is on the car
    private float bagTimer = 0f;
    public bool isReady;

    // Start is called before the first frame update
    void Start()
    {
        //find the horizontal scroll snap script
        myHSS = GameObject.Find("Horizontal Scroll Snap").GetComponent<HorizontalScrollSnap>();


        myImage = GetComponent<Image>();
//        secondImage = GetComponentInChildren<Image>();
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

    private float timer;
    private float remainTime;

    // Update is called once per frame
    void Update()
    {
        if(!FinalCameraController.isTutorial && !isNoticePrefab)
        {
            secondImage.sprite = myImage.sprite;
        }       
        
        if(!isNotice)
        {
            timer += Time.deltaTime;
            remainTime = (SubwayMovement.moveTime + SubwayMovement.stayTime) * 3 - timer;


            if (!isOverdue)
            {
                //find the machine that has the same tag with this bag
                for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
                {
                    if (WasherControllerList[i].transform.CompareTag(this.transform.gameObject.tag) &&
                        WasherControllerList[i].myMachineState == AllMachines.MachineState.finished)
                    {
                        alreadyWashed = true;
                        break;
                    }
                }
            }

            if (!FinalCameraController.isTutorial && !timeUp)
            {
                bagTimer += Time.deltaTime;
                //如果还没转够整整一圈，enable timer
                if (bagTimer < 3 * SubwayMovement.moveTime + 3.5 * SubwayMovement.stayTime)
                {
                    timeUp = false;
                }
                else //if the train arrives the station for the second time
                {
                    if(!FinalCameraController.ChapterOneEnd)
                    {
                        timeUp = true;
                        //应该再检测一下是不是在离开站台的瞬间
                        //检测是否有一个finish的洗衣机，里面装着这个tag的衣服
                        if (alreadyWashed) //if this bag is already washed
                        {
                            //一次只能还一个包
                            //如果已经有包在还了，就直接还
                            if(!AllMachines.isReturning)
                            {
                                //AllMachines.currentBag = this.gameObject;
                                FinalCameraController.lateReturnComic = true;
                                print("lateReturncomic =" + FinalCameraController.lateReturnComic);
                                StartCoroutine(returnClothYes());
                            }
                            else
                            {
                                returnClothYesDirectly();
                            }
                            //enable fish comic image
                            //现在是，鱼老板直接帮忙把洗好又到站的衣服换了，没有其他惩罚，转到鱼老板界面，老板告诉karara没来得及还
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
        }
    }

    private bool timeUp;
   
    
    private void ChangeToUnderwear(string cloth)
    {
        if (cloth == "top")
        {
            CalculateInventory.whiteShirt.enabled = true;
            CalculateInventory.whiteShirtS.enabled = true;
            CalculateInventory.whiteShirtASR.sprite = CalculateInventory.allAdCloth["WhiteShirt"];
        }
        else if (cloth == "bottom")
        {
            CalculateInventory.blackPants.enabled = true;
            CalculateInventory.blackPantsS.enabled = true;
            CalculateInventory.blackPantsASR.sprite = CalculateInventory.allAdCloth["BlackPants"];
        }
        else if(cloth == "everything")
        {
            CalculateInventory.whiteShirt.enabled = true;
            CalculateInventory.whiteShirtS.enabled = true;
            CalculateInventory.whiteShirtASR.sprite = CalculateInventory.allAdCloth["WhiteShirt"];
            
            CalculateInventory.blackPants.enabled = true;
            CalculateInventory.blackPantsS.enabled = true;
            CalculateInventory.blackPantsASR.sprite = CalculateInventory.allAdCloth["BlackPants"];
        }
    }


    IEnumerator returnClothYes ()
    {
        AllMachines.isReturning = true;
        FinalCameraController.ChangeToSubway();


        //找到那个洗衣机
        for (int a = 0; a < AllMachines.WashingMachines.Count; a++)
        {
            if (WasherControllerList[a].CompareTag(tag))
            {
                FinalCameraController.returnMachineNum = a;
            }
        }

        int temp = 0;
        //转到这个洗衣机在的页面
        if (FinalCameraController.returnMachineNum == 0)
        {
            temp = 2;
        }
        else
        {
            temp = 3;
        }
        
        FinalCameraController.myHSS.GoToScreen(temp);
        
        yield return new WaitForSeconds(1f);
        print("bag disappear");

        //包消失
        
        FinalCameraController.AllStationClothList.Remove(tag);
        FinalCameraController.alreadyNotice = false;
        //去掉这个洗衣机上的tag

        if(!FinalCameraController.isTutorial)
        {
            AllMachines.nameToTemp[tag].Clear();
            //reset the temp list, so all the clothes are in the temp list
            for (int i = 0; i < AllMachines.nameToPermenant[tag].Count; i++)
            {
                AllMachines.nameToTemp[tag].Add(AllMachines.nameToPermenant[tag][i]);
            }
        }
        
        for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
        {
            if (WasherControllerList[i].CompareTag(tag))
            {
                FinalCameraController.returnMachineNum = i;

//                print("AllMachines.currentBag.tag = " + AllMachines.currentBag.tag);
                //对所有inventory中带着这个洗衣机tag的衣服，把它们放回洗衣机
                
                for (int u = 0; u < CalculateInventory.inventory.Count; u++)
                {
                    if (CalculateInventory.inventory[u].CompareTag(tag))
                    {
                        //change the inventory button image back to start
                        CalculateInventory.inventory[u].GetComponent<Image>().sprite = FinalCameraController.startSprite;
                        
                        print("returnrnrnrnrnrnrn");
                        //then 
                        CalculateInventory.occupiedI = CalculateInventory.occupiedI - 1;
                        //reset the tag of the inventory item
                        CalculateInventory.inventory[u].tag = "Untagged";
                    }
                }
                //todo: can be simpler
                //如果karara身上正好穿着这件衣服，那么在地铁、换装、广告界面都要脱下来
                if (CalculateInventory.everythingSR.CompareTag(this.tag))
                {
                    CalculateInventory.everythingSR.sprite = CalculateInventory.transparent;
                    CalculateInventory.everythingASR.sprite = CalculateInventory.transparent;
                    CalculateInventory.everythingSSR.sprite = CalculateInventory.transparent;
                    
                    CalculateInventory.wearingEverything = false;
                    //如果穿的是这件衣服的话
                    
                    ChangeToUnderwear("everything");
                }
                if (CalculateInventory.topSR.CompareTag(tag))
                {
                    CalculateInventory.topSR.sprite = CalculateInventory.transparent;
                    CalculateInventory.topASR.sprite = CalculateInventory.transparent;
                    CalculateInventory.topSSR.sprite = CalculateInventory.transparent;
                    
                    ChangeToUnderwear("top");
                    CalculateInventory.wearingTop = false;

                }
                if (CalculateInventory.otherSR.CompareTag(tag))
                {
                    CalculateInventory.otherSR.sprite = CalculateInventory.transparent;
                    CalculateInventory.otherASR.sprite = CalculateInventory.transparent;
                    CalculateInventory.otherSSR.sprite = CalculateInventory.transparent;
                    
                    ChangeToUnderwear("bottom");
                    CalculateInventory.wearingBottom = false;

                }
                if (CalculateInventory.shoeSR.CompareTag(tag))
                {
                    CalculateInventory.shoeSR.sprite = CalculateInventory.transparent;
                    CalculateInventory.shoeASR.sprite = CalculateInventory.transparent;
                    CalculateInventory.shoeSSR.sprite = CalculateInventory.transparent;
                    
                    CalculateInventory.wearingShoe = false;

                }

                //reset the machine's variables
                WasherControllerList[i].transform.tag ="Untagged";
                WasherControllerList[i].myMachineState = AllMachines.MachineState.empty;
                WasherControllerList[i].isFirstOpen = true;
                WasherControllerList[i].clothNum = 4;

//                print("destroy");
//                print(this.transform.parent.gameObject.name);

                GameObject bagToReturn = new GameObject();
                
                for (int y = 0; y < SubwayMovement.bagsInCar.Count; y++)
                {
                    if (CompareTag(SubwayMovement.bagsInCar[y].tag))
                    {
                        bagToReturn = SubwayMovement.bagsInCar[y];
                    }
                }
                
                //如果是晚还包的话
                //必须后destroy这个包，因为destroy在每个frame的最后执行，如果先destroy的话，Coroutine就不会被执行了
                if (FinalCameraController.lateReturnComic && !FinalCameraController.isTutorial)//call这一整个function之前就已经是true了
                {
                    //先播放一个包不见的动画
                    bagToReturn.GetComponent<Image>().sprite = CalculateInventory.disappear;
                    
                    yield return new WaitForSeconds(0.2f);

                    bagToReturn.GetComponent<Image>().sprite = CalculateInventory.transparent;

                    yield return new WaitForSeconds(0.5f);
            
                    FinalCameraController.myHSS.GoToScreen(1);
                    yield return new WaitForSeconds(0.2f);
            
                    FinalCameraController.Show(FinalCameraController.fishTalk);
                    FinalCameraController.fishTalkText.text = "Return your customers' clothes on time! Such bad memory!";
                    FinalCameraController.lateReturnComic = false;
                   
                }
              

                    //this function is called in the notice script, so destroy itself doesn't destroy the bag
                    //need to find the bag with the same bagu

                    for (int y = 0; y < SubwayMovement.bagsInCar.Count; y++)
                    {
                        if (CompareTag(SubwayMovement.bagsInCar[y].tag))
                        {
                            Destroy(SubwayMovement.bagsInCar[y]);

                            SubwayMovement.bagsInCar.Remove(SubwayMovement.bagsInCar[y]);
                        }
                    }

                    Destroy(FinalCameraController.generatedNotice);
                    AllMachines.isReturning = false;
                
              
            }
        }
        
        
    }



    
    public void returnClothYesDirectly()
    {

        //包消失
        
        FinalCameraController.AllStationClothList.Remove(tag);
        FinalCameraController.alreadyNotice = false;
        //去掉这个洗衣机上的tag

        if(!FinalCameraController.isTutorial)
        {
            AllMachines.nameToTemp[tag].Clear();
            //reset the temp list, so all the clothes are in the temp list
            for (int i = 0; i < AllMachines.nameToPermenant[tag].Count; i++)
            {
                AllMachines.nameToTemp[tag].Add(AllMachines.nameToPermenant[tag][i]);
            }
        }
        
        for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
        {
            if (WasherControllerList[i].CompareTag(tag))
            {
                FinalCameraController.returnMachineNum = i;

//                print("AllMachines.currentBag.tag = " + AllMachines.currentBag.tag);
                //对所有inventory中带着这个洗衣机tag的衣服，把它们放回洗衣机
                
                for (int u = 0; u < CalculateInventory.inventory.Count; u++)
                {
                    if (CalculateInventory.inventory[u].CompareTag(tag))
                    {
                        //change the inventory button image back to start
                        print(tag + CalculateInventory.inventory[u].name);
                        CalculateInventory.inventory[u].GetComponent<Image>().sprite = FinalCameraController.startSprite;
                        
                        print("returnrnrnrnrnrnrn");
                        //then 
                        CalculateInventory.occupiedI = CalculateInventory.occupiedI - 1;
                        //reset the tag of the inventory item
                        CalculateInventory.inventory[u].tag = "Untagged";
                    }
                }
                //todo: can be simpler
                //如果karara身上正好穿着这件衣服，那么在地铁、换装、广告界面都要脱下来
                if (CalculateInventory.everythingSR.CompareTag(this.tag))
                {
                    CalculateInventory.everythingSR.sprite = CalculateInventory.transparent;
                    CalculateInventory.everythingASR.sprite = CalculateInventory.transparent;
                    CalculateInventory.everythingSSR.sprite = CalculateInventory.transparent;
                    
                    CalculateInventory.wearingEverything = false;
                    //如果穿的是这件衣服的话
                    
                    ChangeToUnderwear("everything");
                }
                if (CalculateInventory.topSR.CompareTag(tag))
                {
                    CalculateInventory.topSR.sprite = CalculateInventory.transparent;
                    CalculateInventory.topASR.sprite = CalculateInventory.transparent;
                    CalculateInventory.topSSR.sprite = CalculateInventory.transparent;
                    
                    ChangeToUnderwear("top");
                    CalculateInventory.wearingTop = false;

                }
                if (CalculateInventory.otherSR.CompareTag(tag))
                {
                    CalculateInventory.otherSR.sprite = CalculateInventory.transparent;
                    CalculateInventory.otherASR.sprite = CalculateInventory.transparent;
                    CalculateInventory.otherSSR.sprite = CalculateInventory.transparent;
                    
                    ChangeToUnderwear("bottom");
                    CalculateInventory.wearingBottom = false;

                }
                if (CalculateInventory.shoeSR.CompareTag(tag))
                {
                    CalculateInventory.shoeSR.sprite = CalculateInventory.transparent;
                    CalculateInventory.shoeASR.sprite = CalculateInventory.transparent;
                    CalculateInventory.shoeSSR.sprite = CalculateInventory.transparent;
                    
                    CalculateInventory.wearingShoe = false;

                }

                //reset the machine's variables
                WasherControllerList[i].transform.tag ="Untagged";
                WasherControllerList[i].myMachineState = AllMachines.MachineState.empty;
                WasherControllerList[i].isFirstOpen = true;
                WasherControllerList[i].clothNum = 4;

//                print("destroy");
//                print(this.transform.parent.gameObject.name);

                GameObject bagToReturn = new GameObject();
                
                for (int y = 0; y < SubwayMovement.bagsInCar.Count; y++)
                {
                    if (CompareTag(SubwayMovement.bagsInCar[y].tag))
                    {
                        bagToReturn = SubwayMovement.bagsInCar[y];
                    }
                }
             
              

                    //this function is called in the notice script, so destroy itself doesn't destroy the bag
                    //need to find the bag with the same bagu

                    for (int y = 0; y < SubwayMovement.bagsInCar.Count; y++)
                    {
                        if (CompareTag(SubwayMovement.bagsInCar[y].tag))
                        {
                            Destroy(SubwayMovement.bagsInCar[y]);

                            SubwayMovement.bagsInCar.Remove(SubwayMovement.bagsInCar[y]);
                        }
                    }

                    Destroy(FinalCameraController.generatedNotice);
                    
                    //reset hitTime so the machine contain different bag of clothes next time
                    //don't really need to reset because hitTime is only related to bags
//                hitTime = 0;
                    if (FinalCameraController.isTutorial && FinalCameraController.TutorialManager.tutorialNumber == 16)
                    {
                        FinalCameraController.TutorialManager.tutorialNumber = 17;
                        FinalCameraController.TutorialManager.screamImage.enabled = true;
                        FinalCameraController.TutorialManager.scrollControl(true);
                        Destroy(FinalCameraController.TutorialManager.bag);
                    }
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
        if(!FinalCameraController.isTutorial)
        {
            FinalCameraController.LevelManager.isInstruction = false;
        }
        
        FinalCameraController.CancelAllUI(false);

        if(FinalCameraController.isSwipping == false)
        {
            if (hitTime == 0)
            {
                //for tutorial
                if (FinalCameraController.isTutorial)
                {
                    WasherControllerList[0].myMachineState = AllMachines.MachineState.bagUnder;
                    //change machine tags to character
                    WasherControllerList[0].transform.gameObject.tag = this.transform.gameObject.tag;

                    this.gameObject.transform.SetParent(WasherControllerList[0].gameObject.transform);

                    transform.position =
                        AllMachines.WashingMachines[0].transform.position + new Vector3(0, -2.5f, 0);

                    FinalCameraController.TutorialManager.scrollControl(true);

                        myHSS.GetComponent<HorizontalScrollSnap>().GoToScreen(2);
                                      
                        
                    FinalCameraController.TutorialManager.KararaStandingImage.enabled = false;
                    FinalCameraController.TutorialManager.tutorialNumber = 3;

                    FinalCameraController.TutorialManager.stopDisappear = false;
                    
                    hitTime++;
                }
                else
                {
                    SubwayMovement.bagNum -= 1;
                    for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
                    {
                        //get machine start washing
                        if (WasherControllerList[i].myMachineState == AllMachines.MachineState.empty)
                        {
                            WasherControllerList[i].myMachineState = AllMachines.MachineState.bagUnder;
                            //change machine tags to character
                            WasherControllerList[i].transform.gameObject.tag = this.transform.gameObject.tag;

                            this.gameObject.transform.SetParent(WasherControllerList[i].gameObject.transform);

                            transform.position =
                                AllMachines.WashingMachines[i].transform.position + new Vector3(0, -2.7f, 0);
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
            }
            
            else if (hitTime == 1)
            {
                //点第二次换成打开的包
                print("tag = " + tag);
                myImage.sprite = AllMachines.openBagsDic[this.tag];
                
                //in tutorial
                if (FinalCameraController.isTutorial)
                {
                    FinalCameraController.TutorialManager.tutorialNumber = 4;
//                    FinalCameraController.TutorialManager.bag.GetComponent<Image>().material.DisableKeyword("SHAKEUV_ON");
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
                                    AllMachines.currentBag = this.gameObject;//currentbag 是指产生了notice的这个包

                                    print("returnClothhhhhh");
                                    if (FinalCameraController.isTutorial)
                                    {
                                        if(FinalCameraController.TutorialManager.tutorialNumber == 16)
                                        {
//                                            FinalCameraController.DisableInput(true);
                                            FinalCameraController.TutorialManager.scrollControl(false);
                                            //then return all clothes in the machine
                                            FinalCameraController.generatedNotice = Instantiate(AllMachines.returnNotice,
                                                new Vector3(0, 0, 0),
                                                Quaternion.identity, WasherControllerList[i].gameObject.transform);

                                            FinalCameraController.generatedNotice.tag = this.tag;
                                            FinalCameraController.generatedNotice.transform.SetParent(WasherControllerList[2].gameObject.transform);
                                            //change karara back into work cloth
                                            
                                            FinalCameraController.alreadyNotice = true;

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
                                        FinalCameraController.generatedNotice.tag = this.tag;
                                        FinalCameraController.alreadyNotice = true;


                                        if (i == 1)
                                        {
                                            FinalCameraController.generatedNotice.transform.SetParent(WasherControllerList[2].gameObject.transform);
                                        }
                                        else
                                        {
                                            FinalCameraController.generatedNotice.transform.SetParent(WasherControllerList[i].gameObject.transform);
                                        }

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

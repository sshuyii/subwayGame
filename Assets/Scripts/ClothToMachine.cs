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

    private List<WasherController> WasherControllerList;

    private WasherController WasherController;

    private FinalCameraController FinalCameraController;

    private int hitTime;

    private Image myImage;

    // Start is called before the first frame update
    void Start()
    {
        //find the horizontal scroll snap script
        myHSS = GameObject.Find("Horizontal Scroll Snap").GetComponent<HorizontalScrollSnap>();


        myImage = GetComponent<Image>();
        hitTime = 0;
        
        ClothInMachineController = GameObject.Find("---ClothInMachineController");
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();
        
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

        
    }


    public void returnClothYes()
    {
        for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
        {
            if (WasherControllerList[i].CompareTag(AllMachines.currentBag.tag))
            {
                print("AllMachines.currentBag.tag = " + AllMachines.currentBag.tag);

                Destroy(AllMachines.currentBag);

                //reset the machine's variables
                WasherControllerList[i].transform.tag = null;
                WasherControllerList[i].myMachineState = AllMachines.MachineState.empty;
                Destroy(FinalCameraController.generatedNotice);
                
                //reset hitTime so the machine contain different bag of clothes next time
                hitTime = 0;
                WasherControllerList[i].isFirstOpen = true;

                if (FinalCameraController.isTutorial)
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
                for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
                {
//                    //get machine start washing
                    if (WasherControllerList[i].myMachineState == AllMachines.MachineState.empty)
                    {
                        //WasherControllerList[i].myMachineState = AllMachines.MachineState.full;
                        //change machine tags to character
                        WasherControllerList[i].transform.gameObject.tag = this.transform.gameObject.tag;

                        this.gameObject.transform.SetParent(WasherControllerList[i].gameObject.transform);

                        transform.position =
                            AllMachines.WashingMachines[i].transform.position + new Vector3(0, -2.9f, 0);
                        if (i == 0)
                        {
                            myHSS.GetComponent<HorizontalScrollSnap>().GoToScreen(1);

                        }
                        else
                        {
                            myHSS.GetComponent<HorizontalScrollSnap>().GoToScreen(2);

                        }
                        
                        //disable karara image in tutorial
                        if (FinalCameraController.isTutorial)
                        {
                            FinalCameraController.TutorialManager.KararaImage.enabled = false;
                            FinalCameraController.TutorialManager.tutorialNumber = 3;

                            
                        }

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
                
                //in tutorial
                if (FinalCameraController.isTutorial)
                {
                    FinalCameraController.TutorialManager.tutorialNumber = 4;

                }

                for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
                {
                    //get machine start washing
                    if (WasherControllerList[i].myMachineState == AllMachines.MachineState.empty)
                    {
                        WasherControllerList[i].myMachineState = AllMachines.MachineState.full;
                        //change machine tags to character
                        WasherControllerList[i].transform.gameObject.tag = this.transform.gameObject.tag;


                        hitTime++;
                        break;
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
                                        returnClothYes();
                                    }
                                    else
                                    {
                                        //generate the notice
                                        FinalCameraController.generatedNotice = Instantiate(AllMachines.returnNotice,
                                            new Vector3(0, 0, 0),
                                            Quaternion.identity, WasherControllerList[i].gameObject.transform);

                                    
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

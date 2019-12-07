using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class ClothToMachine : MonoBehaviour
{
    private GameObject ClothInMachineController;

    private AllMachines AllMachines;

    private List<WasherController> WasherControllerList;

    private WasherController WasherController;

    private NewCameraController NewCameraController;

    private int hitTime;

    // Start is called before the first frame update
    void Start()
    {
        hitTime = 0;
        
        ClothInMachineController = GameObject.Find("---ClothInMachineController");
        NewCameraController = GameObject.Find("Main Camera").GetComponent<NewCameraController>();
        
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

                WasherControllerList[i].transform.tag = "untagged";
                WasherControllerList[i].myMachineState = AllMachines.MachineState.empty;
                Destroy(AllMachines.generatedNotice);
                Destroy(AllMachines.currentBag);
                
                //reset hitTime so the machine contain different bag of clothes next time
                hitTime = 0;
                WasherControllerList[i].isFirstOpen = true;
                
                
                break;
            }
        }
    }


    public void returnClothNo()
    {
        
        Destroy(AllMachines.generatedNotice);

    }
    
    
    
    public void putClothIn()
    {
        if(NewCameraController.isSwipping == false)
        {
            if (hitTime == 0)
            {
                for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
                {
                    //get machine start washing
                    if (WasherControllerList[i].myMachineState == AllMachines.MachineState.empty)
                    {
                        WasherControllerList[i].myMachineState = AllMachines.MachineState.washing;
                        //change machine tags to character
                        WasherControllerList[i].transform.gameObject.tag = this.transform.gameObject.tag;
                        transform.position =
                            AllMachines.WashingMachines[i].transform.position + new Vector3(0, -2.5f, 0);

                        hitTime++;
                        break;
                    }
                }
            }
            else if (hitTime > 0)
            {
                //get the machine with clothes from this bag
                for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
                {
                    if (WasherControllerList[i].CompareTag(this.transform.gameObject.tag))
                    {
                        if (WasherControllerList[i].myMachineState == AllMachines.MachineState.finished)
                        {
                            //WasherControllerList[i].myMachineState = AllMachines.MachineState.empty;

                            //generate the notice
                            AllMachines.generatedNotice = Instantiate(AllMachines.returnNotice, new Vector3(0, 0, 0),
                                Quaternion.identity, AllMachines.noticeParent.transform);

                            AllMachines.currentBag = this.gameObject;

                            //print("AllMachines.currentBag.tag = " + AllMachines.currentBag.tag);

                            hitTime++;
                            break;
                        }
                    }
                }
            }
        }
        
    }
   
}

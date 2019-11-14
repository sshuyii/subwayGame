using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothToMachine : MonoBehaviour
{
    private GameObject ClothInMachineController;

    private AllMachines AllMachines;

    private List<WasherController> WasherControllerList;

    private WasherController WasherController;

    private int hitTime;
    // Start is called before the first frame update
    void Start()
    {
        hitTime = 0;
        
        ClothInMachineController = GameObject.Find("---ClothInMachineController");
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


    public void putClothIn()
    {
        if(hitTime == 0)
        {
            for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
            {
                //get machine start washing
                if (WasherControllerList[i].myMachineState == AllMachines.MachineState.empty)
                {
                    WasherControllerList[i].myMachineState = AllMachines.MachineState.washing;
                    //change machine tags to character
                    WasherControllerList[i].transform.gameObject.tag = this.transform.gameObject.tag;
                    transform.position = AllMachines.WashingMachines[i].transform.position + new Vector3(0, -2.5f, 0);

                    hitTime++;
                    break;
                }
            }
        }
        else if (hitTime == 1)
        {
            //get the machine with clothes from this bag
            for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
            {
                if (WasherControllerList[i].CompareTag(this.transform.gameObject.tag))
                {
                    if(WasherControllerList[i].myMachineState == AllMachines.MachineState.finished)
                    {
                        WasherControllerList[i].myMachineState = AllMachines.MachineState.empty;

                        GetComponent<Image>().enabled = false;

                        hitTime++;
                        break;
                    }
                }
            }
        }
        
    }
   
}

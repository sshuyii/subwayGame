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
    // Start is called before the first frame update
    void Start()
    {
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
        for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
        {

            if (WasherControllerList[i].myMachineState == AllMachines.MachineState.empty)
            {
                WasherControllerList[i].myMachineState = AllMachines.MachineState.washing;
            }
        }
    }
   
}

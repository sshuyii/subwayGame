using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllMachines : MonoBehaviour
{    
    //控制所有洗衣机的代码
    //只有这一份
    //all of alex's clothes
    public List<Button> AlexClothes;
    private List<Button> alexClothesTemp = new List<Button>();

    private bool isFirstOpen = true;

    //all the machines
    public List<GameObject> WashingMachines = new List<GameObject>();

    public GameObject MachineGroup;

    public int washTime;

    public bool isWashing;
    private bool isLoad;
    
    public enum MachineState {
        empty,
        washing,
        finished
    }

    public MachineState myMachineState;
    
    //all the buttons, aka clothes in the machine
    //length should be 5, which is the number of buttons on screen

//    public Button[] clothesInMachine;

    //length should also be 5, which is the number of buttons on screen
    private Sprite[] UIsprites;

    //all possible positions of buttons on the interface
    public Vector3[] buttonPositions;
    


    // Start is called before the first frame update
    void Start()
    {
        //make a copy of the list of clothes
        for (int i = 0; i < AlexClothes.Count; i++)
        {
            alexClothesTemp.Add(AlexClothes[i]);
        }
        print(alexClothesTemp.Count);

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void GenerateCloth()
    {
        
        //randomly generate clothes when player opens the machine
        if(isFirstOpen && isLoad == false)
        { 
            for (int i = 0; i < buttonPositions.Length; i++)
            {
                int randomIndex = Random.Range(0, alexClothesTemp.Count);
                print("random = " + randomIndex);
//                clothesInMachine[i].image.sprite = AlexClothes[randomIndex];

                Button ClothInMachine = Instantiate(alexClothesTemp[randomIndex], buttonPositions[i], Quaternion.identity) as Button;

                alexClothesTemp.Remove(alexClothesTemp[randomIndex]);
                
                //as child of the folder
                ClothInMachine.transform.SetParent(MachineGroup.transform, false);

                //this will make scale a lot bigger than it should be
//              ClothInMachine.transform.parent = MachineGroup.transform;

            

            }

            isFirstOpen = false;
            isLoad = true;
        }
        
    }

    public void ClothToMachine()
    {
        
    }
    
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WasherController : MonoBehaviour
{
    
    public TouchController TouchController;
    public AllMachines AllMachines;
    public NewCameraController NewCameraController;


    public CanvasGroup ClothUI;
    
    private bool isFirstOpen = true;


    public AllMachines.MachineState myMachineState;

//    private Button[] btns;
    private int shut = 0;
    //public Vector3 offsetTap;
    //public GameObject subway;

    private float timer = 0;

    public GameObject[] buttons;
    
    // Start is called before the first frame update
    void Start()
    {
        myMachineState = AllMachines.MachineState.empty;
        
//        btns = ButtonGroup.gameObject.GetComponents<Button>();


    }

    // Update is called once per frame
    void Update()
    {
        
        //if lick a bag of cloth, put them into the machine and start washing
        if (myMachineState == AllMachines.MachineState.washing)
        {
            timer += Time.deltaTime;
            
            if (timer > AllMachines.washTime)
            {
                myMachineState = AllMachines.MachineState.finished;
            }
        }

        
        if(myMachineState == AllMachines.MachineState.finished)
        {
            //tap a washing machine
            if (TouchController.myInputState == TouchController.InputState.Tap &&
                NewCameraController.myCameraState != NewCameraController.CameraState.Closet &&
                Input.touchCount > 0)
            {
                Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit raycastHit;
                if (Physics.Raycast(raycast, out raycastHit))
                {
                    Debug.Log("Something Hit");
                    if (raycastHit.collider.name == this.gameObject.name)
                    {
                        if (shut == 0 && TouchController.touch.phase == TouchPhase.Ended)
                        {
                            //subway.transform.position -= offsetTap;
                            print("hitting the machine");
                            shut++;
                            Show(ClothUI);
                           
                            GenerateCloth(this.transform.gameObject.tag);
                        }
                        //if click machine again, close UI
                        else if (shut == 1)
                        {
                            shut = 0;
                            //subway.transform.position += offsetTap;
                            Hide(ClothUI);
                        }
                    }
                    //if click machine content, don't close UI interface
                    else if (raycastHit.collider.name == "background")
                    {
                        Show(ClothUI);

                    }
                    //if it is a second touch
                    else if (shut == 1)
                    {
                        shut = 0;
                        //subway.transform.position += offsetTap;
                        Hide(ClothUI);
                    }

                }
                else
                {
                    if (shut == 1)
                    {
                        shut = 0;
                        //subway.transform.position += offsetTap;
                        Hide(ClothUI);
                        //backgroundSR.enabled = false;
                    }


                }
            }
        }
        
        //any touch cancels cloth UI
        else if(TouchController.myInputState != TouchController.InputState.None)
        {
            if (shut == 1)
            {
                shut = 0;
                //subway.transform.position += offsetTap;
                Hide(ClothUI);
                //backgroundSR.enabled = false;
            }
        }

        
        void Hide(CanvasGroup UIGroup) {
            UIGroup.alpha = 0f; //this makes everything transparent
            UIGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
            UIGroup.interactable = false;

//            foreach (Button btn in btns)
//            {
//                btn.enabled = false;
//            }            
        }
    
        void Show(CanvasGroup UIGroup) {
            UIGroup.alpha = 1f;
            UIGroup.blocksRaycasts = true;
            UIGroup.interactable = true;

            
//            foreach (Button btn in btns)
//            {
//                btn.enabled = true;
//            }
            
        }
    }
    
    
    
    
    public void GenerateCloth(string tagName)
    {
        
        //randomly generate clothes when player opens the machine
        if(isFirstOpen)
        { 
            for (int i = 0; i < buttons.Length; i++)
            {
                if(tagName == "Alex")
                {
                    int randomIndex = Random.Range(0, AllMachines.alexClothesTemp.Count);
                    //print("random = " + randomIndex);
//                  clothesInMachine[i].image.sprite = AlexClothes[randomIndex];

//                    Button ClothInMachine =
//                        Instantiate(alexClothesTemp[randomIndex], buttonPositions[i], Quaternion.identity) as Button;

                    Image buttonImage = buttons[i].GetComponent<Image>();
                    buttonImage.sprite = AllMachines.alexClothesTemp[randomIndex];
                    
                        
                    AllMachines.alexClothesTemp.Remove(AllMachines.alexClothesTemp[randomIndex]);
                    //as child of the folder
                    //doesn't work for some reason I don't understand
                    //ClothInMachine.transform.SetParent(MachineGroup.transform, false);
                    //ClothInMachine.transform.SetParent(MachineGroup.transform, false);


                }
                else if (tagName == "Bella")
                {
                   
                    int randomIndex = Random.Range(0, AllMachines.bellaClothesTemp.Count);
                    print("random = " + randomIndex);
                    print(AllMachines.bellaClothesTemp.Count);
//                  clothesInMachine[i].image.sprite = AlexClothes[randomIndex];

//                    Button ClothInMachine =
//                        Instantiate(bellaClothesTemp[randomIndex], buttonPositions[i], Quaternion.identity) as Button;

                    Image buttonImage = buttons[i].GetComponent<Image>();
                    buttonImage.sprite = AllMachines.bellaClothesTemp[randomIndex];
                    
                    AllMachines.bellaClothesTemp.Remove(AllMachines.bellaClothesTemp[randomIndex]);
                    //as child of the folder
                    //doesn't work for some reason I don't understand

                    //ClothInMachine.transform.SetParent(MachineGroup.transform, false);

                }
                
               
                //this will make scale a lot bigger than it should be
//              ClothInMachine.transform.parent = MachineGroup.transform;

            

            }

            isFirstOpen = false;
        }
        
    }
}

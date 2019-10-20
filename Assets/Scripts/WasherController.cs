using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasherController : MonoBehaviour
{
    
    public TouchController TouchController;
    public AllMachines AllMachines;
    public NewCameraController NewCameraController;


    public CanvasGroup clothUI;

    public AllMachines.MachineState myMachineState;
    
    
    public int shut = 0;
    //public Vector3 offsetTap;
    //public GameObject subway;

    private float timer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        myMachineState = AllMachines.MachineState.empty;

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
                NewCameraController.myCameraState != NewCameraController.CameraState.Closet)
            {
                Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit raycastHit;
                if (Physics.Raycast(raycast, out raycastHit))
                {
                    Debug.Log("Something Hit");
                    if (raycastHit.collider.name == "Machine1")
                    {
                        if (shut == 0 && TouchController.touch.phase == TouchPhase.Ended)
                        {
                            //subway.transform.position -= offsetTap;
                            print("hitting the machine");
                            shut++;
                            Show(clothUI);
                            AllMachines.GenerateCloth();
                        }
                        //if click machine again, close UI
                        else if (shut == 1)
                        {
                            shut = 0;
                            //subway.transform.position += offsetTap;
                            Hide(clothUI);
                        }
                    }
                    //if click machine content, don't close UI interface
                    else if (raycastHit.collider.name == "background")
                    {
                        Show(clothUI);

                    }
                    //if it is a second touch
                    else if (shut == 1)
                    {
                        shut = 0;
                        //subway.transform.position += offsetTap;
                        Hide(clothUI);
                    }

                }
                else
                {
                    if (shut == 1)
                    {
                        shut = 0;
                        //subway.transform.position += offsetTap;
                        Hide(clothUI);
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
                Hide(clothUI);
                //backgroundSR.enabled = false;
            }
        }

        
        void Hide(CanvasGroup UIGroup) {
            UIGroup.alpha = 0f; //this makes everything transparent
            UIGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
        }
    
        void Show(CanvasGroup UIGroup) {
            UIGroup.alpha = 1f;
            UIGroup.blocksRaycasts = true;
        }
    }
}

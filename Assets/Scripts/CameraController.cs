using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    
    public CanvasGroup clothUI;

    public int shut = 0;
    private bool TOF;
    public Vector3 offsetTap;
    public GameObject subway;

    private Touch touch;
    private enum InputState {
        LeftSwipe,
        RightSwipe,
        Tap,
        None,
    }
    InputState myInputState;

    
    
    void Start()
    {
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
    }
 
    void Update()
    {
        
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if (Input.GetTouch(i).tapCount == 2)
                {
                    Debug.Log("Double Tap");
                }
            }
        }
        
        
        print("shut = " + shut);
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list
 
                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            Debug.Log("Right Swipe");
                            myInputState = InputState.RightSwipe;
                        }
                        else
                        {   //Left swipe
                            Debug.Log("Left Swipe");
                            myInputState = InputState.LeftSwipe;

                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            Debug.Log("Up Swipe");
                        }
                        else
                        {   //Down swipe
                            Debug.Log("Down Swipe");
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    Debug.Log("Tap");
                    myInputState = InputState.Tap;
                }
            }
        }

        
        //tap a washing machine
        if (myInputState == InputState.Tap)
        {            
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                Debug.Log("Something Hit");
                if (raycastHit.collider.name == "Machine")
                {
                    if(shut == 0 && touch.phase == TouchPhase.Ended)
                    {
                        subway.transform.position -= offsetTap;
                        print("hitting the machine");
                        shut++;
                        Show(clothUI);
                    } 
                    else if (shut == 1)
                    {
                        shut = 0;
                        subway.transform.position += offsetTap;
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
                    subway.transform.position += offsetTap;
                    Hide(clothUI);
                }
               
            }
            else
            {
                if (shut == 1)
                {
                    shut = 0;
                    subway.transform.position += offsetTap;
                    Hide(clothUI);
                    //backgroundSR.enabled = false;
                }


            }
        }
        
        //any touch cancels cloth UI
        else
        {
            if (shut == 1)
            {
                shut = 0;
                subway.transform.position += offsetTap;
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
//if not treat background as a button, use this function
//        bool ClickBG()
//        {
//            if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
//            {
//                var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
//                if (buttonSelf.name == "background")
//                {
//                    print("clicking yellow background");
//                    TOF = true;
//                }
//                else
//                {
//                    TOF = false;
//                }
//                print("tof = " + TOF);
//
//            }
//
//            return TOF;
//        }
                
    }
    

}

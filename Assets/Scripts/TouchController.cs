using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    private Vector3 fp;   //First touch position
    public Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    private FinalCameraController FinalCameraController;

    public bool isSwiping = false;
    public bool isSwipable = false;
    private bool TOF;
   
    public Touch touch;

    public bool leftSwipe;
    
    public float offsetX;
    public Transform camTransform;
    public enum InputState {
        LeftSwipe,
        RightSwipe,
        Tap,
        None,
    }
    public InputState myInputState;


    public bool isLongTap;
    public bool isFastSwipe;
    private float startTime;
    private float diffTime;
    private float swipeDistance;

    private float swipeSpeed;

    public bool doubleTouch = true;
    
// Start is called before the first frame update
    void Start()
    {
        myInputState = InputState.None;
        dragDistance = Screen.height * 8 / 100;
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();
    }

    // Update is called once per frame
    void Update()
    {

//        print("cancelCloth = " + doubleTouch);
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if (Input.GetTouch(i).tapCount == 2)
                {
//                    Debug.Log("Double Tap");
                    doubleTouch = true;
                }
                
                
            }
            else if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                doubleTouch = false;
            }
        }
        
        
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;

                startTime = Time.time;
                
                offsetX = camTransform.position.x - lp.x;
                
//                print("offsetX = " + offsetX);
//                print("lp = " + lp);
                
                
                myInputState = InputState.None;
                
                //for swipe screen
                isSwipable = true;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                
                
                lp = touch.position;
//                print("lp = " + lp);

                if (Mathf.Abs(lp.y - fp.y) > dragDistance/4 || Mathf.Abs(lp.x - fp.x) > dragDistance/4)
                {
                    isSwiping = true;
                    
                    //disable dialogue bubble if swiped
                    if(FinalCameraController.isTutorial && !FinalCameraController.TutorialManager.stopDisappear)
                    {
                        FinalCameraController.Hide(FinalCameraController.TutorialManager.GestureCG);
                        FinalCameraController.TutorialManager.screamImage.enabled = false;
                        FinalCameraController.TutorialManager.DoFishDialogue(false);
                    }               
                }
                else
                { 
                    
                    isSwiping = false;
                }

                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {
                    
                }
                else
                {
                    //it is a long tap
                    if (Time.time - startTime > 0.8f)
                    {
                        isLongTap = true;
                    }
                    else
                    {
                        isLongTap = false;
                    }
                }


            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                
//                if(FinalCameraController.isTutorial && FinalCameraController.myCameraState == FinalCameraController.CameraState.Subway)
//                {
//                    if(FinalCameraController.TutorialManager.tutorialNumber != 6 && FinalCameraController.TutorialManager.tutorialNumber != 7)
//                    {
//                        FinalCameraController.TutorialManager.tutorialDialogueState =
//                            TutorialManager.DialogueState.fish;
//                    }                
//                }

                if (FinalCameraController.mySubwayState != FinalCameraController.SubwayState.One &&
                    FinalCameraController.TutorialManager.tutorialNumber == 1)
                {
                    FinalCameraController.Show(FinalCameraController.TutorialManager.GestureCG);
                    FinalCameraController.TutorialManager.screamImage.enabled = true;
                    FinalCameraController.TutorialManager.DoFishDialogue(true);
                }


                lp = touch.position;  //last touch position. Ommitted if you use list
        
                offsetX = 0;
                isSwiping = false;
                
                //check if this is a fast swipe
                diffTime = startTime - Time.time;
                swipeDistance = Vector2.Distance(fp,lp);
                swipeSpeed = Mathf.Abs(swipeDistance / diffTime);//<<<<<<<<
//                print("diffTime =" + diffTime);
//                print("swipeSpeed =" + swipeSpeed);
                if (swipeSpeed > 500 && Mathf.Abs(diffTime) < 0.2f)
                {
                    isFastSwipe = true;
                    
                }
                else
                {
                    isFastSwipe = false;
                }
                
                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            Debug.Log("Right Swipe");
                            leftSwipe = false;
                            myInputState = InputState.RightSwipe;
                        }
                        else
                        {   //Left swipe
                            Debug.Log("Left Swipe");
                            leftSwipe = true;
                            
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
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    private Vector3 fp;   //First touch position
    public Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered


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
    

    public bool isFastSwipe;
    private float startTime;
    private float diffTime;
    private float swipeDistance;

    private float swipeSpeed;

// Start is called before the first frame update
    void Start()
    {
        myInputState = InputState.None;
    }

    // Update is called once per frame
    void Update()
    {
        
        print("leftSwipe = " + leftSwipe);
        
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if (Input.GetTouch(i).tapCount == 2)
                {
//                    Debug.Log("Double Tap");
                }
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

                

            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list
        
                offsetX = 0;
                
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

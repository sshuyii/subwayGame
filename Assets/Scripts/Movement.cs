using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    private float cameraX;
    private float cameraY;
    private float cameraZ;

    public float speed;

    public CanvasGroup wash1;
    public CanvasGroup bottom;

    
    public float a;

    private bool isSwipe;

    public SpriteRenderer uiR;

    private int shut = 0;
    private Vector3 offsetTap = new Vector3(0, 400, 0);
    
    void Start()
    {
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen

        cameraX = transform.position.x;
        cameraY = transform.position.y;
        
        //clothing icons
        
        
    }
 
    void Update()
    {
        
        cameraX = transform.position.x;
        cameraY = transform.position.y;

        
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
                uiR.enabled = false;
                if (shut == 1)
                {
                    transform.position += offsetTap;
                    shut = 0;
                    Hide(wash1);
                    Show(bottom);
                }
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                //background move left and right
                lp = touch.position;

                if (lp.x > fp.x && transform.position.x > -1710)//right
                {
                    isSwipe = true;
                }
                else if (lp.x < fp.x && transform.position.x < 1285)
                {
                    isSwipe = true;
                }
                else
                {
                    isSwipe = false;
                }
                
                if(isSwipe)

                {
                    cameraX = (lp.x - fp.x) * speed;
                    cameraX = Mathf.Round(cameraX);
                    cameraY = Mathf.Round(cameraX * a);

//                    print("x = " + cameraX);
//                    print("y = " + cameraY);
                    transform.position -= new Vector3(cameraX, cameraY, 0);

                }


                

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
                        }
                        else
                        {   //Left swipe
                            Debug.Log("Left Swipe");
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
                }
            }
        }
        
        //tap a washing machine
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                Debug.Log("Something Hit");
                if (raycastHit.collider.name == "Machine")
                {
                    Debug.Log("Soccer Ball clicked");
                    uiR.enabled = true;
                    transform.position -= offsetTap;
                    shut++;
                    Show(wash1);
                    Hide(bottom);

                }
//
//                //OR with Tag
//
//                if (raycastHit.collider.CompareTag("SoccerTag"))
//                {
//                    Debug.Log("Soccer Ball clicked");
//                }
            }
            else
            {
                uiR.enabled = false;
                if (shut == 1)
                {
                    shut = 0;
                    transform.position += offsetTap;
                    Hide(wash1);
                    Show(bottom);

                }

            }
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

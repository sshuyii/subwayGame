﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraController : MonoBehaviour
{
    public TouchController TouchController;
    public enum CameraState {
        One,
        Two,
        Three,
        Four,
        Closet,
        Map,
        App
    }

    public CanvasGroup GoBack;
    private CameraState lastCameraState;
    
    public CameraState myCameraState;
    private Vector3 movement;
    public float distance;
    public CanvasGroup inventory;
    public CanvasGroup machine;
    public CanvasGroup basicUI;

    
    // Start is called before the first frame update
    void Start()
    {
        myCameraState = CameraState.Two;
        movement = new Vector3(2*distance, 0, 0);
        
    }

    
    // Update is called once per frame
    void Update()
    {
        
//        print("lastCameraState = " + lastCameraState);
        //if changing clothes, don't show some UIs
        if (myCameraState == CameraState.Map || myCameraState == CameraState.App)
        {
            Hide(machine);
            Hide(inventory);
            Show(GoBack);
            Hide(basicUI);
        }

        else if(myCameraState == CameraState.Closet)
        {
            Show(inventory);
            Hide(machine);
            Hide(basicUI);

            Show(GoBack);

 
        }
        
        else
        {
            Hide(inventory);
            Hide(GoBack);
            Show(basicUI);
            Show(machine);

            
        }
        
        if (TouchController.myInputState == TouchController.InputState.RightSwipe)
        {
            //two to one
            if (myCameraState == CameraState.Two && TouchController.isSwipable)
            {
                //swipe to left
                if (transform.position.x > -2 * distance)
                {
                    transform.position -= movement;

                }
                else
                {
                    myCameraState = CameraState.One;
                    TouchController.isSwipable = false;
                }
            }
            else if(myCameraState == CameraState.Three && TouchController.isSwipable)
            {
                //swipe to left
                if (transform.position.x > 0f)
                {
                    transform.position -= movement;

                }
                else
                {
                    myCameraState = CameraState.Two;
                    TouchController.isSwipable = false;


                }
            }
            else if(myCameraState == CameraState.Four && TouchController.isSwipable)
            {
                //swipe to left
                if (transform.position.x > 2 * distance)
                {
                    transform.position -= movement;

                }
                else
                {
                    myCameraState = CameraState.Three;
                    TouchController.isSwipable = false;

                }
            }
        }
        else if (TouchController.myInputState == TouchController.InputState.LeftSwipe)
        {
            //two to one
            if (myCameraState == CameraState.Two && TouchController.isSwipable)
            {
                //swipe to left
                if (transform.position.x < 2 * distance)
                {
                    transform.position += movement;
                }
                else
                {
                    myCameraState = CameraState.Three;
                    TouchController.isSwipable = false;


                }
            }
            else if(myCameraState == CameraState.Three && TouchController.isSwipable)
            {
                //swipe to left
                if (transform.position.x < 4 * distance)
                {
                    transform.position += movement;

                }
                else
                {
                    myCameraState = CameraState.Four;
                    TouchController.isSwipable = false;


                }
            }
            else if(myCameraState == CameraState.One && TouchController.isSwipable)
            {
                //swipe to left
                if (transform.position.x < 0)
                {
                    transform.position += movement;

                }
                else
                {
                    myCameraState = CameraState.Two;
                    TouchController.isSwipable = false;


                }
            }
        }
        else
        {
            if (myCameraState == CameraState.One)
            {
                transform.position = new Vector3(-2 * distance, 0, -10);
            }
            else if (myCameraState == CameraState.Two)
            {
                transform.position = new Vector3(0, 0, -10);
            }
            else if (myCameraState == CameraState.Three)
            {
                transform.position = new Vector3(2 * distance, 0, -10);
            }
            else if (myCameraState == CameraState.Four)
            {
                transform.position = new Vector3(4 * distance, 0, -10);
            }
        }
        
        
    }

    public void ChangeToCloth()
    {
        //print("myCameraState = " + myCameraState);
        lastCameraState = myCameraState;
        myCameraState = CameraState.Closet;
        transform.position = new Vector3(0, -14, -10);
    }

    public void ChangeToSubway()
    {
        //transform.position = new Vector3(0, 0, -10);
        if(myCameraState == CameraState.Closet || myCameraState == CameraState.Map || myCameraState == CameraState.App)
        {
            if(lastCameraState != CameraState.Closet && lastCameraState != CameraState.Map && lastCameraState != CameraState.App)
            {
                myCameraState = lastCameraState;
            }
            else
            {
                myCameraState = CameraState.Two;
            }
        }

        Hide(machine.GetComponent<CanvasGroup>());
    }
    
    public void ChangeToApp()
    {
        //transform.position = new Vector3(0, 0, -10);
        lastCameraState = myCameraState;
        myCameraState = CameraState.App;
        transform.position = new Vector3(11, 13, -10);
          
    }
    
    public void ChangeToMap()
    {
        lastCameraState = myCameraState;
        myCameraState = CameraState.Map;
        transform.position = new Vector3(0, 13, -10); 
    }

    void Hide(CanvasGroup UIGroup) {
        UIGroup.alpha = 0f; //this makes everything transparent
        UIGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
        UIGroup.interactable = false;
    }
    
    void Show(CanvasGroup UIGroup) {
        UIGroup.alpha = 1f;
        UIGroup.blocksRaycasts = true;
        UIGroup.interactable = true;
    }
}
using System.Collections;
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
    }
    
    public CameraState myCameraState;
    private Vector3 movement;
    public float distance;

    
    // Start is called before the first frame update
    void Start()
    {
        myCameraState = CameraState.Two;
        movement = new Vector3(2*distance, 0, 0);
        
    }

    
    // Update is called once per frame
    void Update()
    {
        

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
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraController : MonoBehaviour
{
    public TouchController TouchController;

    public enum CameraState
    {
        One,
        Two,
        Three,
        Four,
        Closet,
        Map,
        App,
        Ad
    }


    public List<float> pageCount = new List<float>();

    public int nearestPage;
    private int currentPage;


    public float speed;

    public CanvasGroup GoBack;
    private CameraState lastCameraState;

    public CameraState myCameraState;
    private Vector3 movement;
    public float distance;
    public CanvasGroup inventory;
    public CanvasGroup machine;
    public CanvasGroup basicUI;


    private float offsetX;

    // Start is called before the first frame update
    void Start()
    {

        myCameraState = CameraState.Two;
        movement = new Vector3(2 * distance, 0, 0);

        //add all the subway pages to the list
        pageCount.Add(-2 * distance);
        pageCount.Add(0 * distance);
        pageCount.Add(2 * distance);
        pageCount.Add(4 * distance);
    }


    // Update is called once per frame
    void Update()
    {

//        print("lastCameraState = " + lastCameraState);
        //if changing clothes, don't show some UIs
        if (myCameraState == CameraState.Map || myCameraState == CameraState.App || myCameraState == CameraState.Ad)

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

       
        
       GetCurrentPage();
       GetNearestPage();
       
        //if touch ends, decide which state the camera goes to
        if (myCameraState == CameraState.Map || myCameraState == CameraState.App || myCameraState == CameraState.Ad || myCameraState == CameraState.Closet)
        {
            
        }
        else
        {
            //move camera according to touch position
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                transform.Translate(-touchDeltaPosition.x * speed, transform.position.y, 0);
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                print("isFastSwipe has end = " + TouchController.isFastSwipe);
            
                transform.position = new Vector3((nearestPage - 1) * 2 * distance, transform.position.y, transform.position.z);

            }
        }
        

//        if (TouchController.myInputState == TouchController.InputState.RightSwipe)
//        {
//            //two to one
//            if (myCameraState == CameraState.Two && TouchController.isSwipable)
//            {
//                //swipe to left
//                if (transform.position.x > -2 * distance)
//                {
//                    transform.position -= movement;
//
//                }
//                else
//                {
//                    myCameraState = CameraState.One;
//                    TouchController.isSwipable = false;
//                }
//            }
//            else if(myCameraState == CameraState.Three && TouchController.isSwipable)
//            {
//                //swipe to left
//                if (transform.position.x > 0f)
//                {
//                    transform.position -= movement;
//
//                }
//                else
//                {
//                    myCameraState = CameraState.Two;
//                    TouchController.isSwipable = false;
//
//
//                }
//            }
//            else if(myCameraState == CameraState.Four && TouchController.isSwipable)
//            {
//                //swipe to left
//                if (transform.position.x > 2 * distance)
//                {
//                    transform.position -= movement;
//
//                }
//                else
//                {
//                    myCameraState = CameraState.Three;
//                    TouchController.isSwipable = false;
//
//                }
//            }
//        }
//        else if (TouchController.myInputState == TouchController.InputState.LeftSwipe)
//        {
//            //two to one
//            if (myCameraState == CameraState.Two && TouchController.isSwipable)
//            {
//                //swipe to left
//                if (transform.position.x < 2 * distance)
//                {
//                    transform.position += movement;
//                }
//                else
//                {
//                    myCameraState = CameraState.Three;
//                    TouchController.isSwipable = false;
//
//
//                }
//            }
//            else if(myCameraState == CameraState.Three && TouchController.isSwipable)
//            {
//                //swipe to left
//                if (transform.position.x < 4 * distance)
//                {
//                    transform.position += movement;
//
//                }
//                else
//                {
//                    myCameraState = CameraState.Four;
//                    TouchController.isSwipable = false;
//
//
//                }
//            }
//            else if(myCameraState == CameraState.One && TouchController.isSwipable)
//            {
//                //swipe to left
//                if (transform.position.x < 0)
//                {
//                    transform.position += movement;
//
//                }
//                else
//                {
//                    myCameraState = CameraState.Two;
//                    TouchController.isSwipable = false;
//
//
//                }
//            }
//        }
//        else
//        {
//            if (myCameraState == CameraState.One)
//            {
//                transform.position = new Vector3(-2 * distance, 0, -10);
//            }
//            else if (myCameraState == CameraState.Two)
//            {
//                transform.position = new Vector3(0, 0, -10);
//            }
//            else if (myCameraState == CameraState.Three)
//            {
//                transform.position = new Vector3(2 * distance, 0, -10);
//            }
//            else if (myCameraState == CameraState.Four)
//            {
//                transform.position = new Vector3(4 * distance, 0, -10);
//            }
//        }
        
        
    }

    public void ChangeToCloth()
    {
        //print("myCameraState = " + myCameraState);
        lastCameraState = myCameraState;
        myCameraState = CameraState.Closet;
        transform.position = new Vector3(-25, 0, -10);
    }

    public void ChangeToSubway()
    {
        //transform.position = new Vector3(0, 0, -10);
        if(myCameraState == CameraState.Closet || myCameraState == CameraState.Map || myCameraState == CameraState.App || myCameraState == CameraState.Ad)
        {
            if(lastCameraState != CameraState.Closet && lastCameraState != CameraState.Map && lastCameraState != CameraState.App && myCameraState != CameraState.Ad)
            {
                myCameraState = lastCameraState;
               
            }
            else
            {
                myCameraState = CameraState.Two;
            }
        }

        Hide(machine.GetComponent<CanvasGroup>());
        
        GoSubwayPart();
    }
    
    public void ChangeToApp()
    {
        //transform.position = new Vector3(0, 0, -10);
        lastCameraState = myCameraState;
        myCameraState = CameraState.App;
        transform.position = new Vector3(34, 0, -10);
          
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
    private void GetNearestPage()
    {
//        for (int i = 0; i < pageCount.Count; i++)
//            {
//                float testDist = Mathf.Abs(transform.position.x - pageCount[i]);
//                if (testDist < distance)
//                {
//                    nearestPage = i;
//                }
//            }
//        
        if(TouchController.isFastSwipe == false)
        {
            for (int i = 0; i < pageCount.Count; i++)
            {
                float testDist = Mathf.Abs(transform.position.x - pageCount[i]);
                if (testDist < distance)
                {
                    nearestPage = i;
                }
            }
        }
        else
        {
            if (TouchController.myInputState == TouchController.InputState.LeftSwipe)
            {
                nearestPage = currentPage++;
                TouchController.isFastSwipe = false;
            }
            else if (TouchController.myInputState == TouchController.InputState.RightSwipe)
            {
                nearestPage = currentPage--;
                TouchController.isFastSwipe = false;
            }
        }
        
        //keep nearest page between 0-3
        if (nearestPage < 0)
        {
            nearestPage = 0;
        }
        else if (nearestPage > 3)
        {
            nearestPage = 3;
        }
    }
    private void GetCurrentPage() {

        if (transform.position.y == 0f)
        {
            if (transform.position.x < -2 * distance && transform.position.x > -3 * distance)
            {
                myCameraState = CameraState.One;
                currentPage = 0;
            }
            else if (transform.position.x > -2 * distance && transform.position.x < 0)
            {
                myCameraState = CameraState.Two;
                currentPage = 1;
            }
            else if (transform.position.x >  0 && transform.position.x < 2 * distance)
            {
                myCameraState = CameraState.Three;
                currentPage = 2;
            }
            else if (transform.position.x > 2 * distance && transform.position.x < 4.1 * distance)
            {
                myCameraState = CameraState.Four;
                currentPage = 3;
            }
        }
        
        // based on distance from current position, find nearest page
        Vector2 currentPosition = transform.position;
    }
    
//    private void LerpToPage(int aPageIndex) {
//        aPageIndex = Mathf.Clamp(aPageIndex, 0, _pageCount - 1);
//        _lerpTo = _pagePositions[aPageIndex];
//        _lerp = true;
//        _currentPage = aPageIndex;
//    }

    private void GoSubwayPart()
    {
        //temporary way of putting camera back to subway, need to be replaced
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

    public void GoAdvertisement()
    {
        lastCameraState = myCameraState;
        transform.position = new Vector3(24, 0, -10);
        myCameraState = CameraState.Ad;
        
    }
}

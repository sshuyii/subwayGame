using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class NewCameraController : MonoBehaviour
{
    public TouchController TouchController;

    private bool fastSwipeBool;


    public enum AppState
    {
        Mainpage,
        KararaPage,
        RetroPage,
        Post
        
    }

    private AppState lastAppState;

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

    private bool isLerp = false;

    public List<float> pageCount = new List<float>();

    public int nearestPage;
    public int currentPage;


    public float speed;

    public CanvasGroup GoBack;
    private CameraState lastCameraState;

    public CameraState myCameraState;
    public AppState myAppState;
    private Vector3 movement;
    public float distance;
    public CanvasGroup inventory;
    public CanvasGroup machine;
    public CanvasGroup basicUI;

    public CanvasGroup appBackground;
    public CanvasGroup mainpage;
    public CanvasGroup postpage;
    public CanvasGroup KararaPage;
    public CanvasGroup RetroPage;
    public CanvasGroup DesignerPage;

    public List<CanvasGroup> pageList = new List<CanvasGroup>();

    


    private float offsetX;

    // Start is called before the first frame update
    void Start()
    {

        myCameraState = CameraState.Two;
        myAppState = AppState.Mainpage;
        movement = new Vector3(2 * distance, 0, 0);

        //add all the subway pages to the list
        pageCount.Add(-2 * distance);
        pageCount.Add(0 * distance);
        pageCount.Add(2 * distance);
        pageCount.Add(4 * distance);
        
        pageList.Add(RetroPage);
        pageList.Add(KararaPage);
        pageList.Add(DesignerPage);


        Hide(mainpage);
        Hide(postpage);
        HideAllPersonalPages();

        
    }

    

    // Update is called once per frame
    void Update()
    {

        GetCurrentPageEverytime();
            
//        print("lastCameraState = " + lastCameraState);
        //if changing clothes, don't show some UIs
        if (myCameraState == CameraState.Map || myCameraState == CameraState.App || myCameraState == CameraState.Ad)
        {
            if (myCameraState == CameraState.App)
            {
                Hide(machine);
                Hide(inventory);
                Hide(GoBack);
                Hide(basicUI);
                Show(appBackground);
            }
            else
            {
                Hide(machine);
                Hide(inventory);
                Show(GoBack);
                Hide(basicUI);
                Hide(appBackground);

            }
        }

        else if(myCameraState == CameraState.Closet)
        {
            Show(inventory);
            Hide(machine);
            Hide(basicUI);

            Show(GoBack);
            Hide(appBackground);

        }
        
        else
        {
            Hide(inventory);
            Hide(GoBack);
            Show(basicUI);
            Show(machine);
            Hide(appBackground);

        }

       
        //if touch ends, decide which state the camera goes to
        if (myCameraState == CameraState.Map || myCameraState == CameraState.App || myCameraState == CameraState.Ad || myCameraState == CameraState.Closet)
        {
            
        }
        else
        {
            if(Input.touchCount > 0)
            //move camera according to touch position
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    isLerp = false;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                    transform.Translate(-touchDeltaPosition.x * speed, transform.position.y, 0);
                    print("touch phase is moved");
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    print("isFastSwipe has end = " + TouchController.isFastSwipe);

                    GetNearestPage();

                    isLerp = true;

                    GetCurrentPage();

                }
            }
            
            if (isLerp)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3((nearestPage - 2) * 2 * distance, transform.position.y, transform.position.z), Time.deltaTime * 20);

                if (transform.position.x == (nearestPage - 2) * 2 * distance)
                {
                    isLerp = false;
                }
            }
        }

        
    }

    private void LerpToPage() {

        
    }
    public void ChangeToCloth()
    {
        
            //print("myCameraState = " + myCameraState);
            lastCameraState = myCameraState;
            myCameraState = CameraState.Closet;
            transform.position = new Vector3(-25, 0, -10);
    }

    public void AppBackButton()
    {
       if (myAppState == AppState.Post)
       {
           
            Show(KararaPage);
            Hide(mainpage);
            HideAllPersonalPages();
            myAppState = AppState.KararaPage;


       }
       else if (myAppState == AppState.Mainpage)
       {
            ChangeToSubway();
       }
       else if(myAppState == AppState.RetroPage || myAppState == AppState.KararaPage)
       {
           Show(mainpage);
           HideAllPersonalPages();

           myAppState = AppState.Mainpage;
       }
       

      
    }

    public void ChangeToSubway()
    {
       
            //transform.position = new Vector3(0, 0, -10);
            if (myCameraState == CameraState.Closet || myCameraState == CameraState.Map ||
                myCameraState == CameraState.App || myCameraState == CameraState.Ad)
            {
                if (lastCameraState != CameraState.Closet && lastCameraState != CameraState.Map &&
                    lastCameraState != CameraState.App && myCameraState != CameraState.Ad)
                {
                    myCameraState = lastCameraState;
                }
                else
                {
                    myCameraState = CameraState.Two;
                }
            }

            Hide(machine.GetComponent<CanvasGroup>());
            Hide(mainpage);

            GoSubwayPart();
        
    }
    
    public void ChangeToApp()
    {
        
            //transform.position = new Vector3(0, 0, -10);
            
            lastCameraState = myCameraState;
            myCameraState = CameraState.App;
            myAppState = AppState.Mainpage;
            
            transform.position = new Vector3(35, 0, -10);

            print("mainpageeeeee");
            Show(mainpage);

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
        if(TouchController.isFastSwipe == false)
        {
            print("fast swipe is false" + TouchController.isFastSwipe);
            for (int i = 0; i < pageCount.Count; i++)
            {
                float testDist = Mathf.Abs(transform.position.x - pageCount[i]);
                if (testDist < distance)
                {
                    nearestPage = i + 1;
                }
            }
        }
        else
        {
            if (TouchController.leftSwipe)
            {
//                print(nearestPage);
//                print(currentPage);

                nearestPage = currentPage + 1;
                print("leftFastSwipe");
//                print(nearestPage);
//                print(currentPage);


            }
            else
            {
                nearestPage = currentPage - 1;                         
                print("rightFastSwipe");

            }
           
        }
        
        //keep nearest page between 1-4
        if (nearestPage < 1)
        {
            nearestPage = 1;
        }
        else if (nearestPage > 4)
        {
            nearestPage = 4;
        }
        
        //keep nearest page between 1-4
        if (currentPage < 1)
        {
            currentPage = 1;
        }
        else if (currentPage > 4)
        {
            currentPage = 4;
        }
    }
    
    
    private void GetCurrentPage() {

//        if (transform.position.y == 0f)
//        {
            if (transform.position.x < -1 * distance && transform.position.x > -3 * distance)
            {
                myCameraState = CameraState.One;
                currentPage = 1;

            }
            else if (transform.position.x > -1 * distance && transform.position.x < distance)
            {
                myCameraState = CameraState.Two;
                currentPage = 2;

            }
            else if (transform.position.x > distance && transform.position.x < 3 * distance)
            {
                myCameraState = CameraState.Three;
                currentPage = 3;

            }
            else if (transform.position.x > 3 * distance && transform.position.x < 5 * distance)
            {
                myCameraState = CameraState.Four;
                currentPage = 4;

            }
//        }
        
        // based on distance from current position, find nearest page
        Vector2 currentPosition = transform.position;
    }
    private void GetCurrentPageEverytime() {

//        if (transform.position.y == 0f)
//        {
        if (transform.position.x < -1 * distance && transform.position.x > -3 * distance)
        {
            currentPage = 1;

        }
        else if (transform.position.x > -1 * distance && transform.position.x < distance)
        {
            currentPage = 2;

        }
        else if (transform.position.x > distance && transform.position.x < 3 * distance)
        {
            currentPage = 3;

        }
        else if (transform.position.x > 3 * distance && transform.position.x < 5 * distance)
        {
            currentPage = 4;

        }
//        }
        
        // based on distance from current position, find nearest page
        Vector2 currentPosition = transform.position;
    }
    


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
    
    public void GoMainpage()
    {
            transform.position = new Vector3(35, 0, -10);        
            Show(mainpage);

    }
    public void GoPersonalpage()
    {
        transform.position = new Vector3(55, 0, -10);        
        Hide(mainpage);

    }

    public void HideAllPersonalPages()
    {
        for (var i = 0; i < pageList.Count; i++)
        {
            Hide(pageList[i]);
        }
    }
}

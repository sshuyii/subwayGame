using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class FinalCameraController : MonoBehaviour
{
    public TouchController TouchController;

    private bool fastSwipeBool;

    public bool isSwipping = false;

    public enum AppState
    {
        Mainpage,
        KararaPage,
        RetroPage,
        DesignerPage,
        NPCPage,
        Post
    }

    public AppState lastAppState;

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

    public CanvasGroup GoBack;
    public CameraState lastCameraState;

    public CameraState myCameraState;
    public AppState myAppState;
    private Vector3 movement;
    public CanvasGroup inventory;
    //public CanvasGroup basicUI;

    public CanvasGroup appBackground;
    public CanvasGroup frontPage;
    public CanvasGroup postpage;
    public CanvasGroup KararaPage;
    public CanvasGroup RetroPage;
    public CanvasGroup DesignerPage;
    public CanvasGroup NPCPage;
    public CanvasGroup subwayBackground;


    public List<CanvasGroup> pageList = new List<CanvasGroup>();
    

    // Start is called before the first frame update
    void Start()
    {

        myCameraState = CameraState.Two;
        myAppState = AppState.Mainpage;

    
        
        pageList.Add(RetroPage);
        pageList.Add(KararaPage);
        pageList.Add(DesignerPage);


        Hide(frontPage);
        Hide(postpage);
        HideAllPersonalPages();

        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (TouchController.isSwiping == true)
        {
            isSwipping = true;
        }
        else
        {
            isSwipping = false;
        }

            
        //if changing clothes, don't show some UIs
        if (myCameraState == CameraState.Map || myCameraState == CameraState.App || myCameraState == CameraState.Ad)
        {
            if (myCameraState == CameraState.App)
            {
                Hide(inventory);
                Hide(GoBack);
                //Hide(basicUI);
                Show(appBackground);
            }
            else
            {
                Hide(inventory);
                Show(GoBack);
                //Hide(basicUI);
                Hide(appBackground);
            }
        }

        else if(myCameraState == CameraState.Closet)
        {
            Show(inventory);
            //Hide(basicUI);

            Show(GoBack);
            Hide(appBackground);

        }
        
        else
        {
            Hide(inventory);
            Hide(GoBack);
            //Show(basicUI);
            Hide(appBackground);

        }
    }


    
    public void ChangeToCloth()
    {
        Hide(subwayBackground);

        if(isSwipping == false)
        {
            //print("myCameraState = " + myCameraState);
            lastCameraState = myCameraState;
            myCameraState = CameraState.Closet;
            transform.position = new Vector3(-25, 0, -10);
        }
    }

    public void AppBackButton()
    {
       if (myAppState == AppState.Post)
       {
           
            //Hide(mainpage);
            Hide(postpage);
            HideAllPersonalPages();
            
            //go back to the personal page the post belongs to
            if (lastAppState == AppState.KararaPage)
            {
                Show(KararaPage);
                myAppState = AppState.KararaPage;
            }
            else if (lastAppState == AppState.RetroPage)
            {
                Show(RetroPage);
                myAppState = AppState.RetroPage;
            }
            else if (lastAppState == AppState.DesignerPage)
            {
                Show(DesignerPage);
                myAppState = AppState.DesignerPage;
            }
            


       }
       else if (myAppState == AppState.Mainpage)
       {
           lastCameraState = CameraState.Two;
            ChangeToSubway();
       }
       else if(myAppState == AppState.RetroPage || myAppState == AppState.KararaPage || myAppState == AppState.DesignerPage || myAppState == AppState.NPCPage)
       {
           Show(frontPage);
           HideAllPersonalPages();
           Hide(postpage);
           Hide(NPCPage);


           myAppState = AppState.Mainpage;
       }
       

      
    }

    public void ChangeToSubway()
    {
        Show(subwayBackground);
        transform.position = new Vector3(0, 0, -10);
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

        Hide(frontPage);
        Hide(appBackground);

        GoSubwayPart();
        
    }
    
    public void ChangeToApp()
    {
        Hide(subwayBackground);

        if(isSwipping == false)
        {
            //transform.position = new Vector3(0, 0, -10);

            
            lastCameraState = myCameraState;
            myCameraState = CameraState.App;
            myAppState = AppState.Mainpage;

            transform.position = new Vector3(35, 0, -10);

            print("mainpageeeeee");
            Show(frontPage);
        }
    }
    
    public void ChangeToMap()
    {  
        Hide(subwayBackground);
        if(isSwipping == false)
        {
            lastCameraState = myCameraState;
            myCameraState = CameraState.Map;
            transform.position = new Vector3(0, 13, -10);
        }
        
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
    
 
 
    private void GoSubwayPart()
    {
        //temporary way of putting camera back to subway, need to be replaced
//        if (myCameraState == CameraState.One)
//        {
//            transform.position = new Vector3(-2 * distance, 0, -10);
//
//        }
//        else if (myCameraState == CameraState.Two)
//        {
//            transform.position = new Vector3(0, 0, -10);
//
//        }
//        else if (myCameraState == CameraState.Three)
//        {
//            transform.position = new Vector3(2 * distance, 0, -10);
//
//        }
//        else if (myCameraState == CameraState.Four)
//        {
//            transform.position = new Vector3(4 * distance, 0, -10);
//
//        }
    }

    public void GoAdvertisement()
    {
        Hide(subwayBackground);

        lastCameraState = myCameraState;
        transform.position = new Vector3(24, 0, -10);
        myCameraState = CameraState.Ad;
    }
    
    public void GoMainpage()
    {
        transform.position = new Vector3(35, 0, -10);        
        Show(frontPage);

    }
    public void GoPersonalpage()
    {
        transform.position = new Vector3(55, 0, -10);        
        //Hide(mainpage);

    }

    public void HideAllPersonalPages()
    {
        for (var i = 0; i < pageList.Count; i++)
        {
            Hide(pageList[i]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;


public class FinalCameraController : MonoBehaviour
{
    public TouchController TouchController;
    public AllMachines AllMachines;

    private bool fastSwipeBool;

    public TutorialManager TutorialManager;

    public bool isSwipping = false;
    public bool isTutorial;

    public CanvasGroup fishTalk;
    public TextMeshProUGUI fishTalkText;
    private bool isfishTalking = false;
    public HorizontalScrollSnap HorizontalScrollSnap;
    
    public HorizontalScrollSnap myHSS;
    public ScrollRect subwayScrollRect;

    //a dictionary of all the clothes that comes from station 0 and are currently in the machines 
    public List<Sprite> ClothStation0 = new List<Sprite>();
    public Dictionary<string, List<Sprite>> AllStationClothList = new Dictionary<string, List<Sprite>>();
    
    public Sprite startSprite;

    public CanvasGroup Inventory;

    public CanvasGroup clothCG;
    public CanvasGroup messageCG;

    public bool lateReturnComic;

    
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
        Subway,
        Closet,
        Map,
        App,
        Ad
    }
    
    public enum SubwayState
    {
        One,
        Two,
        Three,
        Four
    }

    public SubwayState mySubwayState;
    
    //if notice UI is on display, this is true
    public bool alreadyNotice = false;
    public bool alreadyClothUI = false;
    public CanvasGroup currentClothUI;
    
    
    
    public GameObject generatedNotice;



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
        myCameraState = CameraState.Subway;
        myAppState = AppState.Mainpage;
        
        myHSS = GameObject.Find("Horizontal Scroll Snap").GetComponent<HorizontalScrollSnap>();
        subwayScrollRect = GameObject.Find("Horizontal Scroll Snap").GetComponent<ScrollRect>();
        pageList.Add(RetroPage);
        pageList.Add(KararaPage);
        pageList.Add(DesignerPage);


        Hide(fishTalk);
        Hide(frontPage);
        Hide(postpage);
        HideAllPersonalPages();

       

        fishTalkText = fishTalk.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        //hide the UIs when click Karara
        Hide(clothCG);
        Hide(messageCG);
        Hide(Inventory);
    }


    public void CancelAllUI()
    {
        if (!isTutorial)
        {
            //touch anywhere on screen, close Karara UI
            Hide(clothCG);
            Hide(messageCG);
            isShown = false;
            //close fish talking
            Hide(fishTalk);
            if (alreadyNotice)
            {
                Hide(generatedNotice.GetComponent<CanvasGroup>());
            }
        

        for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
            {
                Hide(AllMachines.WashingMachines[i].GetComponent<WasherController>().backgroundUI);
                Hide(AllMachines.WashingMachines[i].GetComponent<WasherController>().ClothUI);
            }
        }
     
    }
    // Update is called once per frame
    void Update()
    {
        //code below doesn't work because it doesn't tell if is touch a button but UI element
        // Check if there is a touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Check if finger is over a UI element
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                //Debug.Log("Touched the UI");
            }
            else
            {
//                //touch anywhere on screen, close Karara UI
//                Hide(clothCG);
//                Hide(messageCG);
//                isShown = false;
//                //close fish talking
//                Hide(fishTalk);
            }
        }
        
        //disable swipe before player click the poster
        if (isTutorial)
        {
            if(TutorialManager.tutorialNumber == 0 && mySubwayState == SubwayState.Four)
            {
                //myHSS.GoToScreen(4);
                //myHSS.enabled = false;
            }        
            else if(TutorialManager.tutorialNumber == 2 && mySubwayState == SubwayState.One)
            {
                //myHSS.GoToScreen(1);
                //myHSS.enabled = false;
            }
        }
        
        //add a beginning and an end for the subway background scene
        if (HorizontalScrollSnap.CurrentPage == 5)
        {
            myHSS.GoToScreen(4);
        }
        else if(HorizontalScrollSnap.CurrentPage == 0)
        {
            myHSS.GoToScreen(1);
        }

        if (lateReturnComic)
        {
//            GoSubwayPart();
//            lateReturnImage.enabled = true;
            ChangeToSubway();
            myHSS.GoToScreen(1);
            Show(fishTalk);
            fishTalkText.text = "Return your customers' clothes in time! How can you have such bad memory!";
            lateReturnComic = false;

        }
        
        if (TouchController.isSwiping == true)
        {
            isSwipping = true;
        }
        else
        {
            isSwipping = false;
        }

            
        //print(HorizontalScrollSnap.CurrentPage);
        //change camera state to page number
        if(myCameraState == CameraState.Subway)
        {
            CheckScreenNum();
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

    public void BossTalk()
    {
        CancelAllUI();
        if(isfishTalking == false)
        {
            Show(fishTalk);
            isfishTalking = true;
            fishTalkText.text = "Concentrate on your work! Do the laundry!";
        }
        else
        {
            Hide(fishTalk);
            isfishTalking = false;
        }
    }
    
    void CheckScreenNum()
    {
        if (HorizontalScrollSnap.CurrentPage == 1)
        {
            mySubwayState = SubwayState.One;
        }
        else if (HorizontalScrollSnap.CurrentPage == 2)
        {
            mySubwayState = SubwayState.Two;
        }
        else if (HorizontalScrollSnap.CurrentPage == 3)
        {
            mySubwayState = SubwayState.Three;
        }
        else if (HorizontalScrollSnap.CurrentPage == 4)
        {
            mySubwayState = SubwayState.Four;
        }
    }

    private bool isShown;
    public void clickKarara()
    {
        print("clickKarara");
        if(!isShown)
        {
            CancelAllUI();
            Show(clothCG);
            Show(messageCG);
            isShown = true;
        }
        else
        {
            Hide(clothCG);
            Hide(messageCG);
            isShown = false;
        }
    }
    
    public void ChangeToCloth()
    {
        Show(Inventory);
        if (isTutorial)
        {
            if(TutorialManager.tutorialNumber < 11)
            {
                TutorialManager.tutorialNumber = 11;
            }        
            else if (TutorialManager.tutorialNumber == 14)
            {
                TutorialManager.fishText.text = "Cool";
                return;
            }
        }
        
        if(alreadyClothUI == false)
        {
            Hide(subwayBackground);

            if (isSwipping == false)
            {
                //print("myCameraState = " + myCameraState);
                lastCameraState = myCameraState;
                myCameraState = CameraState.Closet;
                transform.position = new Vector3(-25, 0, -10);
            }
        }
        else
        {
            Destroy(generatedNotice);
            Hide(currentClothUI);
            alreadyClothUI = false;
        }

      
    }

    public void clickLateComic()
    {
//        lateReturnComic = false;
//        lateReturnImage.enabled = false;
        Hide(fishTalk);


         
        print("clickLateComic");
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
           lastCameraState = CameraState.Subway;
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
        Hide(Inventory);
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
                myCameraState = CameraState.Subway;
            }
        }

        Hide(frontPage);
        Hide(appBackground);

        
        //for Tutorial
        if (isTutorial)
        {
            if (TutorialManager.tutorialNumber == 12)
            {
                mySubwayState = SubwayState.Four;
                myHSS.GetComponent<HorizontalScrollSnap>().GoToScreen(4);
                TutorialManager.tutorialNumber = 13;
             }
            else if(TutorialManager.tutorialNumber == 14)
            {
                mySubwayState = SubwayState.One;
                myHSS.GetComponent<HorizontalScrollSnap>().GoToScreen(1);
                TutorialManager.tutorialNumber = 15;
            }
            else if(TutorialManager.tutorialNumber == 16)
            {
                mySubwayState = SubwayState.One;
                myHSS.GetComponent<HorizontalScrollSnap>().GoToScreen(1);
                TutorialManager.tutorialNumber = 15;
            }
        }
        
    }
    
    public void ChangeToApp()
    {
        //cancel all dialogues
        print("click ChangeToAPP");

       if(alreadyClothUI == false)        
        {
            print("alreadyClothUi = false");
            Hide(subwayBackground);

            if (isSwipping == false)
            {
                //transform.position = new Vector3(0, 0, -10);
                lastCameraState = myCameraState;
                myCameraState = CameraState.App;
                myAppState = AppState.Mainpage;

                transform.position = new Vector3(35, 0, -10);
                Show(frontPage);
            }
        }
        else
        {
            print("alreadyClothUi = true");

            Destroy(generatedNotice);
            Hide(currentClothUI);
            alreadyClothUI = false;
        }

        
    }
    
    public void ChangeToMap()
    {  
        print("mappppppp");
        if(alreadyClothUI == false)        {
            Hide(subwayBackground);
            if (isSwipping == false)
            {
                lastCameraState = myCameraState;
                myCameraState = CameraState.Map;
                transform.position = new Vector3(0, 13, -10);
            }
        }
        else
        {
            Destroy(generatedNotice);
            Hide(currentClothUI);
            alreadyClothUI = false;
        }
        
    }

    public void Hide(CanvasGroup UIGroup) {
        UIGroup.alpha = 0f; //this makes everything transparent
        UIGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
        UIGroup.interactable = false;
    }
    
    public void Show(CanvasGroup UIGroup) {
        UIGroup.alpha = 1f;
        UIGroup.blocksRaycasts = true;
        UIGroup.interactable = true;
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

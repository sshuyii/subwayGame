using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class FinalCameraController : MonoBehaviour
{
    public TouchController TouchController;
    public AllMachines AllMachines;
    public LevelManager LevelManager;
    private InstagramController InstagramController;
    

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
    public CanvasGroup setting;

    
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
        Four,
        None
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
        
        Hide(setting);
        myCameraState = CameraState.Subway;
        myAppState = AppState.Mainpage;
        
        myHSS = GameObject.Find("Horizontal Scroll Snap").GetComponent<HorizontalScrollSnap>();
        subwayScrollRect = GameObject.Find("Horizontal Scroll Snap").GetComponent<ScrollRect>();
        InstagramController = GameObject.Find("---InstagramController").GetComponent<InstagramController>();

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
                Hide(fishTalk);
                Hide(setting);
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
        if (HorizontalScrollSnap.CurrentPage == 1 && HorizontalScrollSnap._settled)
        {
            mySubwayState = SubwayState.One;
        }
        else if (HorizontalScrollSnap.CurrentPage == 2 && HorizontalScrollSnap._settled)
        {
            mySubwayState = SubwayState.Two;
        }
        else if (HorizontalScrollSnap.CurrentPage == 3 && HorizontalScrollSnap._settled)
        {
            mySubwayState = SubwayState.Three;
        }
        else if (HorizontalScrollSnap.CurrentPage == 4 && HorizontalScrollSnap._settled)
        {
            mySubwayState = SubwayState.Four;
        }
        else
        {
            mySubwayState = SubwayState.None;
        }
    }

    public bool isShown = true;
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
            TutorialManager.scrollControl(false);

            if(TutorialManager.tutorialNumber < 13)
            {
                TutorialManager.tutorialNumber = 13;
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
           resetPostOrder();

       }
    }


    public void resetPostOrder()
    {

        print("resetttttttposterOrder");

        List<int> temp = new List<int>();
        List<GameObject> temp1 = new List<GameObject>();

        Dictionary<int, GameObject> tempDic = new Dictionary<int, GameObject>();
        Dictionary<GameObject, int> tempDic1 = new Dictionary<GameObject, int>();

        
        //reset the order of all posts
        for (int i = 0; i < InstagramController.postList.Count; i++)
        {
            temp.Add(InstagramController.postList[i].GetComponent<EntryTime>().time);
            tempDic.Add(InstagramController.postList[i].GetComponent<EntryTime>().time, InstagramController.postList[i]);

        }   
        //时间是不会重复的，每个npc的个位数都不一样
        
        temp.Sort();
        temp.Reverse();
        
        for (int i = 0; i < InstagramController.postList.Count; i++)
        {
            tempDic1.Add(tempDic[temp[i]], i);

        }  
        //需要一个list，按顺序放着所有的gameobject
        
        
        
        for (int i = 0; i < InstagramController.postList.Count; i++)
        {
            //order是gameobject在list中排的位置
            InstagramController.postList[i].GetComponent<EntryTime>().order = tempDic1[InstagramController.postList[i]];

            InstagramController.postList[i].transform.SetSiblingIndex(InstagramController.postList[i].GetComponent<EntryTime>().order);
        }           
        
//        for (int i = 0; i < InstagramController.postList.Count; i++)
//        {
//            InstagramController.postList[i].transform.SetSiblingIndex(-InstagramController.postList[i].GetComponent<EntryTime>().time);
//        }           
        
    }
    
    public void ChangeToSubway()
    {
        if(isTutorial)
        {
            TutorialManager.scrollControl(true);
        }

        else if (LevelManager.isInstruction)//换到鱼界面
        {
            myHSS.GoToScreen(1);
            LevelManager.clicktime = 5;
        }
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
            if (TutorialManager.tutorialNumber == 14)//从换装界面出来
            {
                TutorialManager.scrollControl(true);
                mySubwayState = SubwayState.Four;
                myHSS.GetComponent<HorizontalScrollSnap>().GoToScreen(4);
                TutorialManager.tutorialNumber = 15;
             }
            else if(TutorialManager.tutorialNumber == 15)//从地铁界面出来
            {
                mySubwayState = SubwayState.One;
                myHSS.GetComponent<HorizontalScrollSnap>().GoToScreen(1);
                TutorialManager.tutorialNumber = 16;
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
        //reset the order of all posts
        resetPostOrder();
        
        //cancel all dialogues
        print("click ChangeToAPP");
        
        //cancel red dot
        InstagramController.redDot.SetActive(false);

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

    private bool isSetting;
    public void clickSetting()
    { 
        CancelAllUI();
        if (isSetting)
        {
            Hide(setting);
            isSetting = false;
        }
        else
        {
            Show(setting);
            isSetting = true;
        }
    }
}

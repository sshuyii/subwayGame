﻿using System.Collections;
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

    public CanvasGroup disableInputCG;
    public CanvasGroup ChapterOneEndComic;
    public int entryTime = 1;
    public CanvasGroup TakePhoto;
    public CanvasGroup Posture;
    public bool ChapterOneEnd;
    public CanvasGroup Mask;
    
    public CanvasGroup fishShoutCG;

    public CanvasGroup ChapterOneFailCG;
    public bool machineOpen;

    public bool chapterOneSucceed;
    public bool chapterOneFail;

    public int returnMachineNum;//this is used to record the machine that should be turned to when retucn cloth
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

    public ScrollRect mainpageScrollRect;

    public CanvasGroup SubwayMap;
  
    
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
        
        if(!isTutorial)
        {
            Hide(setting);
        }
//        myCameraState = CameraState.Subway;
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
        Hide(TakePhoto);
        Hide(Posture);

       

        fishTalkText = fishTalk.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        //hide the UIs when click Karara
        Hide(clothCG);
        Hide(messageCG);
        Hide(Inventory);
    }


    public void CancelAllUI(bool clickMachine)
    {
        print("cancelallui");
        if (!isTutorial)
        { 
            //touch anywhere on screen, close Karara UI
            Hide(clothCG);
            Hide(messageCG);
            isShown = false;
            //close fish talking
//            Hide(fishTalk);
            if (alreadyNotice)
            {
                Hide(generatedNotice.GetComponent<CanvasGroup>());
            }

            for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
            {
               
                Hide(AllMachines.WasherControllerList[i].backgroundUI);
                Hide(AllMachines.WasherControllerList[i].backgroundUI3);
                Hide(AllMachines.WasherControllerList[i].backgroundUI2);
                Hide(AllMachines.WasherControllerList[i].ClothUI);
                if(!clickMachine)
                {
                    AllMachines.WasherControllerList[i].shut = 0;
                }                
                if(AllMachines.WasherControllerList[i].myMachineState == AllMachines.MachineState.finished)
                {
                    Show(AllMachines.WasherControllerList[i].Occupied);
                }
                AllMachines.WasherControllerList[i].DoorImage.sprite = AllMachines.closedDoor;
            }
        }
     
    }
    // Update is called once per frame
    void Update()
    {
        
        //see if a machine is open
        for (int i = 0; i < AllMachines.WasherControllerList.Count; i++)
        {
            if (!AllMachines.WasherControllerList[i].pressOK)
            {
                machineOpen = true;
                break;
            }
            else
            {
                machineOpen = false;
            }

        }
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
        
        //hide shout
        if(!isTutorial && LevelManager.isInstruction)
        {
            if (mySubwayState == SubwayState.One)
            {
                //LevelManager.isInstruction = false;
                Hide(fishShoutCG);
            }
            else
            {
                Show(fishShoutCG);
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
//            ChangeToSubway();
//            myHSS.GoToScreen(1);
//            Show(fishTalk);
//            fishTalkText.text = "Return your customers' clothes in time! How can you have such bad memory!";
//            lateReturnComic = false;

        }
        else
        {
        }
        
        if (TouchController.isSwiping == true)
        {
            isSwipping = true;
        }
        else
        {
            isSwipping = false;
        }

        if (isSwipping && !machineOpen)
        {
            CancelAllUI(false);
        }
            
        //print(HorizontalScrollSnap.CurrentPage);
        //change camera state to page number
        if(myCameraState == CameraState.Subway && !isSwipping)
        {
            CheckScreenNum();
            Show(subwayBackground);
        }
        
        
        //if changing clothes, don't show some UIs
        if (myCameraState == CameraState.Map || myCameraState == CameraState.App || myCameraState == CameraState.Ad)
        {
            if (myCameraState == CameraState.App)
            {
                Hide(inventory);
                //Hide(basicUI);
                Show(appBackground);
            }
            else
            {
                Hide(inventory);
                //Hide(basicUI);
                Hide(appBackground);
            }
        }

        else if(myCameraState == CameraState.Closet)
        {
            Show(inventory);
            //Hide(basicUI);
            Hide(subwayBackground);
            Hide(appBackground);

        }
        
        else
        {
            Hide(inventory);
            //Show(basicUI);
            Hide(appBackground);
        }
        
        //如果没到达30fo
        if (chapterOneFail)
        {
            StartCoroutine(ChapterOneFailComic());
        }
    }

    IEnumerator ChapterOneFailComic()
    {
        DisableInput(true);
        yield return new WaitForSeconds(1f);
        
        ChangeToSubway();
        myHSS.GoToScreen(2);


        Show(ChapterOneFailCG);
    }

    public void BossTalk()
    {
        CancelAllUI(false);
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
        if (HorizontalScrollSnap.CurrentPage == 1 && HorizontalScrollSnap._settled && !HorizontalScrollSnap._moveStarted)
        {

            mySubwayState = SubwayState.One;
        }
        else if (HorizontalScrollSnap.CurrentPage == 2 && HorizontalScrollSnap._settled && !HorizontalScrollSnap._moveStarted)
        {
            mySubwayState = SubwayState.Two;

        }
        else if (HorizontalScrollSnap.CurrentPage == 3 && HorizontalScrollSnap._settled && !HorizontalScrollSnap._moveStarted)
        {
            mySubwayState = SubwayState.Three;

        }
        else if (HorizontalScrollSnap.CurrentPage == 4 && HorizontalScrollSnap._settled && !HorizontalScrollSnap._moveStarted)
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
            CancelAllUI(false);//进行完了isshown一定是false?

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
        Mask.alpha = 0;
             
        print("ChangeToCLoth");
        if (isTutorial)
        {
            TutorialManager.scrollControl(false);

            print("ChangeToCLoth before 13");

            if(TutorialManager.tutorialNumber < 13)
            {
                TutorialManager.tutorialNumber = 13;
                print("ChangeToCLoth13");

            }
        }
        
        if(alreadyClothUI == false)
        {
            Hide(subwayBackground);

            print("alreadyCLoth = false" + isSwipping);

            if (isSwipping == false)
            {
                //print("myCameraState = " + myCameraState);
                lastCameraState = myCameraState;
                myCameraState = CameraState.Closet;
                transform.position = new Vector3(-25, 0, -10);
                print("actually change");
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
           if (ChapterOneEnd)//第一章结束，出现漫画？如果点了一下之后进入第二章
           {
               if(chapterOneSucceed)
               {
                   Show(ChapterOneEndComic); //漫画
               }
              
           }
           else
           {
               lastCameraState = CameraState.Subway;
               ChangeToSubway();
           }


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
        
        //清空所有内容
        temp.Clear();
        tempDic.Clear();
        tempDic1.Clear();
        
    }
    
    
    
    public static void ScrollToTop(ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 1);
    }
    
    public void ChangeToSubway()
    {
        Mask.alpha = 1;
        if(isTutorial)
        {
            TutorialManager.scrollControl(true);
            myCameraState = CameraState.Subway;

        }
        else if (LevelManager.isInstruction)//换到鱼界面
        {
            print("Final camera controller clicktime = 7");
            myHSS.GoToScreen(3);
            LevelManager.clicktime = 7;
            Show(fishShoutCG);
        }

            Hide(TakePhoto);
            Hide(Posture);
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
        

        //hide everything 
        Hide(Inventory);
        Show(subwayBackground);
        Hide(frontPage);
        Hide(appBackground);
        Hide(NPCPage);
        Hide(SubwayMap);
        Hide(postpage);
        
        HideAllPersonalPages(); 
        
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
        
        //go to main page top
        if(!isTutorial)
        {
            ScrollToTop(mainpageScrollRect);
        }        
        //cancel all dialogues
        print("click ChangeToAPP");
        
        //cancel red dot
        InstagramController.redDot.SetActive(false);
        Hide(TakePhoto);

       if(alreadyClothUI == false)        
        {
            print("alreadyClothUi = false");
            Hide(subwayBackground);

            if (isSwipping == false)
            {
                transform.position = new Vector3(0, 0, -10);
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
                Show(SubwayMap);
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

        Show(TakePhoto);
        Show(Posture);
        lastCameraState = myCameraState;
        transform.position = new Vector3(24, 0, -10);
        myCameraState = CameraState.Ad;
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
        CancelAllUI(false);
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

    public void DisableInput(bool temp)
    {
        disableInputCG.blocksRaycasts = temp;
    }
}

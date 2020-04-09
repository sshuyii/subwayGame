using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;


public class TutorialManager : MonoBehaviour
{
    public GameObject DialogueBubble;

    public CanvasGroup finalComic;
    public CanvasGroup anotherApp;
 
    public Image hint;
    public Image screamImage;
    public GameObject KararaDialogueBubble;

    private Animator DialogueBubbleAC;

    private RectTransform kararaDialogueRT;
    public RectTransform kararaTextRT;
    
    public CanvasGroup GestureCG;
    public FinalCameraController FinalCameraController;

    public GameObject ClothUI;
    private Button[] ClothUIButtons;
    public Image[] ClothUIImages;


    public bool chooseBag = false;
    public GameObject ScreenZero;
    public GameObject ScreenFour;
    
    private HorizontalScrollSnap myHSS;
    public Image[] DialogueImageList;
    public TextMeshProUGUI fishText;
    public Image arrow;
    public CanvasGroup arrowButton;
    
    public Image[] KararaDialogueImageList;
    public TextMeshProUGUI kararaText;

    
    public GameObject poster;

    public bool wearNothing;
   
    public GameObject TakeScreenshot;


    public GameObject KararaStanding;
    public GameObject KararaSitting;
    
    public Image KararaStandingImage;
    private Sprite KararaWorkCloth;
    private CanvasGroup KararaSittingCanvasGroup;


    
    public Image ProfileImage;
    public Sprite KararaProfile;
    public Sprite FishProfile;

    public TextMeshProUGUI nameTag;
    public Sprite DialogueKarara;
    public Sprite DialogueFish;

    public Image DialogueBackground;

    public bool pressScreenshot;
    public CanvasGroup myFlash;
    public bool isFishTalking;
    public bool isPrewash;

    public int tutorialNumber;

    public GameObject clothBag;
    public Vector2 bagPos;
    public GameObject clothBagGroup;

    private RectTransform KararaRectT;
    private Image[] KararaAllImage;

    public GameObject bag;
    public GameObject door;
    public GameObject cloth;
    public Button SubwayMap;

    private Image bagImage;
    private Image doorImage;
    private Image clothImage;


    public GameObject gobackButton;
    public Button KararaButton;

    private Color fishColor;
    private Color kararaColor;
    public Sprite KararaDisco;
    
    //use this to decide whether dialogues should disappear when swipe
    public bool stopDisappear = false;

    private Vector2 lowerPosition;
    private Vector2 higherPosition;
    private RectTransform DialogueRT;

    private Vector2 startPosition;
    private Vector2 leftPosition;
    private Vector2 rightPosition = new Vector2(175, 289);

    public Image NameTagImage;

    //which dialogue should be shown on screen
    public enum DialogueState
    {
        fish,
        karara,
        fishElse,
        none,
        all
    }
    
    public DialogueState tutorialDialogueState;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Hide(GestureCG);
        
        fishColor = new Color(176f/255f, 140f/255f, 84f/255f);
        
        myHSS = GameObject.Find("Horizontal Scroll Snap").GetComponent<HorizontalScrollSnap>();
        
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();

        KararaStandingImage = KararaStanding.GetComponent<Image>();

        KararaSittingCanvasGroup = KararaSitting.GetComponent<CanvasGroup>();
        kararaDialogueRT = KararaDialogueBubble.GetComponent<RectTransform>();
        KararaWorkCloth = KararaStandingImage.sprite;
        startPosition = kararaDialogueRT.anchoredPosition;
        
        StartCoroutine("TrainMoveIn");

        KararaRectT = KararaStanding.GetComponent<RectTransform>();
        
        DialogueImageList = DialogueBubble.GetComponentsInChildren<Image>();
        fishText = DialogueBubble.GetComponentInChildren<TextMeshProUGUI>();
        
        KararaDialogueImageList = KararaDialogueBubble.GetComponentsInChildren<Image>();
        kararaText = KararaDialogueBubble.GetComponentInChildren<TextMeshProUGUI>();



        DialogueRT = DialogueBubble.GetComponent<RectTransform>();
        lowerPosition = DialogueRT.anchoredPosition - new Vector2(0, 200);
        higherPosition = DialogueRT.anchoredPosition;
        //disable the dialogues
        DoFishDialogue(false);
        

        //disable karara standing in the train
//        KararaStandingImage.enabled = false;
        KararaDisappear(false);

        ClothUIButtons = ClothUI.GetComponentsInChildren<Button>();
        ClothUIImages = ClothUI.GetComponentsInChildren<Image>();
        
        doorImage = door.GetComponent<Image>();
        clothImage = cloth.GetComponent<Image>();

        ProfileImage.enabled = false;

        //disable scream
        screamImage.enabled = false;


        KararaAllImage = KararaSitting.gameObject.GetComponentsInChildren<Image>();

        //disable all shader effect
        //bag.GetComponent<Image>().material.EnableKeyword("SHAKEUV_OFF");
        cloth.GetComponent<Image>().material.DisableKeyword("SHAKEUV_ON");
        door.GetComponent<Image>().material.DisableKeyword("SHAKEUV_ON");

        foreach (var button in ClothUIButtons)
        {
            button.GetComponent<Image>().material.DisableKeyword("SHAKEUV_ON");
        }

        foreach (var image in KararaAllImage)
        {
            Material mat = image.material;
//            mat.DisableKeyword("SHAKEUV_ON");
//            mat.DisableKeyword("DOODLE_ON");
        }
        //disable map before it's in the tutorial
        SubwayMap.enabled = false;

        leftPosition = kararaDialogueRT.anchoredPosition;

        //set dialogue state
        tutorialDialogueState = DialogueState.none;

        DialogueBubbleAC = DialogueBubble.GetComponentInChildren<Animator>();


    }

    private bool temp1;

    private int time;
    private bool pause;
    // Update is called once per frame
    void Update()
    {
        //decide which dialogue should be shown on screen using the state machine
        if (tutorialDialogueState == DialogueState.fish)
        {
            DialogueBubbleAC.SetBool("isOut", false);
            DoFishDialogue(true);
            DoKararaDialogue(false);
        }
        else if (tutorialDialogueState == DialogueState.karara)
        {
            DoFishDialogue(false);
            DoKararaDialogue(true);
        }
        else if (tutorialDialogueState == DialogueState.fishElse)
        {
            DialogueBubbleAC.SetBool("isOut", true);
            NameTagImage.enabled = false;
            
            DoFishDialogue(true);
            DoKararaDialogue(false);
        }
        else if (tutorialDialogueState == DialogueState.none)
        {
            DoFishDialogue(false);
            DoKararaDialogue(false);
        }
        else if (tutorialDialogueState == DialogueState.all)
        {
            DoFishDialogue(true);
            DoKararaDialogue(true);
        }
        
        
        //don't show fish dialogue if not in fish page
        if (FinalCameraController.myCameraState == FinalCameraController.CameraState.Subway)
        {
            if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Two ||
                FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Three)
            {
                kararaDialogueRT.anchoredPosition = new Vector3(0, -570);
                kararaTextRT.localScale = new Vector3(2f, -2f, 2f);
                kararaDialogueRT.localScale = new Vector3(0.5f, -0.5f, 0.5f);
                
            }
            else if(FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Four)
            {
                kararaDialogueRT.anchoredPosition = leftPosition;
                kararaTextRT.localScale = new Vector3(2f, 2f, 2f);

                kararaDialogueRT.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            }
            else//在第一个画面里永远出现鱼的对话框
            {
                //tutorialDialogueState = DialogueState.fish;//always show fish dialogue in scene1
                screamImage.enabled = false;//no noise image
                //karara dialogue position needs to be adjusted
                kararaDialogueRT.anchoredPosition = rightPosition;
                kararaTextRT.localScale = new Vector3(2f, 2f, 2f);
                kararaDialogueRT.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            }
        }
        else if(FinalCameraController.myCameraState == FinalCameraController.CameraState.Ad)
        {
            //exception is when taking photo in the beginning of the tutorial
            if(!pause )
            {
                tutorialDialogueState = DialogueState.none;

            }
            
        }
        else if(FinalCameraController.myCameraState == FinalCameraController.CameraState.Closet)
        {
            screamImage.enabled = false;
            //在换装界面set好位置
            kararaDialogueRT.anchoredPosition = new Vector2(-80, 600);
            kararaTextRT.localScale = new Vector3(2f, 2f, 2f);
            kararaDialogueRT.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        if (nameTag.text == "Goldfish" || nameTag.text == "?????")
        {
            //print("goldfish");
            if(FinalCameraController.mySubwayState != FinalCameraController.SubwayState.One)
            {
                //DialogueRT.anchoredPosition = lowerPosition;
            }
            else
            {
                //DialogueRT.anchoredPosition = higherPosition;
            }
        }
        else
        {
            //DialogueRT.anchoredPosition = higherPosition;
        }
        

        //make sure that karara only appears at the end and start when player is taught to swipe
//        if (tutorialNumber == 1 && FinalCameraController.myCameraState == FinalCameraController.CameraState.Subway)
//        {
//            if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Four ||
//                FinalCameraController.mySubwayState == FinalCameraController.SubwayState.One)
//            {
//                KararaImage.enabled = true;
//            }
//            else
//            {
//                KararaImage.enabled = false;
//            }
//        }
        
        //disable go back button when it is not needed
        if (tutorialNumber < 12)
        {
            gobackButton.SetActive(false);
        }

        if (tutorialNumber == 2)
        {
//            if (clicktime == 3)
//            {
//                clicktime++;
//                StartCoroutine(KararaAskBag());
//                print("kararaAskBag");
//                
//            }
        }
        else if (tutorialNumber == 3)
        {
            if(clicktime == 3)
            {
                //包已经在洗衣机下面
                tutorialDialogueState = DialogueState.fishElse;
                fishText.text = "Click the bag! Put the cloth into the machine!";
                clicktime = 4;
            }

        }
        else if (tutorialNumber == 4)
        {
            //衣服已经进了洗衣机，还没开始洗
            tutorialDialogueState = DialogueState.fishElse;
            fishText.text = "Click the button to start!";
        }
        else if (tutorialNumber == 5)
        {
            
            //enable sitting
            Show(KararaSittingCanvasGroup);
        }
        else if(tutorialNumber == 6)
        {
            //洗衣机洗好了之后自然出现了这个
            if(clicktime == 4)
            {
                tutorialDialogueState = DialogueState.karara;
                kararaText.text = "Done";
                //ProfileImage.sprite = KararaProfile;
                //arrow.enabled = true;
                clicktime++;
                Show(arrowButton);
            }
//            else if (clicktime == 7 && FinalCameraController.mySubwayState == FinalCameraController.SubwayState.One)
//            {
//                Hide(arrowButton);
//                
//               
//            }
//            else if(clicktime == 8)
//            {
//                if(FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Two)
//                {
//                    tutorialDialogueState = DialogueState.karara;
//                    kararaText.text = "......";
//                    Show(arrowButton);
//                    clicktime = 9;
//                }
//                else
//                {
//                    Hide(arrowButton);
//                }
//            }
        }
        //如果打开了门
        else if(tutorialNumber == 7)
        {
            if(clicktime == 6)
            {
//                if(!temp)
//                {
//                    temp = true;
//                    StartCoroutine(FishCloseDoor());
//                }
//
//                else 
//                {
                    if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.One)
                    {
                        tutorialDialogueState = DialogueState.fishElse;
                        fishText.text = "What are you doing? Close the door!";
                        screamImage.enabled = false;
                        Hide(arrowButton);
                    }
                    else if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Two)
                    {
                        //出现鱼对话制止karara开门，karara对话框消失
                        tutorialDialogueState = DialogueState.karara;
//                        fishText.text = "What are you doing? Close the door!";
                        kararaText.text = "Get cloth";
                        screamImage.enabled = true;
                        Hide(arrowButton);
//                        time++;
//                        if (time > 90)
//                        {
//                            kararaText.text = "Get Cloth";
//                        }
//                        else if(time > 30)
//                        {
//                            kararaText.text = "Nah";
//                            if(temp)
//                            {
//                                kararaText.text = "Nah";
//                            }
//                        }
//                    }
                }
            }
        }
        else if(tutorialNumber == 8)
        {
            tutorialDialogueState = DialogueState.karara;
            kararaText.text = "Open door";
            
            if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.One)
            {
                tutorialDialogueState = DialogueState.fishElse;
                fishText.text = "What are you doing? Close the door!";
                screamImage.enabled = false;
                Hide(arrowButton);
            }
            else if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Two)
            {
                //出现鱼对话制止karara开门，karara对话框消失
                tutorialDialogueState = DialogueState.karara;
                screamImage.enabled = true;
                Hide(arrowButton);
            }
        }
        else if(tutorialNumber == 9)
        {
            kararaText.text = "Get cloth";
            
            if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.One)
            {
                tutorialDialogueState = DialogueState.fishElse;
                fishText.text = "Don't you dare!";
                screamImage.enabled = false;
                Hide(arrowButton);
            }
            else if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Two)
            {
                //出现鱼对话制止karara开门，karara对话框消失
                tutorialDialogueState = DialogueState.karara;
                screamImage.enabled = true;
                Hide(arrowButton);
            }
        }
        else if(tutorialNumber == 10)
        {
            if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.One)
            {
                tutorialDialogueState = DialogueState.fishElse;
                fishText.text = "Karara!";
                screamImage.enabled = false;
                Hide(arrowButton);
            }
            else if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Two)
            {
                //出现鱼对话制止karara开门，karara对话框消失
                tutorialDialogueState = DialogueState.karara;
                screamImage.enabled = true;
                Hide(arrowButton);
            }
        }
        else if(tutorialNumber == 11)
        {
            kararaText.text = "Disco";
        }
        else if(tutorialNumber == 12)
        {
            kararaText.text = "Cool";
            arrow.enabled = true;
            Hide(arrowButton);
        }
        else if(tutorialNumber == 13)
        {
            kararaText.text = "Poster";
            Hide(arrowButton);
            Hide(KararaSittingCanvasGroup);
            KararaStandingImage.enabled = true;
            KararaStandingImage.sprite = KararaDisco;
            //show karara
            KararaRectT.anchoredPosition = 
                new Vector3(-72, KararaRectT.anchoredPosition.y);
            KararaStanding.transform.SetParent(ScreenFour.transform);
//            var RectTransform = KararaStanding.GetComponent<RectTransform>();
//            RectTransform.anchoredPosition = new Vector3(85, RectTransform.anchoredPosition.y);
            KararaStandingImage.enabled = true;
            KararaDisappear(false);

            
        }
        else if(tutorialNumber == 15)
        {
            if(clicktime == 6)
            {
                //show karara
                KararaRectT.anchoredPosition =
                    new Vector3(85, KararaRectT.anchoredPosition.y);
                KararaStanding.transform.SetParent(ScreenZero.transform);
            var RectTransform = KararaStanding.GetComponent<RectTransform>();
            RectTransform.anchoredPosition = new Vector3(85, RectTransform.anchoredPosition.y);
                KararaStandingImage.enabled = true;
                KararaDisappear(false);

                //fish talking
                tutorialDialogueState = DialogueState.fish;
                fishText.text = "What are you doing with your customer's clothes?";
                nameTag.text = "GoldFish";

                Show(arrowButton);
                clicktime++;
            }
        }
        else if(tutorialNumber == 16)
        {
            
            //还好包了
            KararaStandingImage.sprite = KararaWorkCloth;
            
            if(clicktime == 8)
            {
                //如果转到第一个页面，那么鱼老板讲话
                if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.One)
                {
                    tutorialDialogueState = DialogueState.fish;
                    fishText.text = "Lucky you! All clothes are automatically returned!";
                    screamImage.enabled = false;
                    Show(arrowButton);
                    clicktime = 9;
                }
                else
                {
                    screamImage.enabled = true;
                    tutorialDialogueState = DialogueState.none;
                }
            }
          
        }
        else if(tutorialNumber == 18)
        {
            fishText.text = "The end of my day, easy!";
            arrow.enabled = true;
            Show(arrowButton);

//            StartCoroutine("StoryStart");
        }
        
        
        
        //only show Karara when she's in the subway scene
        if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.One ||
            FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Two ||
            FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Three ||
            FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Four)
        {
        }
        else
        {
            Hide(KararaSittingCanvasGroup);
            KararaStandingImage.enabled = false;

        }
        
        
        
        //take a photo scene
        if (FinalCameraController.myCameraState == FinalCameraController.CameraState.Ad)
        {
//            touchImage.enabled = true;
//
//            touch.transform.position = TakeScreenshot.transform.position;
        }
        
        //if the player has already pressed screenshot button
        //only need to do this once?
        if (pressScreenshot)
        {
            pause = true;
            if(tutorialNumber == 0)
            {
//                StartCoroutine(FishCallout());
                //pressScreenshot = false;
                if (temp)
                {
                    DialogueRT.anchoredPosition -= new Vector2(0, 300);
                    temp = false;
                    
                    //wait for a second then show the fish dialogue
                    for (int i = 0; i < 60; i++)
                    {
                        if (i > 58)
                        {
                            tutorialDialogueState = DialogueState.fish;

                            break;
                        }
                    }
                }
                print("pressscreenshot is true, should go a lot of times");

                //this is specifically for the taking picture process, some images should be cancelled
                screamImage.enabled = true;
                NameTagImage.enabled = false;

                fishText.text = "Hey you! Over here!";
                //scrollControl(true);//enable player to swipe

                arrow.enabled = false;

                backToSubway = true;
                Show(arrowButton);
            }
            else if(tutorialNumber > 12)
            {
                DoFishDialogue(false);
                fishText.text = "";
                nameTag.text = "";
                myFlash.alpha = myFlash.alpha - Time.deltaTime;
     
                if (myFlash.alpha <= 0)
                {
                    myFlash.alpha = 0;
                    pressScreenshot = false;
                    KararaDisappear(false);
                }
            }
        }
        
        if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.One && tutorialNumber == 1)
        {
            //don't let the dialogue disappear if swipe
            stopDisappear = true;

            //show fish dialogue
            tutorialDialogueState = DialogueState.fish;
            fishText.text = "Karara! You're finally here!";
            arrow.enabled = true;
            
            //enable click the screen
            Show(arrowButton);

            Hide(GestureCG);
            
            //show karara
            KararaRectT.anchoredPosition = 
                new Vector3(205, KararaRectT.anchoredPosition.y);
            KararaStanding.transform.SetParent(ScreenZero.transform);
            var RectTransform = KararaStanding.GetComponent<RectTransform>();
            RectTransform.anchoredPosition = new Vector3(85, RectTransform.anchoredPosition.y);
            KararaStandingImage.enabled = true;
            KararaDisappear(false);
            
            tutorialNumber = 2;            

            ChangeDialogue("Goldfish");
        }
    }

    public void ChangeDialogue(string Character)
    {
        nameTag.text = Character;
        if (Character == "Karara")
        {
            DialogueBackground.sprite = DialogueKarara;
        }
        else if (Character == "Goldfish" || Character == "?????")
        {
            DialogueBackground.sprite = DialogueFish;

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


    IEnumerator FishCloseDoor()
    {
        yield return new WaitForSeconds(0.4f);
        myHSS.GoToScreen(1);
        yield return new WaitForSeconds(0.4f);
        
    }

    IEnumerator StoryStart()
    {
        yield return new WaitForSeconds(0.4f);

        SceneManager.LoadScene("StreetStyle", LoadSceneMode.Single);
        print("loadScene");
    }
    
    IEnumerator ChangeText(string dialogueText, bool isclick)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        fishText.text = dialogueText;
        if (isclick)
        {
            clicktime++;
        }
    }

   
    public void DoFishDialogue(bool trueOrFalse)
    {
        //disable the dialogues
        for (int a = 0; a < DialogueImageList.Length; a++)
        {
            if(trueOrFalse)
            {
                //if true, enable fish dialogue
                DialogueImageList[a].enabled = true;
                fishText.enabled = true;
                nameTag.enabled = true;
            }
            else
            {
                //if false, disable fish dialogue
                DialogueImageList[a].enabled = false;
                fishText.enabled = false;
                nameTag.enabled = false;
            }
           
        }
    }
    
    public void DoKararaDialogue(bool trueOrFalse)
    {
        //disable the dialogues
        for (int a = 0; a < KararaDialogueImageList.Length; a++)
        {
            if(trueOrFalse)
            {
                //if true, enable karara dialogue
                KararaDialogueImageList[a].enabled = true;
                kararaText.enabled = true;
            }
            else
            {
                //if false, disable karara dialogue
                KararaDialogueImageList[a].enabled = false;
                kararaText.enabled = false;
            }
        }
    }
    IEnumerator TrainMoveIn() 
    {
        yield return new WaitForSeconds(0.5f);
        
        for(int i = 0; i < 5; i ++)
        {
            yield return new WaitForSeconds(0);
            myHSS.GoToScreen(i);

            if (i == 4)
            {
                //karara appears
                yield return new WaitForSeconds(1);

                KararaDisappear(false);                
                yield return new WaitForSeconds(1);

                //show karara dialogue
                tutorialDialogueState = DialogueState.karara;
                kararaText.text = "poster";
                arrow.enabled = false;
                Hide(arrowButton);//player should click poster
            }
        }
    }

    public void scrollControl(bool trueOrFalse)
    {
        //if true, then swipe is enabled
        stopDisappear = !trueOrFalse;
//        myHSS.enabled = trueOrFalse;
//        FinalCameraController.subwayScrollRect.enabled = trueOrFalse;
    }


  
    //true means disappear, like literally
    private void KararaDisappear(bool trueOrFalse)
    {
        var tempColor = KararaStandingImage.color;
        if (trueOrFalse)
        {
            tempColor.a = 0f;
        }
        else if (!trueOrFalse)
        {
            tempColor.a = 1f;
        }
        KararaStandingImage.color = tempColor;
    }

  

   

 

    bool temp = true;

    private bool backToSubway;
    private bool startFadeOut;

    IEnumerator WaitAndChange()
    {
        yield return new WaitForSeconds(0.8f);
        clicktime = 8;

    }
    IEnumerator KararaAskBag()
    {
        yield return new WaitForSeconds(4);

        if(tutorialNumber == 2)
        {
            tutorialDialogueState = DialogueState.karara;
            kararaText.text = "Bag?";
        }

    }
    IEnumerator FishCallout()
    {
        yield return new WaitForSeconds(1);
        tutorialDialogueState = DialogueState.fishElse;

        //this is specifically for the taking picture process
        screamImage.enabled = true;
        fishText.text = "Hey you! Over here!";
        //scrollControl(true);

        //swipe is enabled
        myHSS.enabled = true;
        FinalCameraController.subwayScrollRect.enabled = true;

        arrow.enabled = false;

        if (temp)
        {
            DialogueRT.anchoredPosition -= new Vector2(0, 300);
            temp = false;
        }

        yield return new WaitForSeconds(1);

        backToSubway = true;
        Show(arrowButton);
    }


    IEnumerator MessageAppear()
    {
        tutorialDialogueState = DialogueState.fish;
        fishText.text = "Stop talking! listen, never dare you...";
        yield return new WaitForSeconds(0.5f);
        Hide(arrowButton);
        Show(anotherApp);

    }

    


//        //cancel flash after a few seconds
//        myFlash.alpha = myFlash.alpha - Time.deltaTime;
//     
//        if (myFlash.alpha <= 0)
//        {
////            myFlash.alpha = 0;
////            pressScreenshot = false;
////            Show(GestureCG);
//            backToSubway = true;
//            Show(arrowButton);
//
//
//        }
        
        //change back to subway after the flashlight
        //KararaImage.enabled = true;
//        FinalCameraController.ChangeToSubway();
//        tutorialNumber = 1;
        
        //change dialogue background and name
//        ChangeDialogue("?????");

        //KararaDisappear(true);

   
    public int clicktime = 0;
    private bool clickBool = false;

    private bool tempClick = true;

    public void DialogueArrowButton()
    {
        print("clickedddddddddddddd");
        if (tutorialNumber == 0 && backToSubway)
        {
            if(clicktime == 0)
            {
                backToSubway = false;

                myFlash.alpha = 0;
                pressScreenshot = false;


                Show(GestureCG);
                //screamImage.enabled = true;

                    print("temptemp");
                    DialogueRT.anchoredPosition += new Vector2(0, 250);
                    NameTagImage.enabled = true;

                    //disable fish dialogue
                    tutorialDialogueState = DialogueState.none;
                    print("pressscreenshot is true, should go only one time");
                

                //change back to subway after the flashlight
                FinalCameraController.ChangeToSubway();
                Hide(arrowButton);
                tutorialNumber = 1;
                pause = false;
            }
        }
        else if (tutorialNumber == 2)
        {
            stopDisappear = true;
          if (clicktime == 0)
          {
              tutorialDialogueState = DialogueState.karara;
              kararaText.text = "Karara?";
              
              Show(arrowButton);
              clicktime++;
          }
      
          else if (clicktime == 1)
          {
              tutorialDialogueState = DialogueState.fish;
              fishText.text = "You are already late! Stop talking and do the laundry!";
              clicktime++;
          }
          else if (clicktime == 2)
          {
              tutorialDialogueState = DialogueState.karara;
              kararaText.text = "laundry?";
              clicktime++;
          }
          else if(clicktime == 3)
          {
              tutorialDialogueState = DialogueState.fish;
              fishText.text = "How come you have such a bad memory! Do I need to teach you every time! Now pick up the bag!";
              
              //instantiate the bag
              bag = Instantiate(clothBag, bagPos, Quaternion.identity) as GameObject;
              bag.transform.SetParent(clothBagGroup.transform, false);
              arrow.enabled = false;
              Hide(arrowButton);//so the bag can be picked
          }

          
        }
        else if(tutorialNumber == 6)
        {
            if (clicktime == 5)
            {
                kararaText.text = "Open door";
                Hide(arrowButton);
                clicktime++;
            }
            else if (clicktime == 6)
            {
               
                
//                if(tempClick)
//                {
//                    myHSS.GoToScreen(1);
//                    Hide(arrowButton);
//
//                    tempClick = false;
//                }                
//                tutorialDialogueState = DialogueState.fish;
//                fishText.text = "What are you waiting for! Do the laundry!";
//                StartCoroutine(WaitAndChange());

            }
            
        }
        else if (tutorialNumber == 7)
        {
            //这里主要需要通过点击内容推进对话，不通过clicktime
            
        }
        else if(tutorialNumber == 15)
        {
            if (clicktime == 7)
            {
                fishText.text = "Now return the clothes! Immediately!";
                arrow.enabled = false;
                Show(arrowButton);
                clicktime++;
            }
            else if (clicktime == 8)
            {
                myHSS.GoToScreen(2);
                Hide(arrowButton);
                //clicktime++;
            }
//            else if (clicktime == 10)
//            {
//                fishText.text = "What are you murmuring? Quiet! Always remember time! Check the map if you need to!";
//                nameTag.text = "Goldfish";
//                
//                //enable map button
//                SubwayMap.enabled = true;
//                Hide(arrowButton);
//
//                clicktime++;
//            }
//            else if (clicktime == 11)
//            {
//                //镜头转到map
//                
//                FinalCameraController.mySubwayState = FinalCameraController.SubwayState.Three;
//                myHSS.GetComponent<HorizontalScrollSnap>().GoToScreen(3);
//                //stopDisappear = true;
//                
//                clicktime++;
//            }
        }
        else if(tutorialNumber == 16)
        {
            if(clicktime == 8)
            {
                tutorialDialogueState = DialogueState.fish;
                fishText.text = "Lucky you! All clothes are automatically returned!";
                clicktime++;
            }
            else if (clicktime == 9)
            {
                tutorialDialogueState = DialogueState.karara;
                kararaText.text = "Pity";
                clicktime++;
            }
            else if (clicktime == 10)
            {
                StartCoroutine(MessageAppear());
            }
           
           
        }
        else if (tutorialNumber == 18)
        { 
            print("startSceneeeee");
            StartCoroutine(StoryStart());
        }
    }
}

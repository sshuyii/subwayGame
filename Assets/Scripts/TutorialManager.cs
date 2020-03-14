using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.SceneManagement;



public class TutorialManager : MonoBehaviour
{
    public GameObject DialogueBubble;

    public FinalCameraController FinalCameraController;

    public GameObject ClothUI;
    private Button[] ClothUIButtons;
    public Image[] ClothUIImages;


    public bool chooseBag = false;
    public GameObject ScreenZero;
    
    private HorizontalScrollSnap myHSS;
    public Image[] DialogueImageList;
    public TextMeshProUGUI myText;
    public Image arrow;
    public CanvasGroup arrowButton;
    
    public GameObject poster;

    public bool wearNothing;
   
    public GameObject TakeScreenshot;


    public GameObject KararaStanding;
    public GameObject KararaSitting;
    
    public Image KararaStandingImage;
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

    private Image bagImage;
    private Image doorImage;
    private Image clothImage;


    public GameObject gobackButton;
    public Button KararaButton;

    private Color fishColor;
    private Color kararaColor;

    
    // Start is called before the first frame update
    void Start()
    {
        fishColor = new Color(176f/255f, 140f/255f, 84f/255f);
        
        myHSS = GameObject.Find("Horizontal Scroll Snap").GetComponent<HorizontalScrollSnap>();
        
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();

        KararaStandingImage = KararaStanding.GetComponent<Image>();
        KararaSittingCanvasGroup = KararaSitting.GetComponent<CanvasGroup>();


        StartCoroutine("TrainMoveIn");

        KararaRectT = KararaStanding.GetComponent<RectTransform>();
        
        DialogueImageList = DialogueBubble.GetComponentsInChildren<Image>();
        myText = DialogueBubble.GetComponentInChildren<TextMeshProUGUI>();

        
        //disable the dialogues
        DoDialogues(false);

        //disable karara standing in the train
//        KararaStandingImage.enabled = false;
            KararaDisappear(false);

        ClothUIButtons = ClothUI.GetComponentsInChildren<Button>();
        ClothUIImages = ClothUI.GetComponentsInChildren<Image>();
        
        doorImage = door.GetComponent<Image>();
        clothImage = cloth.GetComponent<Image>();

        ProfileImage.enabled = false;

        

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
            mat.DisableKeyword("SHAKEUV_ON");
            mat.DisableKeyword("DOODLE_ON");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        
        
        if (tutorialNumber == 4)
        {
            myText.text = "Click the button to Start!";
            arrow.enabled = false;
            Hide(arrowButton);
            //bag.GetComponent<Image>().material.DisableKeyword("SHAKEUV_ON");


        }
        else if (tutorialNumber == 3)
        {
            myText.text = "Put clothes into the machine!";
            arrow.enabled = false;
            Hide(arrowButton);
            
        }
        else if (tutorialNumber == 5)
        {
            myText.text = "Now all you need to do is wait...";
            //disable standing 
            
            //enable sitting
            Show(KararaSittingCanvasGroup);
        }
        else if(tutorialNumber == 6)
        {
            myText.text = "It is finished, should I take the clothes out?";
            ProfileImage.sprite = KararaProfile;
            tutorialNumber = 7;
            arrow.enabled = true;
            Show(arrowButton);

            
            //change dialogue background and name
            ChangeDialogue("Karara");
        }
        else if(tutorialNumber == 8)
        {
            StartCoroutine(WaitUntilUI());
        }
        else if(tutorialNumber == 9)
        {
            if (chooseBag)
            {
                myText.text = "I wish I have them in my wardrobe...Do I have to return those immediately?";

                arrow.enabled = true;
                Show(arrowButton);
                bag.GetComponent<Image>().material.DisableKeyword("SHAKEUV_ON");
                chooseBag = false;
            }
            else if (clicktime == 7)
            {
//                myText.text = "Now I'm supposed to put them into the bag...Just click the bag.";
//                arrow.enabled = false;

            }
        }
        else if(tutorialNumber == 10)
        {
            myText.text = "Well, I guess since no one is watching... I should try it on!";
            //tutorialNumber = 11;

            foreach (var image in KararaAllImage)
            {
                Material mat = image.material;
                //mat.EnableKeyword("SHAKEUV_ON");
                mat.EnableKeyword("DOODLE_ON");
            }
            KararaButton.enabled = true;
            arrow.enabled = false;
            Hide(arrowButton);
            
            bag.GetComponent<Image>().material.DisableKeyword("SHAKEUV_ON");

        }
        else if(tutorialNumber == 11)
        {
            myText.text = "Now click the cloth to put it on.";
        }
        else if(tutorialNumber == 12)
        {
            myText.text = "How do I look! I should take a picture!";
            arrow.enabled = true;
            Hide(arrowButton);

            tutorialNumber = 13;
        }
        else if(tutorialNumber == 13)
        {
            foreach (var image in KararaAllImage)
            {
                Material mat = image.material;
                //mat.EnableKeyword("SHAKEUV_ON");
                mat.DisableKeyword("DOODLE_ON");
            }
        }
        else if(tutorialNumber == 14 && FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Four)
        {
            myText.text = "Oh here it is!";
            tutorialNumber = 15;
        }
        else if(tutorialNumber == 16)
        {
            DoDialogues(true);
            myText.text = "The clothes should be returned before ";
            arrow.enabled = false;
            Hide(arrowButton);



            nameTag.text = "Karara";
            bag.GetComponent<Image>().material.EnableKeyword("SHAKEUV_ON");
        }
        else if(tutorialNumber == 17)
        {
            myText.text = "The end of my day, easy!";
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
        if (pressScreenshot)
        {
            //wait for several seconds
//            StartCoroutine(WaitBeforeDialogues(2, "Hey you! Come over here!", FishProfile));
            if(tutorialNumber == 0)
            {
                StartCoroutine(FishCallout());
            }
            else if(tutorialNumber > 12)
            {
                DoDialogues(false);
                myText.text = "";
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
            myText.text = "Your first day of work!";
            
            arrow.enabled = true;
            Show(arrowButton);


            KararaRectT.anchoredPosition = 
                new Vector3(205, KararaRectT.anchoredPosition.y);
            tutorialNumber = 2;
            StartCoroutine(WaitFor1Seconds());
            
            KararaStanding.transform.SetParent(ScreenZero.transform);
            var RectTransform = KararaStanding.GetComponent<RectTransform>();
            RectTransform.anchoredPosition = new Vector3(85, RectTransform.anchoredPosition.y);
            KararaStandingImage.enabled = true;
            KararaDisappear(false);
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

    IEnumerator StoryStart()
    {
        yield return new WaitForSeconds(0.4f);

        SceneManager.LoadScene("StreetStyle", LoadSceneMode.Single);
        print("loadScene");
    }
    
    IEnumerator ChangeText(string dialogueText, bool isclick)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        myText.text = dialogueText;
        if (isclick)
        {
            clicktime++;
        }

    }

    public void DoDialogues(bool trueOrFalse)
    {
        //disable the dialogues
        for (int a = 0; a < DialogueImageList.Length; a++)
        {
            if(trueOrFalse)
            {
                DialogueImageList[a].enabled = true;
            }
            else
            {
                DialogueImageList[a].enabled = false;
            }
        }
    }
    IEnumerator TrainMoveIn() 
    {
        yield return new WaitForSeconds(1);
        
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

                for (int a = 0; a < DialogueImageList.Length; a++)
                {
                    DialogueImageList[a].enabled = true;
                    myText.text = "I should take a selfie in front of that poster";
                    nameTag.text = "Karara";
                    arrow.enabled = false;
                    Hide(arrowButton);


                    //myText.color = fishColor;

//                    ProfileImage.sprite = FishProfile;
                }
                scrollControl(false);

//                touch.transform.position = poster.transform.position;
//                touchImage.enabled = true;
//                touchAnimator.SetBool("isTouch", true);
                //change dialogue background and name
                ChangeDialogue("Karara");

            }
        }
    }

    public void scrollControl(bool trueOrFalse)
    {
        myHSS.enabled = trueOrFalse;
        FinalCameraController.subwayScrollRect.enabled = trueOrFalse;
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

    //wait for several seconds
    IEnumerator WaitBeforeDialogues(int seconds, string dialogue, Sprite profile)
    {
        yield return new WaitForSeconds(seconds);
        DoDialogues(true);
        myText.text = dialogue;
        ProfileImage.sprite = profile;

    }

    IEnumerator WaitFor1Seconds()
    {
        yield return new WaitForSeconds(1);
        scrollControl(false);
    }

    IEnumerator WaitUntilUI()
    {
        tutorialNumber = 9;

        yield return new WaitForSeconds(2);
        myText.text = "Wow, this is beautiful.";
//        //这时还不能点衣服
//        foreach (var button in ClothUIButtons)
//        {
//            button.enabled = false;
//        }

        arrow.enabled = true;
        Show(arrowButton);

    }
    
    IEnumerator FishCallout()
    {
        yield return new WaitForSeconds(1);
        DoDialogues(true);
        
        myText.text = "Hey you! Come over here! Swipe to move around!";
        scrollControl(true);

        ProfileImage.sprite = FishProfile;
        arrow.enabled = false;
        Hide(arrowButton);

        
        yield return new WaitForSeconds(1);

        //cancel flash after a few seconds
        myFlash.alpha = myFlash.alpha - Time.deltaTime;
     
        if (myFlash.alpha <= 0)
        {
            myFlash.alpha = 0;
            pressScreenshot = false;
        }
        
        //change back to subway after the flashlight
        //KararaImage.enabled = true;
        FinalCameraController.ChangeToSubway();
        tutorialNumber = 1;
        
        //change dialogue background and name
        ChangeDialogue("?????");

        //KararaDisappear(true);

    }


    IEnumerator ClickTimeOne()
    {
        yield return new WaitForSeconds(0.5f);

        myText.text = "Now pick up those bags.";

        bag = Instantiate(clothBag, bagPos, Quaternion.identity) as GameObject;
        bag.transform.SetParent(clothBagGroup.transform, false);
        arrow.enabled = false;
        Hide(arrowButton);

                
        bag.GetComponent<Image>().material.EnableKeyword("SHAKEUV_ON");

    }

    public int clicktime = 0;
    private bool clickBool = false;

    public void DialogueArrowButton()
    {
        print("clickedddddddddddddd");
        if (tutorialNumber == 2)
        {
            arrow.enabled = true;
            Show(arrowButton);

            if(clicktime == 0)
            {
                StartCoroutine(ChangeText("Good that you are already wearing the workwear.",true));
                 
            }
            else if (clicktime == 1)
            {
                //StartCoroutine(ChangeText("Now pick up those bags.",false));

                //myText.text = "Now pick up those bags.";
                StartCoroutine(ClickTimeOne());

            }
        }
        else if (tutorialNumber == 7)
        {
         
            if (clicktime == 1)
            {
                myText.text = "Boss?";
                ProfileImage.sprite = KararaProfile;
                
                clicktime++;

            }
            else if (clicktime == 2)
            {
                myText.text = "......";
                ProfileImage.sprite = FishProfile;
                
                clicktime++;

            }
            else if (clicktime == 3)
            {
                myText.text = "Hey you! Come over here!";
                
                clicktime++;
                
                //change dialogue background and name
                ChangeDialogue("Goldfish");

            }
            else if (clicktime == 4)
            {
                myText.text = "Good that you are already wearing the workwear.";
                
                clicktime++;

            }
            else if (clicktime == 5)
            {
                myText.text = "He has forgotten me, guess I'll do this on my own. First, open the door.";
                ProfileImage.sprite = KararaProfile;
                //touch.transform.position = door.transform.position;

                door.GetComponent<Image>().material.EnableKeyword("SHAKEUV_ON");
                cloth.GetComponent<Image>().material.EnableKeyword("SHAKEUV_ON");
                
                cloth.GetComponent<Button>().enabled = true;
                arrow.enabled = false;
                Hide(arrowButton);

                
                //change dialogue background and name
                ChangeDialogue("Karara");
            }
        }

        else if(tutorialNumber == 9)
        {
            if (clicktime == 5)
            {
                //这时能点衣服
                foreach (var button in ClothUIButtons)
                {
                    button.enabled = true;
                }
               
                foreach (var image in ClothUIImages)
                {
                    Material mat = image.material;
                    mat.EnableKeyword("SHAKEUV_ON");
                    mat.EnableKeyword("DOODLE_ON");
                    
                }

                myText.text = "I'm supposed to put them back into the bag...Just click the bag.";

                bag.GetComponent<Image>().material.EnableKeyword("SHAKEUV_ON");

                arrow.enabled = false;
                Hide(arrowButton);
                
                clicktime++;


//                touch.transform.position = cloth.transform.position;
//                touchImage2.enabled = true;
//                touch2.transform.position = bag.transform.position;
            }
            else if (clicktime == 7)
            {
                myText.text = "Boss is not paying attention. He doesn't remember things anyway. Maybe I should...?";
                arrow.enabled = false;
                Hide(arrowButton);
            }
        }
        else if(tutorialNumber == 13)
        {
            if (wearNothing)
            {
                myText.text = "Underwear? At least I should wear something on my picture...";

            }
            else
            {
                FinalCameraController.ChangeToSubway();
                myText.text = "I should be quick. Where is that poster?";
                tutorialNumber = 14;
                arrow.enabled = false;
                Hide(arrowButton);
                
                //disable player button, now if clicked, nothing would happen, cannot go back to closet scene
                //maybe don't disable the button, but instead karara say something and refuse to go back, achieved in FinalCameraController
                //KararaSittingCanvasGroup.interactable = false;

            }
           
        }
        else if (tutorialNumber == 17)
        { 
            print("startSceneeeee");
            StartCoroutine(StoryStart());
        }
    }
}

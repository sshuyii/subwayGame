using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;


public class TutorialManager : MonoBehaviour
{
    public Image KararaImage;
    public GameObject DialogueBubble;

    public FinalCameraController FinalCameraController;
    
    private HorizontalScrollSnap myHSS;
    public Image[] DialogueImageList;
    public TextMeshProUGUI myText;
    public GameObject poster;

    public GameObject touch;
    public GameObject touch2;

    public Image touchImage;
    public Image touchImage2;
    public GameObject TakeScreenshot;

    public Animator touchAnimator;

    public GameObject KararaObject;
    public Image ProfileImage;
    public Sprite KararaProfile;
    public Sprite FishProfile;

    public bool pressScreenshot;
    public CanvasGroup myFlash;
    public bool isFishTalking;
    public bool isPrewash;

    public int tutorialNumber;

    public GameObject clothBag;
    public Vector2 bagPos;
    public GameObject clothBagGroup;

    private RectTransform KararaRectT;
    public CanvasGroup KararaSubway;

    public GameObject bag;
    public GameObject door;
    public GameObject cloth;
    
    // Start is called before the first frame update
    void Start()
    {
        myHSS = GameObject.Find("Horizontal Scroll Snap").GetComponent<HorizontalScrollSnap>();
        
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();


        StartCoroutine("TrainMoveIn");

        KararaRectT = KararaObject.GetComponent<RectTransform>();
        
        DialogueImageList = DialogueBubble.GetComponentsInChildren<Image>();
        myText = DialogueBubble.GetComponentInChildren<TextMeshProUGUI>();

        
        //disable the dialogues
        DoDialogues(false);

        //disable karara standing in the train
        KararaImage.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (tutorialNumber == 4)
        {
            myText.text = "Click Start!";
        }
        else if (tutorialNumber == 3)
        {
            myText.text = "Put clothes into the machine!";
        }
        else if (tutorialNumber == 5)
        {
            myText.text = "Now all you need to do is wait...";
            KararaImage.enabled = false;
            Show(KararaSubway);
        }
        else if(tutorialNumber == 6)
        {
            myText.text = "Now it is finished, should I take the clothes out?";
            ProfileImage.sprite = KararaProfile;
            tutorialNumber = 7;
        }
        else if(tutorialNumber == 8)
        {
            myText.text = "Wow, this is beautiful.";
            tutorialNumber = 9;

        }
        else if(tutorialNumber == 10)
        {
            myText.text = "Well, since no one is watching...";
            tutorialNumber = 11;

        }
        else if(tutorialNumber == 12)
        {
            myText.text = "This looks good on me! I should take a picture!";
            tutorialNumber = 13;

        }
        else if(tutorialNumber == 14 && FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Four)
        {
            myText.text = "Oh here it is!";
            tutorialNumber = 15;
        }
        else if(tutorialNumber == 16)
        {
            DoDialogues(true);
            myText.text = "If I click the bag, all clothes will be automatically returned.";
        }
        else if(tutorialNumber == 17)
        {
            myText.text = "The end of my day, easy!";

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
            KararaImage.enabled = false;
        }
        
        
        
        //take a photo scene
        if (FinalCameraController.myCameraState == FinalCameraController.CameraState.Ad)
        {
            touchImage.enabled = true;

            touch.transform.position = TakeScreenshot.transform.position;
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
            else
            {
                DoDialogues(false);
                myText.text = "";
                myFlash.alpha = myFlash.alpha - Time.deltaTime;
     
                if (myFlash.alpha <= 0)
                {
                    myFlash.alpha = 0;
                    pressScreenshot = false;
                    Show(KararaSubway);
                }
            }
        }
        
        if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.One && tutorialNumber == 1)
        {
            myText.text = "Your first day of work!";
            
            KararaImage.enabled = true;
            KararaRectT.anchoredPosition = 
                new Vector3(205, KararaRectT.anchoredPosition.y);
            tutorialNumber = 2;
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
        yield return new WaitForSeconds(5);
        
        for(int i = 0; i < 4; i ++)
        {
            yield return new WaitForSeconds(0);
            myHSS.GoToScreen(i);

            if (i == 3)
            {
                //karara appears
                yield return new WaitForSeconds(1);

                KararaImage.enabled = true;
                
                yield return new WaitForSeconds(1);

                for (int a = 0; a < DialogueImageList.Length; a++)
                {
                    DialogueImageList[a].enabled = true;
                    myText.text = "I should take a selfie in front of that poster";
//                    ProfileImage.sprite = FishProfile;
                }

                touch.transform.position = poster.transform.position;
                touchImage.enabled = true;
                touchAnimator.SetBool("isTouch", true);
                
            }
        }
        
    }


    //wait for several seconds
    IEnumerator WaitBeforeDialogues(int seconds, string dialogue, Sprite profile)
    {
        yield return new WaitForSeconds(seconds);
        DoDialogues(true);
        myText.text = dialogue;
        ProfileImage.sprite = profile;

    }

    IEnumerator FishCallout()
    {
        yield return new WaitForSeconds(1);
        DoDialogues(true);
        myText.text = "Hey you! Come over here! Swipe to move around!";
        ProfileImage.sprite = FishProfile;
        
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
        
    }

    
    int clicktime = 0;
    private bool clickBool = false;

    public void DialogueArrowButton()
    {
        print("aaaaaa" + clicktime);

        if (tutorialNumber == 2)
        {
            if(clicktime == 0)
            {
                myText.text = "Good that you are already wearing the workcloth.";
                clicktime++;

            }
            else if (clicktime == 1)
            {
                myText.text = "Now pick up those bags.";
                bag = Instantiate(clothBag, bagPos, Quaternion.identity) as GameObject;
                bag.transform.SetParent(clothBagGroup.transform, false);

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

            }
            else if (clicktime == 4)
            {
                myText.text = "Good that you are already wearing the workcloth.";
                
                clicktime++;

            }
            else if (clicktime == 5)
            {
                myText.text = "He has forgotten me, guess I'll do this on my own. First, open the door.";
                ProfileImage.sprite = KararaProfile;
                touch.transform.position = door.transform.position;
                
            }
            
        }
        else if(tutorialNumber == 9)
        {
            if (clicktime == 5)
            {
                myText.text = "Now I'm supposed to put them into the bag...";
                clicktime++;
                touch.transform.position = cloth.transform.position;
                touchImage2.enabled = true;
                touch2.transform.position = bag.transform.position;
            }
        }
        else if(tutorialNumber == 13)
        {
            FinalCameraController.ChangeToSubway();
            myText.text = "I should be quick. Where is that poster?";
            tutorialNumber = 14;
        }

       
        
    }
    



}

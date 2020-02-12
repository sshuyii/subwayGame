using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[System.Serializable]

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

    public Image touchImage;
    public GameObject TakeScreenshot;

    public Animator touchAnimator;

    public Image ProfileImage;
    public Sprite KararaProfile;
    public Sprite FishProfile;

    public bool pressScreenshot;
    public CanvasGroup myFlash;
    
    // Start is called before the first frame update
    void Start()
    {
        myHSS = GameObject.Find("Horizontal Scroll Snap").GetComponent<HorizontalScrollSnap>();
        
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();


        StartCoroutine("TrainMoveIn");

       
        
        DialogueImageList = DialogueBubble.GetComponentsInChildren<Image>();
        myText = DialogueBubble.GetComponentInChildren<TextMeshProUGUI>();

        
        //disable the dialogues
        DoDialogues(false);

    }

    // Update is called once per frame
    void Update()
    {

        //take a photo scene
        if (FinalCameraController.myCameraState == FinalCameraController.CameraState.Ad)
        {
            touchImage.enabled = true;

            touch.transform.position = TakeScreenshot.transform.position;
        }
        
        if (pressScreenshot)
        {
            //wait for several seconds
//            StartCoroutine(WaitBeforeDialogues(2, "Hey you! Come over here!", FishProfile));
            StartCoroutine(FishCallout());


            

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
        myText.text = "Hey you! Come over here!";
        ProfileImage.sprite = FishProfile;
        
        yield return new WaitForSeconds(3);

        //cancel flash after a few seconds
        myFlash.alpha = myFlash.alpha - Time.deltaTime;
     
        if (myFlash.alpha <= 0)
        {
            myFlash.alpha = 0;
            pressScreenshot = false;
        }
        
        //change back to subway after the flashlight
        FinalCameraController.ChangeToSubway();
    }
    



}

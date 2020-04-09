using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBackground :MonoBehaviour
{
    //public List<Sprite> subwayAdList;


    private InstagramController InstagramController;
    private FinalCameraController FinalCameraController;

    private TutorialManager TutorialManager;
    public Image photoBackground;

    private Button myButton;

    
    //public Dictionary<string, Sprite> allSubAd = new Dictionary<string, Sprite>();


    
    // Start is called before the first frame update
    void Start()
    {
//        for (int i = 0; i < subwayAdList.Count; i++)
//        {
//            allSubAd.Add(subwayAdList[i].name, subwayAdList[i]);
//        }

        InstagramController = GameObject.Find("---InstagramController").GetComponent<InstagramController>();
        
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();
        if(FinalCameraController.isTutorial)
        {
            TutorialManager = GameObject.Find("---TutorialManager").GetComponent<TutorialManager>();
        }
        myButton = GetComponent<Button>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickBackground()
    {
        InstagramController.currentBackground = gameObject.name;
        //reset everything to the first posture being able to use in this background
        if(FinalCameraController.alreadyClothUI == false)
        {
            if(FinalCameraController.isSwipping == false && !FinalCameraController.isTutorial)
            {
                InstagramController.CalculateInventory.resetPosture();
    
                FinalCameraController.CancelAllUI();
                photoBackground.sprite = InstagramController.allBackAd[transform.name];
                InstagramController.currentBackground = transform.name;
    
                FinalCameraController.GoAdvertisement();
            }
            else if (FinalCameraController.isSwipping == false && FinalCameraController.isTutorial)
            {
//              if (InstagramController.AdAlreadyTakenList[transform.name])
//              {
                if (FinalCameraController.TutorialManager.tutorialNumber == 0)
                {                                                                                                                                                                     
                    //cancel dialogues and the touch tutorial
                    //TutorialManager.tutorialDialogueState = TutorialManager.DialogueState.none;
                    
                     
                    photoBackground.sprite = InstagramController.allBackAd[transform.name];
                    InstagramController.currentBackground = transform.name;

                    FinalCameraController.GoAdvertisement();
                }
                else if (FinalCameraController.TutorialManager.tutorialNumber == 2 || FinalCameraController.TutorialManager.tutorialNumber == 3)
                {
                    TutorialManager.fishText.text = "Don't touch it! Come over here!";
                    TutorialManager.KararaStandingImage.enabled = true;
                }
                else if (FinalCameraController.TutorialManager.tutorialNumber == 13)
                {
                    photoBackground.sprite = InstagramController.allBackAd[transform.name];
                    InstagramController.currentBackground = transform.name;

                    FinalCameraController.GoAdvertisement();
                }

//              }
            }
        }
        else
        {
            Destroy(FinalCameraController.generatedNotice);
            Destroy(FinalCameraController.currentClothUI);
            FinalCameraController.alreadyClothUI = false;

        }
        
    }
}

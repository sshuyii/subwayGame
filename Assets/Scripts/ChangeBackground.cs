using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeBackground : MonoBehaviour
{
    //public List<Sprite> subwayAdList;


    private InstagramController InstagramController;
    private FinalCameraController FinalCameraController;

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

        myButton = GetComponent<Button>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void clickBackground()
    {

        if(!FinalCameraController.alreadyClothUI)
        {
            if (FinalCameraController.isSwipping == false)
            {
                if (InstagramController.AdAlreadyTakenList[transform.name])
                {
                    photoBackground.sprite = InstagramController.allBackAd[transform.name];
                    InstagramController.currentBackground = transform.name;

                    FinalCameraController.GoAdvertisement();
                }
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

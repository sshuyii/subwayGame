using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstagramButtons : MonoBehaviour
{
    private Sprite currentSprite;

    private Image myImage;
    private Button myButton;

    private InstagramController InstagramController;
    private CalculateInventory CalculateInventory;
    private Text username;

    private FinalCameraController FinalCameraController;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentSprite = GetComponent<Image>().sprite;
        username = GetComponentInChildren<Text>();
        
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();

        myImage = GetComponent<Image>();
        myButton = GetComponent<Button>();


        InstagramController = GameObject.Find("---InstagramController").GetComponent<InstagramController>();
        CalculateInventory = GameObject.Find("---InventoryController").GetComponent<CalculateInventory>();

    }

    // Update is called once per frame
    void Update()
    {
        if (myImage.sprite == InstagramController.transparent)
        {
            myButton.enabled = false;
           
        }
        else
        {
            myButton.enabled = true;
        }
            
    }

    public void ChangeToPersonalPage()
    {
        FinalCameraController.lastAppState = FinalCameraController.myAppState;

        Hide(FinalCameraController.frontPage);
        FinalCameraController.HideAllPersonalPages();

        
        if (username.text == "Karara")
        {
            Camera.main.transform.position =
                new Vector3(45, Camera.main.transform.position.y, Camera.main.transform.position.z);
            
            FinalCameraController.myAppState = FinalCameraController.AppState.KararaPage;
            Show(FinalCameraController.KararaPage);
            

        }    
        else if (username.text == "Nico")
        {
            Camera.main.transform.position =
                new Vector3(55, Camera.main.transform.position.y, Camera.main.transform.position.z);
            FinalCameraController.myAppState = FinalCameraController.AppState.RetroPage;
            Show(FinalCameraController.RetroPage);


        }       
        else if (username.text == "Alex")
        {
            Camera.main.transform.position =
                new Vector3(55, Camera.main.transform.position.y, Camera.main.transform.position.z);
            FinalCameraController.myAppState = FinalCameraController.AppState.DesignerPage;
            Show(FinalCameraController.DesignerPage);


        }       
    }
    
    public void ClickInsPost()
    {
        FinalCameraController.lastAppState = FinalCameraController.myAppState;

        Hide(FinalCameraController.frontPage);
        FinalCameraController.HideAllPersonalPages();
        
        Show(FinalCameraController.postpage);
        
        Camera.main.transform.position = new Vector3(65, Camera.main.transform.position.y, Camera.main.transform.position.z);

        InstagramController.gotoImage.sprite = currentSprite;
        
        FinalCameraController.myAppState = FinalCameraController.AppState.Post;
        
        for (int i = 0; i < InstagramController.retroPostList.Count; i++)
        {
            if (InstagramController.retroPostList[i] == currentSprite)
            {
                InstagramController.gotoProfile.sprite = InstagramController.allProfile["nico"];
                InstagramController.gotoText.GetComponent<Text>().text = "nico";
            }
        }
        
        for (int i = 0; i < InstagramController.designerPostList.Count; i++)
        {
            if (InstagramController.designerPostList[i] == currentSprite)
            {
                InstagramController.gotoProfile.sprite = InstagramController.allProfile["ojisan"];
                InstagramController.gotoText.GetComponent<Text>().text = "ojisan";
            }
        }

       
    }

    public void ChangePosture()
    {

        if(CalculateInventory.posNum < 2)
        {
            CalculateInventory.posNum++;
        }
        else
        {
            CalculateInventory.posNum = 0;
        }
        
        CalculateInventory.allAdCloth = CalculateInventory.postureDictionaryList[CalculateInventory.posNum];

        
        InstagramController.adBodyImage.sprite = InstagramController.postureList[CalculateInventory.posNum];
        InstagramController.workClothImage.sprite = InstagramController.workclothList[CalculateInventory.posNum];
        
        //change clothes after posture changes
        for(int i = 0; i < InstagramController.adClothes.Count; i ++)
        {
            if (InstagramController.adClothes[i].sprite.name.Contains("Top"))
            {
                CalculateInventory.topASR.sprite = CalculateInventory.allAdCloth[InstagramController.adClothes[i].sprite.name];
                print("change top ad");
            }
            else if (InstagramController.adClothes[i].sprite.name.Contains("Bottom"))
            {
                CalculateInventory.otherASR.sprite = CalculateInventory.allAdCloth[InstagramController.adClothes[i].sprite.name];

            }
            else if (InstagramController.adClothes[i].sprite.name.Contains("Shoe"))
            {
                CalculateInventory.shoeASR.sprite = CalculateInventory.allAdCloth[InstagramController.adClothes[i].sprite.name];

            }
            else if (InstagramController.adClothes[i].sprite.name.Contains("Everything"))
            {
                CalculateInventory.everythingASR.sprite = CalculateInventory.allAdCloth[InstagramController.adClothes[i].sprite.name];

            }
            else
            {
                print("nothing ad");

            }
        }

        for (int i = 0; i < InstagramController.adClothes.Count; i++)
        {
            print(InstagramController.adClothes[i].name);
        }

        
        
        
//        for (int i = 0; i < InstagramController.postureList.Count; i++)
//        {
//            print("InstagramController.postureList.Count = " + InstagramController.postureList.Count);
//                if (myImage.sprite == InstagramController.postureList[i])
//                {
//                    //if not the last pose
//
//                    if (i != InstagramController.postureList.Count - 1)
//                    {
//                        myImage.sprite = InstagramController.postureList[i + 1];
//                        CalculateInventory.posNum = i + 1;
//                        CalculateInventory.allAdCloth = CalculateInventory.postureDictionaryList[CalculateInventory.posNum];
//                        print("posNum = " + CalculateInventory.posNum);
//                        break;
//                    }
//                    else
//                    {
//                        myImage.sprite = InstagramController.postureList[0];
//                        CalculateInventory.posNum = 1;
//                        CalculateInventory.allAdCloth = CalculateInventory.postureDictionaryList[CalculateInventory.posNum];
//
//                        break;
//                    }
//                }
//        }
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

    
}

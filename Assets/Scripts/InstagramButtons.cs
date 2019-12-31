using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InstagramButtons : MonoBehaviour
{
    private Sprite currentSprite;

    private Image myImage;
    private Button myButton;

    private InstagramController InstagramController;
    private CalculateInventory CalculateInventory;
    private Text username;

    private FinalCameraController FinalCameraController;
    private int myBackgroundName;
    
    
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

        if(GetComponent<RecordBackgroundPosture>() != null)
        {
            myBackgroundName = this.GetComponent<RecordBackgroundPosture>().backgroundName;
        }
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
        //destroy all previous generated comments
        foreach (Transform child in InstagramController.postReplyParent.transform) {
            GameObject.Destroy(child.gameObject);
        }
        
        FinalCameraController.lastAppState = FinalCameraController.myAppState;

        Hide(FinalCameraController.frontPage);
        FinalCameraController.HideAllPersonalPages();
        
        Show(FinalCameraController.postpage);
        
        Camera.main.transform.position = new Vector3(65, Camera.main.transform.position.y, Camera.main.transform.position.z);

        InstagramController.gotoImage.sprite = currentSprite;
        
        FinalCameraController.myAppState = FinalCameraController.AppState.Post;
        
        //if it is karara's page
        if (FinalCameraController.lastAppState == FinalCameraController.AppState.KararaPage)
        {
            InstagramController.gotoProfile.sprite = InstagramController.allProfile["karara"];
            InstagramController.gotoText.GetComponent<Text>().text = "karara";
            //change post text according to background
            if (myBackgroundName == 0)
            {
                InstagramController.postText.text =
                    "<b>Karara</b> That RV looks amazing. I'm gonna get one for myself.";
            }
            else if (myBackgroundName == 1)
            {
                InstagramController.postText.text =
                    "<b>Karara</b> Fresh and tasty.";
            }
            else if (myBackgroundName == 2)
            {
                InstagramController.postText.text = "<b>Karara</b> Amusement parks nowadays are so boring.";
            }
        }
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
        
        
        
        
        //change post text and comments
        if (currentSprite.name == "designDoc")
        {
            InstagramController.postText.text =
                "<b>Ojisan</b> Just found this sketch from the bottom of my bookshelf. I doubt the design might already be outdated nowadays.";
            var reply1 = Instantiate(InstagramController.commentPrefab, new Vector3(0, 0, 0), Quaternion.identity, InstagramController.postReplyParent.transform);
            reply1.GetComponentInChildren<Image>().sprite = InstagramController.NPCspriteList[1];
            var replyText1 = reply1.GetComponentInChildren<TextMeshProUGUI>();
            replyText1.text = "Not at all! I like this design :)";

        }
        
        else if(currentSprite.name == "Gallery")
        {
            InstagramController.postText.text =
                "<b>Nico</b> Who took this picture of me???";
            var reply1 = Instantiate(InstagramController.commentPrefab, new Vector3(0, 0, 0), Quaternion.identity, InstagramController.postReplyParent.transform);
            reply1.GetComponentInChildren<Image>().sprite = InstagramController.NPCspriteList[3];
            var replyText1 = reply1.GetComponentInChildren<TextMeshProUGUI>();
            replyText1.text = "Let me guess!";

        }
        else if(currentSprite.name == "icecream")
        {
            InstagramController.postText.text =
                "<b>Nico</b> Nightmare. I screamed.";
            var reply1 = Instantiate(InstagramController.commentPrefab, new Vector3(0, 0, 0), Quaternion.identity, InstagramController.postReplyParent.transform);
            reply1.GetComponentInChildren<Image>().sprite = InstagramController.NPCspriteList[3];
            var replyText1 = reply1.GetComponentInChildren<TextMeshProUGUI>();
            replyText1.text = "(╯°Д°)╯︵凸 ICE CREEEEEEEEEEAM!!!!";

        }
        
        else if(currentSprite.name == "akunohana")
        {
            InstagramController.postText.text =
                "<b>Nico</b> Human nature is a joke";

        }
        else if(currentSprite.name == "gotoschool")
        {
            InstagramController.postText.text =
                "<b>Nico</b> First day of school. Feels okay.";
            var reply1 = Instantiate(InstagramController.commentPrefab, new Vector3(0, 0, 0), Quaternion.identity, InstagramController.postReplyParent.transform);
            reply1.GetComponentInChildren<Image>().sprite = InstagramController.NPCspriteList[3];
            var replyText1 = reply1.GetComponentInChildren<TextMeshProUGUI>();
            replyText1.text = "Come on! We’re happy to see you again!";

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
        
        
        if(CalculateInventory.wearingTop == false)
        {
            CalculateInventory.whiteShirtASR.sprite = CalculateInventory.allAdCloth["WhiteShirt"];
        }  
        if(CalculateInventory.wearingBottom == false)
        {
            CalculateInventory.blackPantsASR.sprite = CalculateInventory.allAdCloth["BlackPants"];
        }
        if(CalculateInventory.wearingShoe == false)
        {
            CalculateInventory.workShoeASR.sprite = CalculateInventory.allAdCloth["WorkShoe"];
        }
        
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

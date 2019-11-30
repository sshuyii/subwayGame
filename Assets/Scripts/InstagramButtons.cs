using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstagramButtons : MonoBehaviour
{
    private Sprite currentSprite;
    public Sprite transparent;

    private Image myImage;
    public Image gotoImage;
    private Button myButton;

    public InstagramController InstagramController;
    public CalculateInventory CalculateInventory;
    private Text username;

    private NewCameraController NewCameraController;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentSprite = GetComponent<Image>().sprite;
        username = GetComponentInChildren<Text>();
        
        NewCameraController = GameObject.Find("Main Camera").GetComponent<NewCameraController>();

        myImage = GetComponent<Image>();
        myButton = GetComponent<Button>();


    }

    // Update is called once per frame
    void Update()
    {
        if (myImage.sprite == transparent)
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
        Hide(NewCameraController.mainpage);
        NewCameraController.HideAllPersonalPages();

        
        if (username.text == "Karara")
        {
            Camera.main.transform.position =
                new Vector3(45, Camera.main.transform.position.y, Camera.main.transform.position.z);
            
            NewCameraController.myAppState = NewCameraController.AppState.KararaPage;
            Show(NewCameraController.KararaPage);
            

        }    
        else if (username.text == "Nico")
        {
            Camera.main.transform.position =
                new Vector3(55, Camera.main.transform.position.y, Camera.main.transform.position.z);
            NewCameraController.myAppState = NewCameraController.AppState.RetroPage;
            Show(NewCameraController.RetroPage);


        }       
    }
    
    public void ClickInsPost()
    {
        Camera.main.transform.position = new Vector3(65, Camera.main.transform.position.y, Camera.main.transform.position.z);

        gotoImage.sprite = currentSprite;
        
        NewCameraController.myAppState = NewCameraController.AppState.Post;


    }

    public void ChangePosture()
    {

        if(CalculateInventory.posNum < 3)
        {
            CalculateInventory.posNum++;
        }
        else
        {
            CalculateInventory.posNum = 1;
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

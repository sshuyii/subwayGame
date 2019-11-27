using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstagramButtons : MonoBehaviour
{
    private Sprite currentSprite;
    public Image gotoImage;

    public InstagramController InstagramController;
    private Text username;

    private NewCameraController NewCameraController;
    // Start is called before the first frame update
    void Start()
    {
        currentSprite = GetComponent<Image>().sprite;
        username = GetComponentInChildren<Text>();
        
        NewCameraController = GameObject.Find("Main Camera").GetComponent<NewCameraController>();

    }

    // Update is called once per frame
    void Update()
    {
        
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
        for (int i = 0; i < InstagramController.postureList.Count; i++)
        {
            if (InstagramController.postureList[i] == currentSprite)
            {
                if(i != InstagramController.postureList.Count - 1)
                {
                    currentSprite = InstagramController.postureList[i + 1];
                }
                else
                {
                    currentSprite = InstagramController.postureList[0];

                }
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

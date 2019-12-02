using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeToPersonalPageForButton : MonoBehaviour
{
    private NewCameraController NewCameraController;

    private Sprite mySprite;
    // Start is called before the first frame update
    void Start()
    {
        NewCameraController = GameObject.Find("Main Camera").GetComponent<NewCameraController>();
        mySprite = GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeToPersonalPage()
    {
        Hide(NewCameraController.mainpage);
        NewCameraController.HideAllPersonalPages();

        
        if (mySprite.name.Contains("karara"))
        {
            Camera.main.transform.position =
                new Vector3(45, Camera.main.transform.position.y, Camera.main.transform.position.z);
            
            NewCameraController.myAppState = NewCameraController.AppState.KararaPage;
            Show(NewCameraController.KararaPage);
            
        }    
        else if (mySprite.name.Contains("retro"))
        {
            Camera.main.transform.position =
                new Vector3(55, Camera.main.transform.position.y, Camera.main.transform.position.z);
            NewCameraController.myAppState = NewCameraController.AppState.RetroPage;
            Show(NewCameraController.RetroPage);
        }       
        else if (mySprite.name.Contains("ojisan"))
        {
            Camera.main.transform.position =
                new Vector3(55, Camera.main.transform.position.y, Camera.main.transform.position.z);
            NewCameraController.myAppState = NewCameraController.AppState.RetroPage;
            Show(NewCameraController.DesignerPage);
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

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChangeToPersonalPageForButton : MonoBehaviour
{
    private NewCameraController NewCameraController;

    private InstagramController InstagramController;
    
    private Sprite mySprite;
    // Start is called before the first frame update
    void Start()
    {
        NewCameraController = GameObject.Find("Main Camera").GetComponent<NewCameraController>();
        InstagramController = GameObject.Find("---InstagramController").GetComponent<InstagramController>();

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
        else if (mySprite.name.Contains("nico"))
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
            NewCameraController.myAppState = NewCameraController.AppState.DesignerPage;
            Show(NewCameraController.DesignerPage);
        }       
    }

    public void DoReply()
    {
        var reply1 = Instantiate(InstagramController.replyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        var reply2 = Instantiate(InstagramController.replyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        
       
        var replyText1 = reply1.GetComponentInChildren<Text>();
        var replyText2 = reply2.GetComponentInChildren<Text>();
           
        reply1.transform.SetParent(InstagramController.replyParent.transform);
        reply2.transform.SetParent(InstagramController.replyParent.transform);


        

        if(InstagramController.currentBackground == "Toothpaste")
        {
//            reply1.transform.SetParent(InstagramController.ToothpasteReply.transform);
//            reply2.transform.SetParent(InstagramController.ToothpasteReply.transform);


            replyText1.text = "xxxxx";
            replyText2.text = "ddddd";

        }        
        else if (InstagramController.currentBackground == "RV")
        {
//            reply1.transform.SetParent(InstagramController.RVReply.transform);
//            reply2.transform.SetParent(InstagramController.RVReply.transform);
            
            replyText1.text = "xxxxd";
            replyText2.text = "ddddx";
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

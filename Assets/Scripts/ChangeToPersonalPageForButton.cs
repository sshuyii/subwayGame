using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeToPersonalPageForButton : MonoBehaviour
{
    private NewCameraController NewCameraController;

    private InstagramController InstagramController;
    
    private Sprite mySprite;

    private Image myImage;
    private bool likeBool = true;

    private bool replyBool = true;
    private bool replyChosen = false;

    private List<GameObject> replyList = new List<GameObject>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        NewCameraController = GameObject.Find("Main Camera").GetComponent<NewCameraController>();
        InstagramController = GameObject.Find("---InstagramController").GetComponent<InstagramController>();

        myImage = GetComponent<Image>();

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
        else if (mySprite.name.Contains("NPC"))
        {
            Camera.main.transform.position =
                new Vector3(55, Camera.main.transform.position.y, Camera.main.transform.position.z);
            NewCameraController.myAppState = NewCameraController.AppState.NPCPage;
            Show(NewCameraController.NPCPage);
            if (mySprite.name.Contains("gNPC"))
            {
                
            }
        }       
        
    }

    public void DoReply()
    {

        if(replyChosen == false)
        {

            if (replyBool)
            {
                replyBool = false;

                var reply1 = Instantiate(InstagramController.replyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                var reply2 = Instantiate(InstagramController.replyPrefab, new Vector3(0, 0, 0), Quaternion.identity);

                replyList.Add(reply1);
                replyList.Add(reply2);


                InstagramController.replyList.Add(reply1);
                InstagramController.replyList.Add(reply2);


                var replyText1 = reply1.GetComponentInChildren<TextMeshProUGUI>();
                var replyText2 = reply2.GetComponentInChildren<TextMeshProUGUI>();

                reply1.transform.SetParent(InstagramController.replyParent.transform);
                reply2.transform.SetParent(InstagramController.replyParent.transform);



                if (InstagramController.currentBackground == "Toothpaste")
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

                    replyText1.text = "Highly recommend! It is amazing!";
                    replyText2.text = "And it matches my clothes so well!";
                }

            }
            else if (replyBool == false)
            {
                replyBool = true;
                for (int i = 0; i < replyList.Count; i++)
                {
                    Destroy(replyList[i]);
                }

            }
        }

    }

    public void FollowNico()
    {
        int retroTime = 3;

        for (int i = 0; i < InstagramController.retroPostList.Count; i++)
        {
            var newPost = Instantiate(InstagramController.PosturePostPrefabNew, new Vector3(0, 0, 0), Quaternion.identity);
        

            newPost.GetComponent<EntryTime>().time = retroTime;

            newPost.transform.Find("Post").gameObject.GetComponent<Image>().sprite =
                InstagramController.retroPostList[i];

            //set parent(probably a better way to do
            newPost.transform.SetParent(InstagramController.postParent.transform);
            
            retroTime += 10;
        }

        rearrangePosts();



        //change sprite to the newly taken photo
//        newPost.GetComponent<Image>().sprite = Sprite.Create(sprites,rec,new Vector2(0,0),100f);
//        
//        toothpastePost.transform.Find("Post").gameObject.GetComponent<Image>().sprite = Sprite.Create(sprites,rec,new Vector2(0,0),100f);

    }

    private void rearrangePosts()
    {
       
        var arr = InstagramController.postParent.GetComponentsInChildren<EntryTime>();
        System.Array.Sort(arr, (b, a) => a.time.CompareTo(b.time)); //you can flip a and b to sort the opposite direction
        for(int i = 0; i < arr.Length; i++)
        {
            arr[i].transform.SetSiblingIndex(i);
        }
    }
    
    
    public void ClickReply()
    {
        for (int i = 0; i < InstagramController.replyList.Count; i++)
        {
            if (InstagramController.replyList[i] !=  this.gameObject.transform.parent.gameObject)
            {
                Destroy(InstagramController.replyList[i]);
                //cannot generate replies once chosen one
                replyChosen = true;
                print(replyChosen);
            }
        }
    }

    public void Like()
    {
        if (likeBool)
        {
            myImage.sprite = InstagramController.likeFUll;
            likeBool = false;
        }
        else
        {
            myImage.sprite = mySprite;
            likeBool = true;
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

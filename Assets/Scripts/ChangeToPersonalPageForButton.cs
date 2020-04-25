using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeToPersonalPageForButton : MonoBehaviour
{
    private FinalCameraController FinalCameraController;

    private InstagramController InstagramController;
    
    private Sprite mySprite;

    private Image myImage;
    private bool likeBool = true;

    private bool replyBool = true;
    private bool replyChosen = false;

    private List<GameObject> replyList = new List<GameObject>();

    public GameObject privateAccount;
    public GameObject posts;
    
    
    // Start is called before the first frame updateGroup
    void Start()
    {
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();
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
        
        Hide(FinalCameraController.frontPage);
        Hide(FinalCameraController.postpage);
        FinalCameraController.HideAllPersonalPages();
        

        
        if (mySprite.name.Contains("karara"))
        {
            Camera.main.transform.position =
                new Vector3(45, Camera.main.transform.position.y, Camera.main.transform.position.z);
            
            FinalCameraController.myAppState = FinalCameraController.AppState.KararaPage;
            Show(FinalCameraController.KararaPage);
            
        }    
        else if (mySprite.name.Contains("nico"))
        {
            Camera.main.transform.position =
                new Vector3(55, Camera.main.transform.position.y, Camera.main.transform.position.z);
            FinalCameraController.myAppState = FinalCameraController.AppState.RetroPage;
            Show(FinalCameraController.RetroPage);
        }       
        else if (mySprite.name.Contains("ojisan"))
        {
            Camera.main.transform.position =
                new Vector3(55, Camera.main.transform.position.y, Camera.main.transform.position.z);
            FinalCameraController.myAppState = FinalCameraController.AppState.DesignerPage;
            Show(FinalCameraController.DesignerPage);
        }       
        else if (mySprite.name.Contains("NPC"))
        {
            Camera.main.transform.position =
                new Vector3(55, Camera.main.transform.position.y, Camera.main.transform.position.z);
            FinalCameraController.myAppState = FinalCameraController.AppState.NPCPage;
            Show(FinalCameraController.NPCPage);
            if (mySprite.name.Contains("aNPC"))
            {
                InstagramController.NPCProfile.sprite = InstagramController.NPCspriteList[0];
            }
            else if (mySprite.name.Contains("gNPC"))
            {
                InstagramController.NPCProfile.sprite = InstagramController.NPCspriteList[1];
            }
            else if (mySprite.name.Contains("rNPC"))
            {
                InstagramController.NPCProfile.sprite = InstagramController.NPCspriteList[2];
            }
            else if (mySprite.name.Contains("zNPC"))
            {
                InstagramController.NPCProfile.sprite = InstagramController.NPCspriteList[3];
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
                    replyText1.text = "Highly recommend! It is amazing!";
                    replyText2.text = "And it matches my clothes so well!";
                }
                else if (InstagramController.currentBackground == "FruitStand")
                {
                    replyText1.text = "Apples!";
                    replyText2.text = "Orange is my favorite color.";
                }
                else if (InstagramController.currentBackground == "Park")
                {
                    replyText1.text = "It is boring for you.";
                    replyText2.text = "......";
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

    private bool isfishTalking = false;
    private int clickBoss = 0;
    public void BossTalk()
    {
        //根据不同时间boss说话内容会变
        FinalCameraController.CancelAllUI();
        if(isfishTalking == false)
        {
            Show(InstagramController.FishBoss);
            isfishTalking = true;
        }
        else
        {
            Hide(InstagramController.FishBoss);
            isfishTalking = false;
        }
        
    }
    
    
    public void FollowNico()
    {
        if(InstagramController.followNico == false)
        {
            //change private account to posts
            //need a list for all the film lists
//            privateAccount.SetActive(false);
//            posts.SetActive(true);
            
            //change others
            InstagramController.followNico = true;
            myImage.sprite = InstagramController.unfollow;
            int retroTime = 1;

            for (int i = 0; i < InstagramController.retroPostList.Count; i++)
            {
                var newPost = Instantiate(InstagramController.PosturePostPrefabNew, new Vector3(0, 0, 0),
                    Quaternion.identity);
                newPost.GetComponent<EntryTime>().time =
                    InstagramController.retroPostList[i].gameObject.GetComponent<EntryTime>().time;

                //set post image in main page
                newPost.transform.Find("Post").gameObject.GetComponent<Image>().sprite =
                    InstagramController.retroPostList[i].sprite;

                //set profile and user name in main page
                var profileNicoImage = newPost.transform.Find("Profile").gameObject.GetComponent<Image>();
                profileNicoImage.sprite = InstagramController.allProfile["nico"];
                profileNicoImage.GetComponentInChildren<Text>().text = "Nico";

                //set parent(probably a better way to do
                newPost.transform.SetParent(InstagramController.postParent.transform);

                InstagramController.postList.Add(newPost);
            }

            rearrangePosts();
        }
        

    }

    public void FollowDesigner()
    {
        if(InstagramController.followDesigner == false)
        {
            //change private account to postsDesigner
            //need a list for all the film lists
//            privateAccount.SetActive(false);
//            posts.SetActive(true);
            
            //change othters
            InstagramController.followDesigner = true;
            myImage.sprite = InstagramController.unfollow;

            for (int i = 0; i < InstagramController.designerPostList.Count; i++)
            {
                var newPost = Instantiate(InstagramController.PosturePostPrefabNew, new Vector3(0, 0, 0),
                    Quaternion.identity);
                newPost.GetComponent<EntryTime>().time =  
                    InstagramController.designerPostList[i].gameObject.GetComponent<EntryTime>().time;


                newPost.transform.Find("Post").gameObject.GetComponent<Image>().sprite =
                    InstagramController.designerPostList[i].sprite;

                var profileImage = newPost.transform.Find("Profile").gameObject.GetComponent<Image>();
                
                profileImage.sprite = InstagramController.allProfile["ojisan"];

                profileImage.GetComponentInChildren<Text>().text = "Alex";

                //set parent(probably a better way to do
                newPost.transform.SetParent(InstagramController.postParent.transform);
                InstagramController.postList.Add(newPost);

            }

            rearrangePosts();
        }
        

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

using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class InstagramController : MonoBehaviour
{
    public List<GameObject> postList = new List<GameObject>();

    public CalculateInventory CalculateInventory;
    private FinalCameraController FinalCameraController;

    private SubwayMovement SubwayMovement;
    
    public int currentPostNum;

    public SortedDictionary<GameObject, int> allPostsDict = new SortedDictionary<GameObject, int>();

    public Sprite transparent;
    public Image gotoImage;
    public Image gotoProfile;
    public GameObject gotoText;
    public Text postText;
    public GameObject postReplyParent;

    public GameObject redDot;
   
    public GameObject postPrefab;
    public GameObject postParent;
    
    public GameObject replyPrefab;
    public Transform replyParent;


    public CanvasGroup FishBoss;
    public GameObject commentPrefab;

    public List<Sprite> NPCspriteList;

    public Image NPCProfile;
    public Sprite likeFUll;
    //change post in karara's page
    public GameObject filmParent;
    public GameObject photoPostPrefab;
    
    //reply list
    public List<GameObject> replyList = new List<GameObject>();
    
    //take picture page gameobjects
    public GameObject PosturePostPrefab;
    public GameObject PosturePostPrefabNew;

    public GameObject originalPosture;
    public Image[] PosturePostImageList;
    
    
    //ad clothes game objects
    public List<Image> adClothes;
  
    //record background, clothes and posture
    public List<Sprite> profileList;
    public Dictionary<string, Sprite> allProfile = new Dictionary<string, Sprite>();


    public List<Sprite> postureList;
    public Dictionary<string, Sprite> allPosture = new Dictionary<string, Sprite>();

    //change backgrounds
    public List<Sprite> backAdList;
    public Dictionary<string, Sprite> allBackAd = new Dictionary<string, Sprite>();
    
    public Dictionary<string, bool> AdAlreadyTakenList = new Dictionary<string, bool>();

    
    public Image adBodyImage;

    //work cloth list
    public List<Sprite> workclothList;
    public Image workClothImage;
    public Image workShoeImage;


    public string currentBackground;
    public Dictionary<string, List<int>> backgroundPoseDict = new Dictionary<string, List<int>>();
    
    
    //this definitely needs to change
    public Transform ToothpasteReply;
    public Transform RVReply;
    public Transform ParkReply;

    
    //a list recording all retro girls photos
    public List<Image> retroPostList = new List<Image>();
    public List<Image> designerPostList = new List<Image>();

    
    
    public bool replyChosen = false;

    public Sprite unfollow;

    public bool followNico;
    public bool followDesigner;

    public List<string> usedAdsList = new List<string>();

    public TextMeshProUGUI followerNum;
    public TextMeshProUGUI subFollowerNum;
    
    //如果每个海报的背景是不一样的话

    public List<int> backgroundPose1;
    public List<int> backgroundPose2;
    public List<int> backgroundPose3;
    public List<int> backgroundPose4;
//    public List<int> backgroundPose5;

    
    //for npc ins that are generated during game
    public List<Sprite> nicoLaterPost;
    public List<Sprite> ojisanLaterPost;

    public GameObject RetroPageContent;
    public GameObject OjisanPageContent;
    public GameObject PersonalPagePrefab;

    //two ads that need to be generated
    public GameObject thirdAd;
    public GameObject fourthAd;


    public TextMeshProUGUI fishText;
    
    // Start is called before the first frame update
    void Start()
    {


        //get the tutorial post into postList
        postList.Add(postParent.transform.GetChild(0).gameObject);
        
        
        redDot.SetActive(false);
        SubwayMovement = GameObject.Find("---StationController").GetComponent<SubwayMovement>();
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();


        for (int i = 0; i < profileList.Count; i++)
        {
            allProfile.Add(profileList[i].name, profileList[i]);
        }
        
        for (int i = 0; i < backAdList.Count; i++)
        {
            allBackAd.Add(backAdList[i].name, backAdList[i]);

        }

        for (int i = 0; i < backAdList.Count; i++)
        {
            AdAlreadyTakenList.Add(backAdList[i].name, true);
        }

        PosturePostImageList = originalPosture.GetComponentsInChildren<Image>();

        for (int i = 0; i < backAdList.Count; i++)
        {
        }
        if(!FinalCameraController.isTutorial)
        {
            backgroundPoseDict.Add(backAdList[0].name, backgroundPose1);
            backgroundPoseDict.Add(backAdList[1].name, backgroundPose2);
            backgroundPoseDict.Add(backAdList[2].name, backgroundPose3);
            backgroundPoseDict.Add(backAdList[3].name, backgroundPose4);
        }
//        backgroundPoseDict.Add(backAdList[4].name, backgroundPose5);



    }

    //those two are used to make sure that npc posts are only generated once
    public int newPostNum;
    public int newStationNum;

    public int takenNum;


    private int endStationPre;

    private bool chapterOneEndPre;
    // Update is called once per frame
    void Update()
    {
       
        
        //enable new posters for chapter one
        //when player is not in subway4
        if (!FinalCameraController.isTutorial)
        {
            if (FinalCameraController.mySubwayState != FinalCameraController.SubwayState.Four)
            {
//            print("AdAlreadyTakenList FruitStand" + AdAlreadyTakenList["FruitStand"]);
//            print("AdAlreadyTakenList FruitStand" + AdAlreadyTakenList["RV"]);

                if (!AdAlreadyTakenList["FruitStand"] && SubwayMovement.alreadyStation1)
                {
                    thirdAd.SetActive(true);
                }

                if (!AdAlreadyTakenList["RV"] && SubwayMovement.alreadyStation2)
                {
                    fourthAd.SetActive(true);
                }
            }


            //第一次到第一站，产生一个新的post, nico
            if (FinalCameraController.myCameraState == FinalCameraController.CameraState.App)
            {
                //啥都不发生
            }
            else if (SubwayMovement.alreadyStation1 && newStationNum == 0)
            {
                print("arriving first station, nico");

                StartCoroutine(CreatePersonalPagePost("nico", nicoLaterPost[0],
                    "I feel like the icecream"));

                newStationNum = 1;
            }

            //第一次到第二站，产生一个新的post, nami
            else if (SubwayMovement.alreadyStation2 && newStationNum == 1)
            {
                print("arriving 2 station, nami");

                StartCoroutine(CreatePersonalPagePost("ojisan", ojisanLaterPost[0],
                    "Homemade salad"));

                newStationNum = 2;

            }
            //第一次到第二站，产生一个新的post, nico
            else if (SubwayMovement.alreadyStation0 && newStationNum == 2)
            {
                print("arriving 3 station, nico");

                StartCoroutine(CreatePersonalPagePost("nico", nicoLaterPost[2],
                    "Crazy is my daily routine"));
                

                newStationNum = 3;

            }


//
//        //to decide how many posters have been used
//        for (int i = 0; i < backAdList.Count; i++)
//        {
//            if (AdAlreadyTakenList[backAdList[i].name])
//            {
//                takenNum++;
//            }
//            
//        }
            //第一次到第一站，产生一个新的post, nico
            if (FinalCameraController.myCameraState == FinalCameraController.CameraState.App)
            {
                //啥都不发生
            }
            //在两个海报前面照过相
            else if (takenNum == 1 && newPostNum == 0)
            {
                print("1 poster used, nami");
                StartCoroutine(CreatePersonalPagePost("ojisan", ojisanLaterPost[1],
                    "Stay healthy"));

                newPostNum = 1;
            }

            else if (takenNum == 2 && newPostNum == 1)
            {
                if (FinalCameraController.myCameraState != FinalCameraController.CameraState.App)
                {
                    print("2 poster used, nico");

                    StartCoroutine(CreatePersonalPagePost("nico", nicoLaterPost[1],
                        "Night in the wood"));
                    

                    newPostNum = 2;
                }
            }

            //todo:if all posters have been used
            //if all posters have been used
            //end for chapter 1
            //必须当玩家不在app界面中的时候才能产生
            if (!AdAlreadyTakenList.ContainsValue(true))
            {
                //if more than 20 followers, chapter 1 succeeds
                if (System.Convert.ToInt32(followerNum.text) >= 20 && newPostNum == 2 && newStationNum == 3 &&
                    !chapterOneEndPre)
                {
                    if (FinalCameraController.myCameraState != FinalCameraController.CameraState.App)
                    {
//                        StartCoroutine(CreatePersonalPagePost("nico", nicoLaterPost[2],
//                            "this is the end of chapter 1"));

                        //下一章ojisan要搬家了，所以这个是最后一张post
                        StartCoroutine(CreatePersonalPagePost("ojisan", ojisanLaterPost[2],
                            "embrace yourself"));
                      
                        newPostNum = 3;
                        fishText.text = "What are you doing? Turn off your phone during work!";
                        chapterOneEndPre = true;
                        endStationPre = SubwayMovement.currentStation;
                    }
                }
            }

            if (chapterOneEndPre)
            {
                if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.Two)
                {
                    FinalCameraController.Show(FinalCameraController.messageCG);
                }
                else if (FinalCameraController.mySubwayState == FinalCameraController.SubwayState.One)
                {
                    FinalCameraController.Show(FishBoss);
                }

                FinalCameraController.ChapterOneEnd = true;

                //到达最近的一站之后，就停下来，车再也不动了
                if (!SubwayMovement.isMoving && SubwayMovement.currentStation == endStationPre &&
                    FinalCameraController.myCameraState == FinalCameraController.CameraState.Subway)
                {
                    SubwayMovement.isMoving = false;
                    FinalCameraController.ChapterOneEnd = true;
                }
            }
        }

    }

    
    IEnumerator CreatePersonalPagePost(string NpcName, Sprite post, string postText)
    {
        //first wait for 10 seconds
        yield return new WaitForSeconds(5f);
        
        
        //instantiate new post object     
        GameObject newPost = Instantiate(photoPostPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        print("create personal page posts");

        newPost.GetComponent<Image>().sprite = post;
        newPost.GetComponent<EntryTime>().time = FinalCameraController.entryTime;
        FinalCameraController.entryTime++;

       
        
        //set parent(probably a better way to do
        if(NpcName == "nico")
        {
            newPost.transform.SetParent(RetroPageContent.transform);
            retroPostList.Add(newPost.GetComponent<Image>());
            newPost.transform.localScale = Vector3.one;

            //set post text
            newPost.GetComponent<EntryTime>().postText = "<b>Nico</b> " + postText;
            //如果关注了nico,放到首页里
            if (followNico)
            {
                CreatePostInMainPage("nico", post, postText, newPost.GetComponent<EntryTime>().time);
            }
        }    
        else if (NpcName == "ojisan")
        {
            print("create ojisan posts");
            newPost.transform.SetParent(OjisanPageContent.transform);
            newPost.transform.localScale = Vector3.one;

            //set post text
            newPost.GetComponent<EntryTime>().postText = "<b>Ojisan</b> " + postText;
            //如果关注了ojisan,放到首页里,或者如果是第一章最后一个post，强制放进首页
            if (followDesigner || chapterOneEndPre)
            {
                CreatePostInMainPage("ojisan", post, postText,newPost.GetComponent<EntryTime>().time);
            }
            designerPostList.Add(newPost.GetComponent<Image>());

        }
        //todo: 和有没有关注这个人有关系，现在先强制放到首页了？
        //move to the first of the list 
        //postList.Insert(0,newPost);
            
        //change post image
        
        redDot.SetActive(true);

//        //re-arrange children object, so the latest is displayed as the first
//        for (int i = 0; i < postList.Count; i++)
//        {
//            postList[i].transform.SetSiblingIndex(i);
//        }
//        
        
    }

   
    public void CreatePostInMainPage(string npcName, Sprite post, string text, int time)
    {
        //instantiate new post object     
        var newPost = Instantiate(PosturePostPrefabNew, new Vector3(0, 0, 0), Quaternion.identity);
        //set parent(probably a better way to do
        newPost.transform.SetParent(postParent.transform);
        newPost.transform.localScale = Vector3.one;
            
        //set entry time
        newPost.GetComponent<EntryTime>().time = time;

        //move to the first of the list
        postList.Add(newPost);
        FinalCameraController.resetPostOrder();

        //Get profile and image
        var profile = newPost.transform.Find("Profile").gameObject.GetComponent<Image>();

        var profileName = profile.gameObject.GetComponentInChildren<Text>();
        
        //change sprite to the right ins photo
        
        //get the post text
        newPost.transform.Find("Post").gameObject.GetComponent<Image>().sprite = post;
        var textList = newPost.transform.Find("Post").gameObject.GetComponentsInChildren<Text>();

    
        //create comments
        var newComment = Instantiate(commentPrefab, new Vector3(0, 0, 0), Quaternion.identity, newPost.transform.Find("Comments"));
        //set parent(probably a better way to do
//        newComment.transform.parent = toothpastePost.transform.Find("Comments");

        newComment.transform.localPosition = new Vector3(0f, 0f, 0);

        //get the text child
        var CommentText = newComment.GetComponentInChildren<TextMeshProUGUI>();

        //get the comment profile
        var ProfileImage = newComment.GetComponent<Image>();
        
//        //todo: comments need to appear after sometime
//        CommentText.text = "That RV looks amazing. I'm gonna get one for myself.";
//        ProfileImage.sprite = allProfile["nico"];
            
        //change the post text
        if(npcName == "nico")
        {
            profile.sprite = allProfile["nico"];
            textList[0].text =
                "<b>Nico</b> " + text;
            textList[1].text = "Today";
            
            
            //set profile and user name in main page
            profileName.text = "Nico";
            //个位数为1
            //之前已经有三个p: 1, 11, 21
//            newPost.GetComponent<EntryTime>().time = time;


        }
        else if(npcName == "ojisan")
        {
            profile.sprite = allProfile["ojisan"];
            textList[0].text =
                "<b>Ojisan</b> " + text;
            textList[1].text = "Today";
            
            //set profile and user name in main page
            profileName.text = "Alex";
            
        }
    }
}

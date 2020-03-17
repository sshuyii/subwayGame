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

    public int currentPostNum;

    public SortedDictionary<GameObject, int> allPostsDict = new SortedDictionary<GameObject, int>();

    public Sprite transparent;
    public Image gotoImage;
    public Image gotoProfile;
    public GameObject gotoText;
    public Text postText;
    public GameObject postReplyParent;

    
   
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
    public List<Sprite> retroPostList = new List<Sprite>();
    public List<Sprite> designerPostList = new List<Sprite>();

    
    
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
    public List<int> backgroundPose5;



    
    // Start is called before the first frame update
    void Start()
    {

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
        backgroundPoseDict.Add(backAdList[0].name, backgroundPose1);
        backgroundPoseDict.Add(backAdList[1].name, backgroundPose2);
        backgroundPoseDict.Add(backAdList[2].name, backgroundPose3);
        backgroundPoseDict.Add(backAdList[3].name, backgroundPose4);
        backgroundPoseDict.Add(backAdList[4].name, backgroundPose5);



    }

    // Update is called once per frame
    void Update()
    {
       
    }
}

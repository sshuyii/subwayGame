using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstagramController : MonoBehaviour
{
    public List<GameObject> postList = new List<GameObject>();

    public CalculateInventory CalculateInventory;

    public Image adBodyImage;
    
    public SortedDictionary<GameObject, int> allPostsDict = new SortedDictionary<GameObject, int>();

    public Sprite transparent;
    public Image gotoImage;
    
    public GameObject postPrefab;
    public GameObject postParent;
    
    public GameObject replyPrefab;
    public Transform replyParent;


    public GameObject commentPrefab;


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

    
    //work cloth list
    public List<Sprite> workclothList;
    public Image workClothImage;


    public string currentBackground;
    
    
    //this definitely needs to change
    public Transform ToothpasteReply;
    public Transform RVReply;
    public Transform ParkReply;

    
    //a list recording all retro girls photos
    public List<Sprite> retroPostList = new List<Sprite>();
    
    
    public bool replyChosen = false;

    
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

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}

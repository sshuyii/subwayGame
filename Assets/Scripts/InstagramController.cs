using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstagramController : MonoBehaviour
{
    public List<GameObject> postList = new List<GameObject>();
    
    
    public GameObject postPrefab;
    public GameObject postParent;
    
    //take picture page gameobjects
    public GameObject PosturePostPrefab;
    public GameObject originalPosture;
    public Image[] PosturePostImageList;
    
    //record background, clothes and posture
    public List<Sprite> profileList;
    public Dictionary<string, Sprite> allProfile = new Dictionary<string, Sprite>();


    public List<Sprite> postureList;
    public Dictionary<string, Sprite> allPosture = new Dictionary<string, Sprite>();

    //change backgrounds
    public List<Sprite> backAdList;
    public Dictionary<string, Sprite> allBackAd = new Dictionary<string, Sprite>();

    
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

        PosturePostImageList = originalPosture.GetComponentsInChildren<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

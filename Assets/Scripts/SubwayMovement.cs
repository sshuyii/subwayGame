using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubwayMovement : MonoBehaviour
{
    private FinalCameraController FinalCameraController;

    public int currentStation;
    public GameObject highlight;
    private SpriteRenderer hSR;
    
    public GameObject arrow;
    private SpriteRenderer aSR;

    public float doorMovement;
    public float doorWidth;
    
    
    public List<GameObject> arrows;

//    public List<Vector3> stationPos;

    public List<GameObject> highlights;

    public List<Vector3> bagPos;
    public List<Button> clothBags;
    private bool bagFirst = true;
    public GameObject clothBagGroup;

    public CanvasGroup dSR1;
    public CanvasGroup dSR2;


    
    private Dictionary<string, int> allStation = new Dictionary<string, int>();
    public List<string> stationNames;

    public bool isMoving = false;//if true, the train is moving
    
    //doors
    public GameObject left1;
    public GameObject right1;
    
    public GameObject left2;
    public GameObject right2;
    
    //door initial positions
    private float left1Pos;
    private float right1Pos;
    private float left2Pos;
    private float right2Pos;

    public float moveTime;
    public float stayTime;

    //a list of buttons in detail background
    public List<Image> detailList0 = new List<Image>();
    public List<Image> detailList1 = new List<Image>();

    
    //time text
    public TextMeshProUGUI CountDownText;
    
    private float CountDownTime;

    private float timer = 0;

    private bool nothingInside = false; //if nothing is in the machine
    
  
    public List<Dictionary<String, List<Sprite>>> allStationList = new List<Dictionary<String, List<Sprite>>>();

   
    
    // Start is called before the first frame update
    void Start()
    {

        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();

        aSR = arrow.GetComponent<SpriteRenderer>();
        hSR = highlight.GetComponent<SpriteRenderer>();

        
        currentStation = 0;
        
        //get all the doors position when game starts
        left1Pos = left1.transform.position.x;
        right1Pos = right1.transform.position.x;
        left2Pos = left2.transform.position.x;
        right2Pos = right2.transform.position.x;

        //record individual station dic into the general list
//        allStationList.Add(Station0List);
//        allStationList.Add(Station1List);
//        allStationList.Add(Station2List);


        //get all station names into the dictionary
        for (var i = 0; i < stationNames.Count; ++i)
        {
            allStation.Add(stationNames[i], i);
        }
        
        //train stays at the first station for 30seconds
        //isMoving = false for 30 seconds, then isMoving = true for 60 seconds
        InvokeRepeating("trainMove", stayTime, stayTime + moveTime);
        InvokeRepeating("trainStop", stayTime + moveTime, stayTime + moveTime);

    }


    private Button clothBag1;
    private Button clothBag2;
    
    // Update is called once per frame
    void Update()
    {
        //train needs 1 minute on its way
//        print("currentStation = " + currentStation);
//        print("isMoving =  " + isMoving);

        //this timer is specifically used for station0
        timer += Time.deltaTime;
        
        NumberRecalculate();

        //decide which station is highlighted on screen
        if (isMoving == false)
        {
            //highlight.transform.localPosition = stationPos[currentStation];
                
            highlight.transform.position = highlights[currentStation].transform.position;
            highlight.transform.rotation = Quaternion.Euler(new Vector3 (highlights[currentStation].transform.rotation.eulerAngles.x, highlights[currentStation].transform.rotation.eulerAngles.y, highlights[currentStation].transform.rotation.eulerAngles.z));

            aSR.enabled = false;
            hSR.enabled = true;

            //open doors
            if (left1.transform.position.x > left1Pos - doorWidth)
            {
                left1.transform.position -= new Vector3(doorMovement, 0, 0);
            }
            
            if (right1.transform.position.x < right1Pos + doorWidth)
            {
                right1.transform.position += new Vector3(doorMovement, 0, 0);
            }
            else
            {
                //if arrives at a station and door already opened
                //这里的衣服应该随机产生，现在先写成了固定两包衣服：Alex + 路人
                if (currentStation == 0 && bagFirst && !FinalCameraController.isTutorial)
                {
                    //generate a bag of clothsPos
                    clothBag1 = Instantiate(clothBags[0], bagPos[0], Quaternion.identity) as Button;
                    clothBag2 = Instantiate(clothBags[1], bagPos[1], Quaternion.identity) as Button;
    
                    clothBag1.transform.SetParent(clothBagGroup.transform, false);
                    clothBag2.transform.SetParent(clothBagGroup.transform, false);

                    bagFirst = false;
                    
                    
                }
            }
            
            //open back doors
            if (left2.transform.position.x > left2Pos - doorWidth)
            {
                left2.transform.position -= new Vector3(doorMovement, 0, 0);
            }
            
            if (right2.transform.position.x < right2Pos + doorWidth)
            {
                right2.transform.position += new Vector3(doorMovement, 0, 0);
            }
        }
        else
        {
            aSR.enabled = true;
            hSR.enabled = false;

            arrow.transform.position = arrows[currentStation].transform.position;
            arrow.transform.rotation = Quaternion.Euler(new Vector3 (arrows[currentStation].transform.rotation.eulerAngles.x, arrows[currentStation].transform.rotation.eulerAngles.y, arrows[currentStation].transform.rotation.eulerAngles.z));
            
            //close doors
            if (left1.transform.position.x < left1Pos)
            {
                left1.transform.position += new Vector3(doorMovement, 0, 0);
            }
            
            if (right1.transform.position.x > right1Pos)
            {
                right1.transform.position -= new Vector3(doorMovement, 0, 0);
            }
            
            //close backdoors
            if (left2.transform.position.x < left2Pos)
            {
                left2.transform.position += new Vector3(doorMovement, 0, 0);
            }
            
            if (right2.transform.position.x > right2Pos)
            {
                right2.transform.position -= new Vector3(doorMovement, 0, 0);
            }
        }
    }

    void trainMove()
    {
        if(currentStation < 2)
        {
            currentStation++;
        }
        else
        {
            currentStation = 0;
        }
        isMoving = true;
    }

    void trainStop()
    {
        isMoving = false;
    }
    
    IEnumerator MyCoroutine(float time)
    {
        //This is a coroutine
        //train stays at a station for 30 seconds
        yield return new WaitForSeconds (time);
        currentStation++;

    }

    private void NumberRecalculate()
    {
        float realTimer = (moveTime + stayTime) * 3 - timer;
        
        if (realTimer < 0)
        {
            realTimer = 0;
        }
        
        if (Mathf.RoundToInt(timer / 60) < 10)
        {
            if (Mathf.RoundToInt(realTimer % 60) < 10)
            {
                CountDownText.text = "0" + Mathf.RoundToInt(realTimer / 60).ToString() + ":" + "0" +
                                Mathf.RoundToInt(realTimer % 60).ToString();
            }
            else
            {
                CountDownText.text = "0" + Mathf.RoundToInt(realTimer / 60).ToString() + ":" +
                                Mathf.RoundToInt(realTimer % 60).ToString();
            }
        }
        else
        {
            if (Mathf.RoundToInt(realTimer % 60) < 10)
            {
                CountDownText.text = Mathf.RoundToInt(realTimer / 60).ToString() + ":" + "0" +
                                Mathf.RoundToInt(realTimer % 60).ToString();
            }
            else
            {
                CountDownText.text = Mathf.RoundToInt(realTimer / 60).ToString() + ":" +
                                Mathf.RoundToInt(realTimer % 60).ToString();
            }
        }
    }
    
    private bool isDetailed = false;
    public void StationDetails(int stationNum)
    {
        //dSR used to be spriteRenderer
//        if (dSR.enabled == false)
//        {
//            dSR.enabled = true;
//        }
//        else if (dSR.enabled == true)
//        {
//            dSR.enabled = false;
//        }

        //change time

        //if the button pressed is the the first station
        if (stationNum == 0)
        {
            //show and close the UI
            if (FinalCameraController.AllStationClothList.ContainsKey(clothBag1.tag) || FinalCameraController.AllStationClothList.ContainsKey(clothBag2.tag))
            {
                if (isDetailed)
                {
                    isDetailed = false;
                }
                else if (isDetailed == false)
                {
                    isDetailed = true;
                }
            }
            
            //if the machine has been opened
            //现在这里假设第一站就只会有两包固定的衣服，因此只需要检测这两包固定的衣服是否已被放入
            if (FinalCameraController.AllStationClothList.ContainsKey(clothBag1.tag))
            {
                //get the clothes inside
                for (int i = 0; i < FinalCameraController.AllStationClothList[clothBag1.tag].Count; i++)
                {
                    detailList0[i].enabled = true;
                    detailList0[i].sprite = FinalCameraController.AllStationClothList[clothBag1.tag][i];
                }

                //show canvas group
                if (isDetailed)
                {
                    Hide(dSR1);
                }
                else
                {
                    Show(dSR1);
                }
            }
            
            //check bag2
            if (FinalCameraController.AllStationClothList.ContainsKey(clothBag2.tag))
            {
                //get the clothes inside
                for (int i = 0; i < FinalCameraController.AllStationClothList[clothBag2.tag].Count; i++)
                {
                    detailList1[i].enabled = true;
                    detailList1[i].sprite = FinalCameraController.AllStationClothList[clothBag2.tag][i];
                }
                //show canvas group
                if (isDetailed)
                {
                    Hide(dSR2);
                }
                else
                {
                    Show(dSR2);
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

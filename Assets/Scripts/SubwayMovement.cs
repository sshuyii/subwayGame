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

    public bool outsideStation = true;

    public List<bool> bagPosAvailable = new List<bool>();
    //public List<GameObject> arrows;

//    public List<Vector3> stationPos;

    public List<GameObject> highlights;

    public List<Vector3> bagPos;
    public List<Button> clothBags;
    private bool bagFirst = true;
    public GameObject clothBagGroup;

    private List<CanvasGroup> DetailCG = new List<CanvasGroup>();
    public CanvasGroup dSR1;
    public CanvasGroup dSR2;


    //a list recording all the bags on the train that haven't been taken into the washing machine
    private Dictionary<string, bool> AllBagsTaken = new Dictionary<string, bool>();
    public int bagNum = 0;
    
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
    private List<List<Image>> AllDetailList = new List<List<Image>>();
    public List<Image> detailList0 = new List<Image>();
    public List<Image> detailList1 = new List<Image>();

    
    //time text
    public TextMeshProUGUI ClothCountDownText;
    
    private float CountDownTime;

    private float timer = 0;

    private bool nothingInside = false; //if nothing is in the machine
    
  
    public List<Dictionary<String, List<Sprite>>> allStationList = new List<Dictionary<String, List<Sprite>>>();

    //one station has one list recording the potential cloth bags
    public List<Button> bagStation0;
    public List<Button> bagStation1;
    public List<Button> bagStation2;

    private Dictionary<string, List<Button>> NameToStationBags = new Dictionary<string, List<Button>>();

    public TextMeshProUGUI CountDownTimer;
    private float stationTimer;
    private float realTimer;

    
    // Start is called before the first frame update
    void Start() 
    {

        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();

        bagPosAvailable.Add(false);
        bagPosAvailable.Add(false);
        bagPosAvailable.Add(false);

            
        aSR = arrow.GetComponent<SpriteRenderer>();
        hSR = highlight.GetComponent<SpriteRenderer>();

        realTimer = (moveTime + stayTime) * 3 - timer;
        
        NameToStationBags.Add("0", bagStation0);
        NameToStationBags.Add("1", bagStation1);
        NameToStationBags.Add("2", bagStation2);

        AllDetailList.Add(detailList0);
        AllDetailList.Add(detailList1);

        DetailCG.Add(dSR1);
        DetailCG.Add(dSR2);

        currentStation = 0;

        stationTimer = stayTime;
        CountDownTimer.text = "";
        
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
//        InvokeRepeating("trainMove", stayTime, stayTime + moveTime);
//        InvokeRepeating("trainStop", stayTime + moveTime, stayTime + moveTime);

        Invoke("trainMove", stayTime);


    }


    private Button clothBag1;
    private Button clothBag2;
    private Button clothBag3;

    public float newTimer1;
    public float newTimer2;

    
    // Update is called once per frame
    void Update()
    {
        //instead of InvokeRepeating
        if(!isMoving)
        {
            newTimer1 += Time.deltaTime;
        }      
        if(newTimer1 > stayTime)
        {
            trainMove();
            newTimer1 = 0;
        }
          
        newTimer2 += Time.deltaTime;
        if (newTimer2 > stayTime + moveTime)
        {
            trainStop();
            newTimer2 = 0;
        }
        
        //train needs 1 minute on its way
//        print("currentStation = " + currentStation);
//        print("isMoving =  " + isMoving);

        //this timer is specifically used for station0
        timer += Time.deltaTime;
        
        if(!FinalCameraController.isTutorial)
        {
            NumberRecalculate(realTimer, ClothCountDownText);
      
            //start the timer once the train's in station
            if (!isMoving)
            {
                stationTimer -= Time.deltaTime;
                NumberRecalculate(stationTimer, CountDownTimer);
            }
            else
            {
                CountDownTimer.text = "";
                stationTimer = stayTime;
            }
        }
        //decide which station is highlighted on screen
        if (isMoving == false)
        {
            //highlight.transform.localPosition = stationPos[currentStation];

            highlight.transform.position = highlights[currentStation].transform.position;
            highlight.transform.rotation = Quaternion.Euler(new Vector3(
                highlights[currentStation].transform.rotation.eulerAngles.x,
                highlights[currentStation].transform.rotation.eulerAngles.y,
                highlights[currentStation].transform.rotation.eulerAngles.z));

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
                //don't generate new bags if there are already three bags in the car
                if (!FinalCameraController.isTutorial && bagFirst)
                {
                    for (int i = 0; i < NameToStationBags[currentStation.ToString()].Count; i++)
                    {
                        print("NameToStationBags[currentStation.ToString()].Count" + NameToStationBags[currentStation.ToString()].Count);
                        GenerateBag(currentStation);
                        if (i == NameToStationBags[currentStation.ToString()].Count - 1)
                        {
                            bagFirst = false;
                        }

                    }

                    /*code used before
                    //if arrives at a station and door already opened
                    //这里的衣服应该随机产生，现在先写成了固定两包衣服：Alex + Bella
                    //generate a bag of clothsPos
                    if(bagNum < 3)
                    clothBag1 = Instantiate(clothBags[0], bagPos[0], Quaternion.identity) as Button;
                    clothBag2 = Instantiate(clothBags[1], bagPos[1], Quaternion.identity) as Button;
                    
                    clothBag1.transform.SetParent(clothBagGroup.transform, false);
                    clothBag2.transform.SetParent(clothBagGroup.transform, false);

                    //record the bags into list
                    AllBagsTaken.Add("Bella", true);
                    AllBagsTaken.Add("Alex", true);


                    bagFirst = false;
                    
                }
                else if (currentStation == 1)
                {
                    //generate a bag of clothsPos
                    clothBag3 = Instantiate(clothBags[0], bagPos[0], Quaternion.identity) as Button;

                    clothBag3.transform.SetParent(clothBagGroup.transform, false);

                    bagFirst = false;
                }
                else if (currentStation == 2)
                {
                    
                }
            */

                }

//                //open back doors
//                if (left2.transform.position.x > left2Pos - doorWidth)
//                {
//                    left2.transform.position -= new Vector3(doorMovement, 0, 0);
//                }
//
//                if (right2.transform.position.x < right2Pos + doorWidth)
//                {
//                    right2.transform.position += new Vector3(doorMovement, 0, 0);
//                }
            }

//            else
//            {
//                aSR.enabled = true;
//                hSR.enabled = false;
//
//                arrow.transform.position = arrows[currentStation].transform.position;
//                arrow.transform.rotation = Quaternion.Euler(new Vector3(
//                    arrows[currentStation].transform.rotation.eulerAngles.x,
//                    arrows[currentStation].transform.rotation.eulerAngles.y,
//                    arrows[currentStation].transform.rotation.eulerAngles.z));
////
//                //close doors
//                if (left1.transform.position.x < left1Pos)
//                {
//                    left1.transform.position += new Vector3(doorMovement, 0, 0);
//                }
//
//                if (right1.transform.position.x > right1Pos)
//                {
//                    right1.transform.position -= new Vector3(doorMovement, 0, 0);
//                }
//
//                //close backdoors
//                if (left2.transform.position.x < left2Pos)
//                {
//                    left2.transform.position += new Vector3(doorMovement, 0, 0);
//                }
//
//                if (right2.transform.position.x > right2Pos)
//                {
//                    right2.transform.position -= new Vector3(doorMovement, 0, 0);
//                }
//            }
        }

    }

    Button bag;
    private int previousIndex = 2;

    //this is used to create new bags in the car
    void GenerateBag(int stationNum)
    {
        if (bagNum < 3)
        {
            //注意：这里不能完全随机产生，不能两次产生一样的包
            //且每个包都要在不同的位置上
            int randomIndex = UnityEngine.Random.Range(0, NameToStationBags[stationNum.ToString()].Count);

            //如果有的站就只有一包衣服，while loop可能会成为死循环
            if(NameToStationBags[stationNum.ToString()].Count != 1)
            {
                while (previousIndex == randomIndex)
                {
                    randomIndex = UnityEngine.Random.Range(0, NameToStationBags[stationNum.ToString()].Count);
                }
            }
                
            print("bagNum = " + bagNum);
            
            //如果拿了第一包衣服，那么再产生的包要出现在第一包衣服而不是第三包
            int emptyNum = 0;
            int firstEmptyPos = 0;
            for (int i = 0; i < 3; i++)
            {
                if (bagPosAvailable[i] == false)//get the first empty bag position
                {
                    firstEmptyPos = i;
                    break;
                }
                else
                {
                    emptyNum++;
                }   
            }
            
            //only generate a new bag if there is an empty position
            if(emptyNum < 3)
            {
                bag = Instantiate(NameToStationBags[stationNum.ToString()][randomIndex], bagPos[firstEmptyPos],
                    Quaternion.identity) as Button;
                //this position is occupied by a bag
                bagPosAvailable[firstEmptyPos] = true;

                bag.GetComponent<ClothToMachine>().myBagPosition = firstEmptyPos;
                bagNum++;
                bag.transform.SetParent(clothBagGroup.transform, false);
            }
            previousIndex = randomIndex;
        }
    }
    
    public void trainMove()
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

    public void trainStop()
    {
        isMoving = false;
        bagFirst = true;
    }
    
    IEnumerator MyCoroutine(float time)
    {
        //This is a coroutine
        //train stays at a station for 30 seconds
        yield return new WaitForSeconds (time);
        currentStation++;

    }

    private void NumberRecalculate(float realTimer, TextMeshProUGUI text)
    {
        
        if (realTimer < 0)
        {
            realTimer = 0;
        }
        
        if (Mathf.RoundToInt(timer / 60) < 10)
        {
            if (Mathf.RoundToInt(realTimer % 60) < 10)
            {
                text.text = "0" + Mathf.RoundToInt(realTimer / 60).ToString() + ":" + "0" +
                                Mathf.RoundToInt(realTimer % 60).ToString();
            }
            else
            {
                text.text = "0" + Mathf.RoundToInt(realTimer / 60).ToString() + ":" +
                                Mathf.RoundToInt(realTimer % 60).ToString();
            }
        }
        else
        {
            if (Mathf.RoundToInt(realTimer % 60) < 10)
            {
                text.text = Mathf.RoundToInt(realTimer / 60).ToString() + ":" + "0" +
                                Mathf.RoundToInt(realTimer % 60).ToString();
            }
            else
            {
                text.text = Mathf.RoundToInt(realTimer / 60).ToString() + ":" +
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
            for (int i = 0; i < NameToStationBags["0"].Count; i++)
            {
                //show and close the UI
                if (FinalCameraController.AllStationClothList.ContainsKey(NameToStationBags["0"][i].gameObject.tag))
                {
                    isDetailed = !isDetailed;
                    
                    break;//only change the variable once
                }
            }
            
            //对某一站的每一个包而言
            for (int u = 0; u < NameToStationBags["0"].Count; u++)
            {
                //get the clothes inside
                //对每一个包的每一件衣服而言
                print("asdfasdf = " + FinalCameraController.AllStationClothList.ContainsKey(NameToStationBags["0"][u].gameObject.tag));
                if(FinalCameraController.AllStationClothList.ContainsKey(NameToStationBags["0"][u].gameObject.tag))
                {
                    for (int q = 0;
                        q < FinalCameraController.AllStationClothList[NameToStationBags["0"][u].gameObject.tag].Count;
                        q++)
                    {
                        AllDetailList[u][q].enabled = true;
                        AllDetailList[u][q].sprite =
                            FinalCameraController.AllStationClothList[NameToStationBags["0"][u].gameObject.tag][q];
                        
                    }
                }
            }
            
            if (isDetailed)
            {
                Show(dSR1);
                Show(dSR2);
            }
            else
            {
                Hide(dSR1);
                Hide(dSR2);
            }
           
//            
//            //if the machine has been opened
//            //现在这里假设第一站就只会有两包固定的衣服，因此只需要检测这两包固定的衣服是否已被放入
//            if (FinalCameraController.AllStationClothList.ContainsKey(NameToStationBags["0"][i].gameObject.tag))
//            {
//                //get the clothes inside
//                for (int i = 0; i < FinalCameraController.AllStationClothList[clothBag1.tag].Count; i++)
//                {
//                    detailList0[i].enabled = true;
//                    detailList0[i].sprite = FinalCameraController.AllStationClothList[clothBag1.tag][i];
//                }
//
//                //show canvas group
//                if (isDetailed)
//                {
//                    Hide(dSR1);
//                }
//                else
//                {
//                    Show(dSR1);
//                }
//            }
//            
//            //check bag2
//            if (FinalCameraController.AllStationClothList.ContainsKey(clothBag2.tag))
//            {
//                //get the clothes inside
//                for (int i = 0; i < FinalCameraController.AllStationClothList[clothBag2.tag].Count; i++)
//                {
//                    detailList1[i].enabled = true;
//                    detailList1[i].sprite = FinalCameraController.AllStationClothList[clothBag2.tag][i];
//                }
//                //show canvas group
//                if (isDetailed)
//                {
//                    Hide(dSR2);
//                }
//                else
//                {
//                    Show(dSR2);
//                }
//            }
        }
        
    }
    
    public void Hide(CanvasGroup UIGroup) {
        UIGroup.alpha = 0f; //this makes everything transparent
        UIGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
        UIGroup.interactable = false;
    }
    
    public void Show(CanvasGroup UIGroup) {
        UIGroup.alpha = 1f;
        UIGroup.blocksRaycasts = true;
        UIGroup.interactable = true;
    }
    
    
}

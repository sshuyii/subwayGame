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

    private int NpcCount = 0;
    private LevelManager LevelManager;
    public int currentStation;
    public GameObject highlight;
    private SpriteRenderer hSR;
    
    public GameObject arrow;
    private SpriteRenderer aSR;

    public float doorMovement;
    public float doorWidth;

    public bool outsideStation = true;
    public CanvasGroup clear;

    public List<bool> bagPosAvailable = new List<bool>();
    //public List<GameObject> arrows;

//    public List<Vector3> stationPos;

    public List<GameObject> highlights;

    //all the bags in the car
    public List<GameObject> bagsInCar;
    public bool noSameBag;
    
    public List<Vector3> bagPos;
    public List<Button> clothBags;
    public bool bagFirst = true;
    public GameObject clothBagGroup;

    private List<CanvasGroup> DetailCG = new List<CanvasGroup>();
    public CanvasGroup dSR1;
    public CanvasGroup dSR2;


    //check how many bags are in the car
    public int firstEmptyPos = 0;
    
    //a list recording all the bags on the train that haven't been taken into the washing machine
    private Dictionary<string, bool> AllBagsTaken = new Dictionary<string, bool>();
    public int bagNum = 0;
    
    private Dictionary<string, int> allStation = new Dictionary<string, int>();
    public List<string> stationNames;

    public bool isMoving = false;//if true, the train is moving
    
    //doors
    public RectTransform left1;
    public RectTransform right1;
    
//    public RectTransform left2;
//    public GameObject right2;
    
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
    public Sprite transparent;
    
  
    public List<Dictionary<String, List<Sprite>>> allStationList = new List<Dictionary<String, List<Sprite>>>();

    //one station has one list recording the potential cloth bags
    public List<Button> bagStation0;
    public List<Button> bagStation1;
    public List<Button> bagStation2;

    private Dictionary<string, List<Button>> NameToStationBags = new Dictionary<string, List<Button>>();

    public TextMeshProUGUI CountDownTimer;
    private float stationTimer;
    private float realTimer;

    public Button NPCBag;

    public bool alreadyStation1;
    public bool alreadyStation2;
    public bool alreadyStation0;
    
    // Start is called before the first frame update
    void Start() 
    {

        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();

        if(!FinalCameraController.isTutorial)
        {
            LevelManager = FinalCameraController.LevelManager;

            bagPosAvailable.Add(false);
            bagPosAvailable.Add(false);
            bagPosAvailable.Add(false);

            noSameBag = true;

            aSR = arrow.GetComponent<SpriteRenderer>();
            hSR = highlight.GetComponent<SpriteRenderer>();
        }

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
//        CountDownTimer.text = "";
        
        if(!FinalCameraController.isTutorial)
        {
            //get all the doors position when game starts
            left1Pos = left1.anchoredPosition.x;
            right1Pos = right1.anchoredPosition.x;
//        left2Pos = left2.transform.position.x;
//        right2Pos = right2.transform.position.x;
        }

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

        //Invoke("trainMove", stayTime);


    }


    private Button clothBag1;
    private Button clothBag2;
    private Button clothBag3;

    public float newTimer1;
    public float newTimer2;


    void emptyBagPos()
    {
        //记录第一个画面没有放包的地方
        for (int i = 0; i < 3; i++)
        {
            if (bagPosAvailable[i] == false)//get the first empty bag position
            {
                firstEmptyPos = i;
                break;
            }
           
        }   
    }
    // Update is called once per frame
    void Update()
    {
        //如果快进，那么train要跑到最近的那一站

        
        //test if have already arrived at stations
        //for poster generation
        //current station means the station the train is heading to
        if (currentStation == 2 && !alreadyStation1)
        {
            alreadyStation1 = true;
        }
        else if (currentStation == 0 && !alreadyStation2 && alreadyStation1)
        {
            alreadyStation2 = true;
        }
        else if (currentStation == 1 && alreadyStation2 && alreadyStation1)
        {
            alreadyStation0 = true;
        }
//        print("FinalCameraController.AllStationClothList.Count  =" + FinalCameraController.AllStationClothList.Count);
        //正式游戏
        if (!FinalCameraController.isTutorial && LevelManager.clicktime > 6)
        {
            //instead of InvokeRepeating
            //newTimer1记录在站内应该停留多长时间
            if (!isMoving)
            {
                newTimer1 += Time.deltaTime;
            }
            if (newTimer1 > stayTime)
            {
                trainMove();
                newTimer1 = 0;
            }

            //如果停在一站+走完下一站，现在马上要进入新的一站了
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

            if (!FinalCameraController.isTutorial)
            {
//                NumberRecalculate(realTimer, ClothCountDownText);

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

//            highlight.transform.position = highlights[currentStation].transform.position;
//            highlight.transform.rotation = Quaternion.Euler(new Vector3(
//                highlights[currentStation].transform.rotation.eulerAngles.x,
//                highlights[currentStation].transform.rotation.eulerAngles.y,
//                highlights[currentStation].transform.rotation.eulerAngles.z));

//            aSR.enabled = false;
//            hSR.enabled = true;

                //open doors
                if (left1.anchoredPosition.x > left1Pos - doorWidth)
                {
                    left1.anchoredPosition -= new Vector2(doorMovement, 0);
                    print("opening left door");
                }
                else
                {
                    left1.anchoredPosition = new Vector2(left1Pos - doorWidth, left1.anchoredPosition.y);
                }

                if (right1.anchoredPosition.x < right1Pos + doorWidth)
                {
                    right1.anchoredPosition += new Vector2(doorMovement, 0);
                }
                else
                {
                    right1.anchoredPosition = new Vector2(right1Pos + doorWidth, right1.anchoredPosition.y);

                    print("bagFirst =" + bagFirst);
                    print("FinalCameraController.ChapterOneEnd = " + FinalCameraController.ChapterOneEnd);
                    
                    //don't generate new bags if there are already three bags in the car
                    //bagfirst保证只运行一次
                    if (!FinalCameraController.isTutorial && bagFirst && !FinalCameraController.ChapterOneEnd)//结束的时候不能往车上放新的包了
                    {
                        print("NameToStationBags[currentStation.ToString()].Count = " + NameToStationBags[currentStation.ToString()].Count );
                        for (int i = 0; i < NameToStationBags[currentStation.ToString()].Count; i++)
                        {
                            print("NameToStationBags[currentStation.ToString()].Count" +
                                  NameToStationBags[currentStation.ToString()].Count);
                            GenerateBag(currentStation);


                            //最后一项的时候不产生包了
                            if (i == NameToStationBags[currentStation.ToString()].Count - 1)
                            {
                                bagFirst = false;
                            }
                        }
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
            }

            else
            {
                if (left1.anchoredPosition.x < left1Pos)
                {
                    left1.anchoredPosition += new Vector2(doorMovement, 0);
                }
                else
                {
                    left1.anchoredPosition = new Vector2(left1Pos, left1.anchoredPosition.y);
                }

//
                if (right1.anchoredPosition.x > right1Pos)
                {
                    right1.anchoredPosition -= new Vector2(doorMovement, 0);
                }
                else
                {
                    right1.anchoredPosition = new Vector2(right1Pos, right1.anchoredPosition.y);
                }
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

    }

    Button bag;
    private int previousIndex = 2;

   

   
    //this is used to create new bags in the car
    void GenerateBag(int stationNum)
    {
        print("generateBag");
        //如果现在车里不够三个包才产生新包
        if (bagNum < 3)
        {
            //注意：这里不能完全随机产生，不能两次产生一样的包
            //且每个包都要在不同的位置上
            //现在每站只有一个包了，就不需要randomIndex
            //int randomIndex = UnityEngine.Random.Range(0, NameToStationBags[stationNum.ToString()].Count);

            //如果有的站就只有一包衣服，while loop可能会成为死循环
//            if(NameToStationBags[stationNum.ToString()].Count != 1)
//            {
//                while (previousIndex == randomIndex)
//                {
//                    randomIndex = UnityEngine.Random.Range(0, NameToStationBags[stationNum.ToString()].Count);
//                }
//            }
                
            //print("bagNum = " + bagNum);
            emptyBagPos();

          
            
            //print("tag = " + NameToStationBags[stationNum.ToString()][randomIndex].tag);
            
            //检查车上是不是已经有这个人的包了
            for (int r = 0; r < bagsInCar.Count; r++)
            {
                //当有的站有两个特殊角色的包
                //每个站只能有一个npc的包
//                if (bagsInCar[r].CompareTag(NameToStationBags[stationNum.ToString()][randomIndex].tag))
                //现在每站只有一个特殊角色的包了，所以只需要是0就可以了
                if (bagsInCar[r].CompareTag(NameToStationBags[stationNum.ToString()][0].tag))//如果车里已经有同样tag的包
                {
                    print("NoSameBag = false;");
                    noSameBag = false;
                    //如果不放进来特殊角色的包，放npc的包
                    //前提是上一次进来的npc的包不在车里
                    string temp = "Npc" + currentStation.ToString();

                    if (!bagsInCar[r].CompareTag(temp))//如果车里没有上一个npc的包,那么产生npc的包，然后结束generateBag
                    {
                        Button npcBag = Instantiate(NPCBag, bagPos[firstEmptyPos],
                            Quaternion.identity) as Button;

                        npcBag.gameObject.transform.tag = temp;

                        //this position is occupied by a bag
                        bagPosAvailable[firstEmptyPos] = true;

                        bagsInCar.Add(npcBag.gameObject);

                        npcBag.GetComponent<ClothToMachine>().myBagPosition = firstEmptyPos;
                        bagNum++;
                        npcBag.transform.SetParent(clothBagGroup.transform, false);
                        
                    }//如果车里有了npc的包，也有了同样tag的包那么也结束generateBag
                    return;
                }
//                else//如果车里没有同样tag的包
//                {
//                    print("NoSameBag = true");
//                    noSameBag = true;
//                }
            }

            //不需要noSameBag这个bool，因为如果有一样包的话，本来也就直接return，结束generateBag了，不会进行到这一步
            //only generate a new bag if there is an empty position
            //如果同一个tag的包已经在车厢里了，那么就不要放进来这个人的包:noSameBag
          
//                if(noSameBag)
//                {
                    //首先一定产生特殊npc的包
                    bag = Instantiate(NameToStationBags[stationNum.ToString()][0], bagPos[firstEmptyPos],
                        Quaternion.identity) as Button;

                    //this position is occupied by a bag
                    bagPosAvailable[firstEmptyPos] = true;

                    bagsInCar.Add(bag.gameObject);

                    bag.GetComponent<ClothToMachine>().myBagPosition = firstEmptyPos;
                    bagNum++;
                    bag.transform.SetParent(clothBagGroup.transform, false);
                    
                    //然后可能产生npc bag
                    //it is decided in the function whether there's an empty position for bag, so no worries for that 
                    GenerateNpcBag();
//                }
                
                    
                

            }
            //previousIndex = randomIndex;
        
    }
    void GenerateNpcBag()
    {
        //如果现在车里不够三个包
        if (bagNum < 3)
        {
            emptyBagPos();
            //only generate a new bag if there is an empty position
            //如果同一个tag的包已经在车厢里了，那么就不要放进来这个人的包:noSameBag
            if (UnityEngine.Random.Range(0f, 10f) < 8f) //给npc bag 调高一点概率
            {
                bag = Instantiate(NPCBag, bagPos[firstEmptyPos],
                    Quaternion.identity) as Button;
                
                string temp = "Npc" + currentStation.ToString();
                bag.gameObject.transform.tag = temp;
                
                //this position is occupied by a bag
                        bagPosAvailable[firstEmptyPos] = true;
                
                        bagsInCar.Add(bag.gameObject);
                
                        bag.GetComponent<ClothToMachine>().myBagPosition = firstEmptyPos;
                        bagNum++;
                        bag.transform.SetParent(clothBagGroup.transform, false);
            }
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
        bagFirst = true;
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
    
    public bool isDetailed = false;
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

        isDetailed = !isDetailed;

       
        //if in the tutorial, only show disco cloth
        if (FinalCameraController.isTutorial)
        {
            if (isDetailed)
            {
                Show(dSR1);
            }
            else{Hide(dSR1);}
        }
        else
        {
            for (int i = 0; i < NameToStationBags[stationNum.ToString()].Count; i++)
            {
                //show and close the UI
                if (FinalCameraController.AllStationClothList.ContainsKey(NameToStationBags[stationNum.ToString()][i]
                    .gameObject.tag))
                {
//                    isDetailed = !isDetailed;
                    break; //only change the variable once
                }
            }

            //todo:要specify这件衣服到底是哪个站上车的，问题在于显示完了之后没有清空！
            if (FinalCameraController.AllStationClothList.Count == 0)//if all clothes are returned
            {
                //if level one has just started
                if (!FinalCameraController.isTutorial && FinalCameraController.LevelManager.clicktime == 5)
                {
                    FinalCameraController.LevelManager.clicktime = 6;
                }
                if (isDetailed)
                {
                    Show(clear);
                     
                }
                else
                {
                    Hide(clear);
                    
                }
            }
            else
            {
                //对某一站的每一个包而言
                for (int u = 0; u < NameToStationBags[stationNum.ToString()].Count; u++)
                {
                    //get the clothes inside
                    //对每一个包的每一件衣服而言
                    //print("asdfasdf = " + FinalCameraController.AllStationClothList.ContainsKey(NameToStationBags[stationNum.ToString()][u].gameObject.tag));
                    //如果某件衣服的tag等于这个包的tag
                    if (FinalCameraController.AllStationClothList.ContainsKey(
                        NameToStationBags[stationNum.ToString()][u]
                            .gameObject.tag))
                    {
                        for (int q = 0;
                            q < FinalCameraController
                                .AllStationClothList[NameToStationBags[stationNum.ToString()][u].gameObject.tag].Count;
                            q++)
                        {
                            AllDetailList[u][q].enabled = true;
                            AllDetailList[u][q].sprite =
                                FinalCameraController.AllStationClothList[
                                    NameToStationBags[stationNum.ToString()][u].gameObject.tag][q];
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
                    //显示完了清空  
                    for (int i = 0; i < AllDetailList.Count; i++)
                    {
                        for (int q = 0; q < AllDetailList[i].Count; q++)
                        {
                            AllDetailList[i][q].sprite = transparent;
                        }
                    }
                }
            }
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

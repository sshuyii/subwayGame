using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{

//    public CanvasGroup startScreen;
    public CanvasGroup chapterOne;
    public GameObject hintArrow;
    public CanvasGroup GoBackSubway;
    private CanvasGroup hintArrowCG;
    public CanvasGroup clear;
    public CanvasGroup subwayCG;
    public CanvasGroup Stations;
    public GameObject car;
    private CanvasGroup carCG;
    private PathFollower PathFollower;
    public GameObject bubble;
    
    
    
    public GameObject station;
    public GameObject map;
    public GameObject bagIn;
    public Sprite bagOut;
    private FinalCameraController FinalCameraController;

    public GameObject InstructionBubble;
    public CanvasGroup arrowButton;

    public CanvasGroup instructionCG;
    public TextMeshProUGUI instructionText;

    public bool isInstruction;
    private Vector2 stationV = new Vector2(-50, 80);

    public GameObject Instructions;
    public bool skip;
    public CanvasGroup fishBubble;
    public TextMeshProUGUI fishText;
    // Start is called before the first frame update
    void Start()
    {
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();

//        instructionText = InstructionBubble.GetComponentInChildren<TextMeshProUGUI>();
        hintArrowCG = hintArrow.GetComponent<CanvasGroup>();

        PathFollower = car.GetComponent<PathFollower>();
        carCG = car.GetComponent<CanvasGroup>();
        
        if (skip)
        {
            clicktime = 6;
            Instructions.SetActive(false);
            Show(subwayCG);
        }
        else
        {
            isInstruction = true;
            Hide(GoBackSubway);
            Hide(subwayCG);
        
//            Show(startScreen);

            Instructions.SetActive(true);
            map.SetActive(false);
            bagIn.SetActive(false);
            bubble.SetActive(false);
            Show(chapterOne);
            Show(arrowButton);
            Hide(Stations);
            Hide(carCG);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (clicktime == 6)//已经点开了station detail
        {
            Hide(instructionCG);
            bubble.SetActive(false);
            Show(GoBackSubway); 
            Hide(hintArrowCG);
            PathFollower.isInstruction = true;
        }
        else if (clicktime == 7 && FinalCameraController.mySubwayState == FinalCameraController.SubwayState.One)//鱼开始说话
        {
            Show(fishBubble);
            StartCoroutine(AnimateText(fishText, "Karara! See the bags? Time for work!", false, null, Vector2.zero));//clicktime = 6;
            //Show(arrowButton);
            
            Hide(arrowButton);
            Hide(clear);
        }
        
    }

    private bool lastTextFinish;

    public IEnumerator AnimateText(TextMeshProUGUI text, string textContent, bool showArrow, GameObject hintObject,
        Vector2 arrowPosition)
    {
        clicktime++;
        lastTextFinish = false;
        Hide(hintArrowCG);

        for (int i = 0; i < (textContent.Length + 1); i++)
        {
            text.text = textContent.Substring(0, i);
            yield return new WaitForSeconds(.01f);

            if (i == textContent.Length)
            {
                yield return new WaitForSeconds(.1f);

                lastTextFinish = true;
                if (showArrow)
                {
                    Show(hintArrowCG);
                    hintArrow.transform.SetParent(hintObject.transform);
                    hintArrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
                }
                else
                {
                    Hide(hintArrowCG);
                }
                
                //train starts to move
                if (clicktime == 7)
                {
                    isInstruction = false;
                }
                else if (clicktime == 5)
                {
                    Show(arrowButton);
                    Hide(arrowButton);  
                    Show(Stations);
                }
            }
        }
    }

    IEnumerator StartChapterOne()
    {
        
        Hide(chapterOne);
        
        map.SetActive(true);//显示地图
        bubble.SetActive(true);

        FinalCameraController.ChangeToMap();
        yield return new WaitForSeconds(0.5f);

       
        Show(instructionCG);
        StartCoroutine(AnimateText(instructionText, "The laundromat is in a subway car", false, null, Vector2.zero));//clicktime = 1;
        Show(arrowButton);
        Show(carCG);


//        instructionText.text = "Clothes are delivered in at each station.";
    }

    public int clicktime = 0;
    
    public void ClickInstruction()
    {
        if (clicktime == 0)
        {
            //once start, show a screen
            StartCoroutine(StartChapterOne());
        }
        else if (clicktime == 1 && lastTextFinish)
        {
            StartCoroutine(AnimateText(instructionText, "Customers drop their bags at each station", false, null, Vector2.zero));//clicktime = 2;
            bagIn.SetActive(true);

            //包箭头指向某一站
//            instructionText.text = "All of them need to be returned before the train reaches the same station for the second time.";

        }
        else if (clicktime == 2 && lastTextFinish)
        {
            StartCoroutine(AnimateText(instructionText, "Train moves and clothes need to be washed ", false, null, Vector2.zero));//clicktime = 3;
            bagIn.SetActive(false);
            PathFollower.isInstruction = true;

            //包箭头指向某一站

//            instructionText.text = "All of them need to be returned before the train reaches the same station for the second time.";

        }
        else if (clicktime == 3 && lastTextFinish)
        {
            StartCoroutine(AnimateText(instructionText, "Return clothes when the car reaches the same station for the second time", false, null, Vector2.zero));//clicktime = 4;
//            instructionText.text = "You can check each station's clothes here.";
            bagIn.SetActive(true);
            bagIn.GetComponent<SpriteRenderer>().sprite = bagOut;
        }
        else if(clicktime == 4 && lastTextFinish)
        {
            StartCoroutine(AnimateText(instructionText, "Check each station's clothes here.", true, station, stationV));//clicktime = 5;
            bagIn.SetActive(false);
            Show(hintArrowCG);
            Hide(arrowButton);  

        }
        else if(clicktime == 5 && lastTextFinish)//点了station detail
        {
            
        }
        //应该用不上
        else if (clicktime == 7 && lastTextFinish)//鱼开始说话
        {
            StartCoroutine(AnimateText(fishText, "See the bags? Time for work!", false, null, Vector2.zero));//clicktime = 7;
            Hide(arrowButton);
            Hide(clear);
        }
        //点击回地铁按钮就回到地铁
        
        //if chapter one ends
        if (FinalCameraController.ChapterOneEnd)//点击之后进入第二章
        {
            SceneManager.LoadScene("StreetStyleTwo", LoadSceneMode.Single);
            print("loadSceneTwo");
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

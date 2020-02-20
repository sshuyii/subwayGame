using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class WasherController : MonoBehaviour
{

    public int number;
    public Image emptyImage;
    public Image fullImage;
    public AllMachines AllMachines;

    public Animator ClothUiAnimator;
    
    public bool alreadyNotice;

    public Collider myCollider;
    
    private Animator myAnimator;

    public CanvasGroup backgroundUI;
    public CanvasGroup backgroundUI2;
    public CanvasGroup backgroundUI3;


    public Animator lightAnimator;
    public CanvasGroup ClothUI;

    public Text timerNum;
    private float realTimer;
    
    public bool isFirstOpen = true;

    public Image DoorImage;

    public AllMachines.MachineState myMachineState;
    
    private FinalCameraController FinalCameraController;
    private CalculateInventory CalculateInventory;


//    private Button[] btns;
    private int shut = 0;
    //public Vector3 offsetTap;
    //public GameObject subway;

    private float timer = 0;

    public GameObject[] buttons;
    private bool fulltemp = false;

    
    // Start is called before the first frame update
    void Start()
    {
        
        myMachineState = AllMachines.MachineState.empty;

        myAnimator = GetComponentInChildren<Animator>();
        
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();
        CalculateInventory = GameObject.Find("---InventoryController").GetComponent<CalculateInventory>();



    }

    public void CloseFullNotice()
    {
        Hide(CalculateInventory.InventoryFull);
    }

    public void ClickStart()
    {
        if(myMachineState == AllMachines.MachineState.full)
        {
            myMachineState = AllMachines.MachineState.washing;
            //for tutorial
            if (FinalCameraController.isTutorial)
            {
                FinalCameraController.TutorialManager.tutorialNumber = 5;
            }

//            print("pressssssssed");
        }
    }
    
    
    // Update is called once per frame
    void Update()
    {

        realTimer = AllMachines.washTime - timer;
        if (Mathf.RoundToInt(timer / 60) < 10)
        {
            if (Mathf.RoundToInt(realTimer % 60) < 10)
            {
                timerNum.text = "0" + Mathf.RoundToInt(realTimer / 60).ToString() + ":" + "0" +
                                Mathf.RoundToInt(realTimer % 60).ToString();
            }
            else
            {
                timerNum.text = "0" + Mathf.RoundToInt(realTimer / 60).ToString() + ":" +
                                Mathf.RoundToInt(realTimer % 60).ToString();
            }
        }
        else
        {
            if (Mathf.RoundToInt(realTimer % 60) < 10)
            {
                timerNum.text = Mathf.RoundToInt(realTimer / 60).ToString() + ":" + "0" +
                                Mathf.RoundToInt(realTimer % 60).ToString();
            }
            else
            {
                timerNum.text = Mathf.RoundToInt(realTimer / 60).ToString() + ":" +
                                Mathf.RoundToInt(realTimer % 60).ToString();
            }
        }
        //if already five items taken
        if (CalculateInventory.occupiedI > 5 && fulltemp == false)
        {
            Hide(ClothUI);
            Show(CalculateInventory.InventoryFull);
            fulltemp = true;
        }

        if (CalculateInventory.occupiedI < 5)
        {
            fulltemp = false;
        }
        
        
        
        //close all ui if swipping the screen
        if (FinalCameraController.isSwipping)
        {
            Destroy(FinalCameraController.generatedNotice);
            FinalCameraController.alreadyNotice = false;
            Hide(ClothUI);
            FinalCameraController.alreadyClothUI = false;

        }
        
        if (myMachineState == AllMachines.MachineState.empty)
        {
            fullImage.enabled = false;
            emptyImage.enabled = true;

        }
        else if (myMachineState == AllMachines.MachineState.full)
        {
            emptyImage.enabled = false;
            fullImage.enabled = true;

        }
        else
        {
            fullImage.enabled = true;
        }
        
        //if click a bag of cloth, put them into the machine and start washing
        if (myMachineState == AllMachines.MachineState.washing)
        {
            myAnimator.SetBool("isWashing", true);
            lightAnimator.SetBool("isWashing", true);
            
            emptyImage.enabled = false;
            
            timer += Time.deltaTime;
            
            
            if (timer > AllMachines.washTime)
            {
                myMachineState = AllMachines.MachineState.finished;
                myAnimator.SetBool("isWashing", false);
                lightAnimator.SetBool("isWashing", false);
            
                timer = 0;
                
                //for tutorial
                if (FinalCameraController.isTutorial)
                {
                    FinalCameraController.TutorialManager.tutorialNumber = 6;
                }
            }
        }
        
   
//        
//        //any touch cancels cloth UI
//        else if(TouchController.myInputState != TouchController.InputState.None)
//        {
//            if (shut == 1)
//            {
//                shut = 0;
//                //subway.transform.position += offsetTap;
//                Hide(ClothUI);
//                //backgroundSR.enabled = false;
//            }
//        }

        
        
    }

    IEnumerator MachineUnfold()
    {
        Show(backgroundUI);
        yield return new WaitForSeconds(0.4f);
        Show(backgroundUI2);
        yield return new WaitForSeconds(0.4f);
        Show(backgroundUI3);
        yield return new WaitForSeconds(0.4f);


    }
    
    IEnumerator MachineFold()
    {
        Hide(backgroundUI3);
        yield return new WaitForSeconds(0.4f);
        Hide(backgroundUI2);
        yield return new WaitForSeconds(0.4f);
        Hide(backgroundUI);
        yield return new WaitForSeconds(0.4f);


    }
    
    public void CancelPanel()
    {
        Hide(ClothUI);
        Hide(backgroundUI);

        shut = 0;
    }

    public void clickMachine()
    {
        print("presssssssed");
        if (myMachineState == AllMachines.MachineState.finished)
        {
           
            if (shut == 0)
            {
                shut++;

                StartCoroutine(MachineUnfold());
                
                //change door to open
               
                DoorImage.sprite = AllMachines.openedDoor;
                
                //ClothUiAnimator.SetBool("isUnfold",true);

                StartCoroutine("WaitFor2Seconds");

                //Hide(AllClothUI);

                
                GenerateCloth(this.transform.gameObject.tag);
                
                //for tutorial
                if (FinalCameraController.isTutorial)
                {
                    FinalCameraController.TutorialManager.tutorialNumber = 8;
                    
                    FinalCameraController.TutorialManager.door.GetComponent<Image>().material.DisableKeyword("SHAKEUV_ON");
                    FinalCameraController.TutorialManager.cloth.GetComponent<Image>().material.DisableKeyword("SHAKEUV_ON");

                }
                
            }
            //if click machine again, close UI
            else if (shut == 1)
            {
                shut = 0;
                Hide(ClothUI);
                StartCoroutine(MachineFold());
                //ClothUiAnimator.SetBool("isUnfold",false);
                
                //change door to closed
                DoorImage.sprite = AllMachines.closedDoor;


            }
        }

    }

    IEnumerator WaitFor2Seconds()
    {
        yield return new WaitForSeconds(1);
        Show(ClothUI);

    }
    
    void Hide(CanvasGroup UIGroup) {
        UIGroup.alpha = 0f; //this makes everything transparent
        UIGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
        UIGroup.interactable = false;


        if (UIGroup == ClothUI)
        {
            FinalCameraController.alreadyClothUI = false;
        }
//            foreach (Button btn in btns)
//            {
//                btn.enabled = false;
//            }            
    }
    
    void Show(CanvasGroup UIGroup) {
        UIGroup.alpha = 1f;
        UIGroup.blocksRaycasts = true;
        UIGroup.interactable = true;
        
        if (UIGroup == ClothUI)
        {
            FinalCameraController.alreadyClothUI = true;
            FinalCameraController.currentClothUI = ClothUI;
        }

            
//            foreach (Button btn in btns)
//            {
//                btn.enabled = true;
//            }
            
    }

   
    
    public void GenerateCloth(string tagName)
    {
        //need to get all button reset, because they are disabled when clicked last time
        
        List<Sprite> MachineClothList = new List<Sprite>();

        //randomly generate clothes when player opens the machine
        if(isFirstOpen)
        { 

            if(tagName == "Alex")
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    int randomIndex = Random.Range(0, AllMachines.alexClothesTemp.Count);

//                  Button ClothInMachine =
//                  Instantiate(alexClothesTemp[randomIndex], buttonPositions[i], Quaternion.identity) as Button;
                    buttons[i].GetComponent<Button>().enabled = true;
                    buttons[i].tag = "Alex";
                    Image buttonImage = buttons[i].GetComponent<Image>();
                    buttonImage.enabled = true;
                    buttonImage.sprite = AllMachines.alexClothesTemp[randomIndex];


                    AllMachines.alexClothesTemp.Remove(AllMachines.alexClothesTemp[randomIndex]);
                    //as child of the folder
                    //doesn't work for some reason I don't understand
                    //ClothInMachine.transform.SetParent(MachineGroup.transform, false);
                    //ClothInMachine.transform.SetParent(MachineGroup.transform, false);

                    //record into List
                    MachineClothList.Add(buttonImage.sprite);
                }
                    
            }
            else if (tagName == "Bella")
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    int randomIndex = Random.Range(0, AllMachines.bellaClothesTemp.Count);
                        print("random = " + randomIndex);
                        print(AllMachines.bellaClothesTemp.Count);
                        buttons[i].tag = "Bella";

//                  clothesInMachine[i].image.sprite = AlexClothes[randomIndex];

//                    Button ClothInMachine =
//                        Instantiate(bellaClothesTemp[randomIndex], buttonPositions[i], Quaternion.identity) as Button;

                        Image buttonImage = buttons[i].GetComponent<Image>();
                        buttonImage.enabled = true;

                        buttonImage.sprite = AllMachines.bellaClothesTemp[randomIndex];

                        AllMachines.bellaClothesTemp.Remove(AllMachines.bellaClothesTemp[randomIndex]);

                        //record into List
                    MachineClothList.Add(buttonImage.sprite);

                        //as child of the folder
                        //doesn't work for some reason I don't understand

                        //ClothInMachine.transform.SetParent(MachineGroup.transform, false);
                }
                    
            }
                
                //record every button into this dictionary
                
               
                //this will make scale a lot bigger than it should be
//              ClothInMachine.transform.parent = MachineGroup.transform;

            

        }

         isFirstOpen = false;
         //record the list into dictionary
         FinalCameraController.AllStationClothList.Add(tagName, MachineClothList);

    }
        
       

    
}

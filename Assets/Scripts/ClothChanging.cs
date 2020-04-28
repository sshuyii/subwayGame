using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class ClothChanging : MonoBehaviour
{
    
    public Text text;
    private Vector3 startPos;
    public Sprite transparent;

    public Image checkImage;
    public GameObject crossImage;

    public CanvasGroup Occupied;

    //used for lont tap
    private bool tapStart = false;
        
    private bool firstTime;
    
    private CalculateInventory CalculateInventory;
    private FinalCameraController FinalCameraController;
    

    private GameObject InventoryController;

    private bool hittingBody;
    //this dictionary is the player inventory

    //a list that stores UI location
    private Sprite currentSprite;
  
    
    //drag
    private float dist;
    private bool dragging = false;
    private Vector3 offset;
    private Transform toDrag;

    private Button selfButton;

    private List<Button> inventoryButtonList;

    private TouchController TouchController;

    private Sprite startSprite;
    private Image myImage;


    private AllMachines AllMachines;
    // Start is called before the first frame update
    void Start()
    {
        
        InventoryController = GameObject.Find("---InventoryController");
        TouchController = GameObject.Find("---TouchController").GetComponent<TouchController>();
        AllMachines = GameObject.Find("---ClothInMachineController").GetComponent<AllMachines>();
        FinalCameraController = GameObject.Find("Main Camera").GetComponent<FinalCameraController>();



        CalculateInventory = InventoryController.GetComponent<CalculateInventory>();
        startPos = transform.position;

        inventoryButtonList = new List<Button>();

        firstTime = true;
//        selfButton.onClick.AddListener(AddClothToInventory);

        //currentSprite = GetComponent<SpriteRenderer>().sprite;

        myImage = GetComponent<Image>();

        startSprite = GetComponent<Image>().sprite;
    
        selfButton = GetComponent<Button>();


        for (var i = 0; i < CalculateInventory.inventory.Count; i++)
        {
            inventoryButtonList.Add(CalculateInventory.inventory[i].GetComponent<Button>());
        }
        

    }

    // Update is called once per frame
    void Update()
    {

        int inventoryTemp = 0;
        for (int i = 0; i < CalculateInventory.inventory.Count; i++)
        {
            
            if (CalculateInventory.inventory[i].GetComponent<Image>().sprite.name != "arrow")
            {
                inventoryTemp++;

            }
        }

        CalculateInventory.occupiedI = inventoryTemp;
        //for long tap
        if (tapStart)
        {
            if (TouchController.isLongTap)
            {
                crossImage.SetActive(true);
                tapStart = false;

                CalculateInventory.isreturning = true;
            }
        }
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            Vector3 pos = touch.position;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(pos);

            Vector3 v3;

            if (Input.touchCount != 1)
            {
                dragging = false;
                return;
            }


            if (touch.phase == TouchPhase.Began)
            {
                if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Draggable"))
                {
                    Debug.Log("Here");
                    toDrag = hit.transform;
                    dist = hit.transform.position.z - Camera.main.transform.position.z;
                    v3 = new Vector3(pos.x, pos.y, dist);
                    v3 = Camera.main.ScreenToWorldPoint(v3);
                    offset = toDrag.position - v3;
                    dragging = true;
                }
            }

            if (dragging && touch.phase == TouchPhase.Moved)
            {
                v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
                v3 = Camera.main.ScreenToWorldPoint(v3);
                toDrag.position = v3 + offset;
            }

            if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
            {
                dragging = false;
                //print("touch ends");
                if (hittingBody)
                {
                    ChangeCloth();
                    
        
                }
            }
        }

    }
        

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            hittingBody = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            hittingBody = false;
        }
    }


    public void ReturnClothPre()
    {
        tapStart = true;
    }

    public void ReturnCloth()
    {
        print("return cloth is working");
        if(FinalCameraController.TouchController.doubleTouch)
        {
            print("return cloth is working and double touched");
            this.tag = "untagged";

            for (int i = 0; i < AllMachines.WashingMachines.Count; i++)
            {
                if (this.CompareTag(AllMachines.WashingMachines[i].tag))
                {
                    var buttonList = AllMachines.WashingMachines[i].GetComponent<WasherController>().buttons;
                    for (int a = 0; a < buttonList.Length; a++)
                    {
                        if (buttonList[a].GetComponent<Image>().enabled == false)
                        {
                            buttonList[a].GetComponent<Image>().enabled = true;
                            myImage.sprite = startSprite;
                            //crossImage.SetActive(false);

                            takeOffCloth();
                            CalculateInventory.occupiedI = CalculateInventory.occupiedI - 1;
                            AllMachines.WashingMachines[i].GetComponent<WasherController>().clothNum++;
                            break;
                        }
                    }
                }
            }
        }
    }

    public void ReturnClothNo()
    {
        crossImage.SetActive(false);
    }


    private void takeOffCloth() 
    {
        if (currentSprite.name.Contains("Top"))
        {
            CalculateInventory.wearingTop = false;

            //change inventory clothes
            CalculateInventory.topSR.sprite = transparent;
               

            //change subway clothes
            CalculateInventory.topSSR.sprite = transparent;
                
            CalculateInventory.whiteShirt.enabled = true;
                                
            CalculateInventory.whiteShirtS.enabled = true;

                
            //change clothes in advertisement
            CalculateInventory.topASR.sprite = transparent;
                
            CalculateInventory.whiteShirtASR.sprite = CalculateInventory.allAdCloth["WhiteShirt"];
        }
        else if (currentSprite.name.Contains("Bottom"))
        {
            CalculateInventory.wearingBottom = false;

            //change inventory clothes
            CalculateInventory.otherSR.sprite = transparent;
               

            //change subway clothes
            CalculateInventory.otherSSR.sprite = transparent;
                
            CalculateInventory.blackPants.enabled = true;
                                
            CalculateInventory.blackPantsS.enabled = true;

                
            //change clothes in advertisement
            CalculateInventory.otherASR.sprite = transparent;
                
            CalculateInventory.blackPantsASR.sprite = CalculateInventory.allAdCloth["BlackPants"];

        }
        else if(currentSprite.name.Contains("Shoe"))
        {
            CalculateInventory.wearingShoe = false;

            //change inventory clothes
            CalculateInventory.shoeSR.sprite = transparent;
               

            //change subway clothes
            CalculateInventory.shoeSSR.sprite = transparent;
                
//            blackPants.enabled = true;
//                                
//            blackPantsS.enabled = true;

                
            //change clothes in advertisement
            CalculateInventory.shoeASR.sprite = transparent;
                
//            CalculateInventory.blackPantsASR.sprite = CalculateInventory.allCloth["BlackPants"];

        }
        else if(currentSprite.name.Contains("Everything"))
        {
            CalculateInventory.wearingEverything = false;

            //change inventory clothes
            CalculateInventory.everythingSR.sprite = transparent;
               

            //change subway clothes
            CalculateInventory.everythingSSR.sprite = transparent;
                
            CalculateInventory.blackPants.enabled = true;        
            CalculateInventory.blackPantsS.enabled = true;
            CalculateInventory.whiteShirt.enabled = true;    
            CalculateInventory.whiteShirtS.enabled = true;

                
            //change clothes in advertisement
            CalculateInventory.everythingASR.sprite = transparent;
                
            CalculateInventory.blackPantsASR.sprite = CalculateInventory.allAdCloth["BlackPants"];
            CalculateInventory.whiteShirtASR.sprite = CalculateInventory.allAdCloth["WhiteShirt"];


        }
       
    }
    
    public void ChangeCloth()
    {
        if (FinalCameraController.TouchController.doubleTouch == false)
        {
            if (CalculateInventory.isreturning)
            {
                CancelReturn();
                CalculateInventory.isreturning = false;
            }

            //for tutorial
            if (FinalCameraController.isTutorial)
            {
                //show the button to return to subway
                FinalCameraController.Show(FinalCameraController.TutorialManager.closetSubway);

                FinalCameraController.TutorialManager.tutorialNumber = 14;

                CalculateInventory.everythingSR.sprite = CalculateInventory.discoCloset;
                CalculateInventory.everythingSSR.sprite = CalculateInventory.discoSubway;

                CalculateInventory.shoeASR.sprite = CalculateInventory.discoAd;
                CalculateInventory.workCloth.enabled = false;
                
                CalculateInventory.workClothASR.enabled = false;
                CalculateInventory.workClothS.enabled = false;
                
                CalculateInventory.blackPants.enabled = false;
                CalculateInventory.whiteShirt.enabled = false;

                CalculateInventory.whiteShirtS.enabled = false;
                CalculateInventory.blackPantsS.enabled = false;
                
                FinalCameraController.TutorialManager.wearNothing = false;
                return;
            }

            //currentSprite = GetComponenft<SpriteRenderer>().sprite;
            currentSprite = GetComponent<Image>().sprite;


            if (CalculateInventory.allCloth.ContainsKey(currentSprite.name))
            {

                //checkImage.enabled = true;

                if (currentSprite.name.Contains("Top"))
                {
                   
                    //buttonChangeBack();
                    CalculateInventory.wearingTop = true;
                    CalculateInventory.wearingEverything = false;


                    //record the button
                    CalculateInventory.topButton = selfButton;

                    //change inventory clothes
                    CalculateInventory.topSR.sprite = CalculateInventory.allCloth[currentSprite.name];
                    print(currentSprite.name);

                    //change subway clothes
                    CalculateInventory.topSSR.sprite = CalculateInventory.allSubwayCloth[currentSprite.name];



                    CalculateInventory.whiteShirt.enabled = false;
                    CalculateInventory.workCloth.enabled = false;


                    CalculateInventory.workClothS.enabled = false;
                    CalculateInventory.whiteShirtS.enabled = false;


                    //change clothes in advertisement
                    print("Given key = " + currentSprite.name);
                    CalculateInventory.topASR.sprite = CalculateInventory.allAdCloth[currentSprite.name];

                    //Take off workcloth
                    CalculateInventory.workClothASR.enabled = false;
                    CalculateInventory.whiteShirtASR.sprite = transparent;
                    //CalculateInventory.blackPantsASR.sprite = CalculateInventory.allAdCloth["BlackPants"];


                    //take off one piece
                    CalculateInventory.everythingSR.sprite = transparent;
                    CalculateInventory.everythingASR.sprite = transparent;
                    CalculateInventory.everythingSSR.sprite = transparent;

                    
                    //change the tag on all clothes on karara
                    CalculateInventory.topSR.gameObject.tag = this.tag;
                    CalculateInventory.topASR.gameObject.tag = this.tag;
                    CalculateInventory.topSSR.gameObject.tag = this.tag;

                    
                    //player talks
                    if (currentSprite.name.Contains("A1"))
                    {
                        //text.text = "<b>Karara</b>: Do people wear Hawaii shirts in Hawaii?";
                    }

                }
                else if (currentSprite.name.Contains("Bottom"))
                {
                    //buttonChangeBack();

                    CalculateInventory.wearingBottom = true;
                    CalculateInventory.wearingEverything = false;


                    //record the button
                    CalculateInventory.bottomButton = selfButton;


                    CalculateInventory.otherSR.sprite = CalculateInventory.allCloth[currentSprite.name];
                    CalculateInventory.otherSSR.sprite = CalculateInventory.allSubwayCloth[currentSprite.name];
                    //change clothes in advertisement
                    CalculateInventory.otherASR.sprite = CalculateInventory.allAdCloth[currentSprite.name];
                    //Take off workcloth
                    CalculateInventory.workClothASR.enabled = false;
                    //CalculateInventory.whiteShirtASR.sprite = CalculateInventory.allAdCloth["WhiteShirt"];
                    CalculateInventory.blackPantsASR.sprite = transparent;


                    //take off one piece
                    CalculateInventory.everythingSR.sprite = transparent;
                    CalculateInventory.everythingASR.sprite = transparent;
                    CalculateInventory.everythingSSR.sprite = transparent;


                    //change the tag on all clothes on karara
                    CalculateInventory.otherSR.gameObject.tag = this.tag;
                    CalculateInventory.otherASR.gameObject.tag = this.tag;
                    CalculateInventory.otherSSR.gameObject.tag = this.tag;
                    print("change bottom");

                    CalculateInventory.workCloth.enabled = false;
                    CalculateInventory.blackPants.enabled = false;

                    CalculateInventory.workClothS.enabled = false;
                    CalculateInventory.blackPantsS.enabled = false;

                }
                else if (currentSprite.name.Contains("Shoe"))
                {
                    //buttonChangeBack();

                    CalculateInventory.wearingShoe = true;

                    //record the button
                    CalculateInventory.shoeButton = selfButton;


                    CalculateInventory.shoeSR.sprite = CalculateInventory.allCloth[currentSprite.name];
                    CalculateInventory.shoeSSR.sprite = CalculateInventory.allSubwayCloth[currentSprite.name];
                    //change clothes in advertisement
                    CalculateInventory.shoeASR.sprite = CalculateInventory.allAdCloth[currentSprite.name];

                    print("change shoe");

                    CalculateInventory.workShoeSR.enabled = false;
                    CalculateInventory.workShoeSSR.enabled = false;
                    CalculateInventory.workShoeASR.sprite = transparent;
                    
                     
                    //change the tag on all clothes on karara
                    CalculateInventory.shoeSR.gameObject.tag = this.tag;
                    CalculateInventory.shoeASR.gameObject.tag = this.tag;
                    CalculateInventory.shoeSSR.gameObject.tag = this.tag;

                }
                else if (currentSprite.name.Contains("Everything"))
                {
                    //buttonChangeBack();
                    

                    CalculateInventory.wearingEverything = true;
                    CalculateInventory.wearingTop = false;
                    CalculateInventory.wearingBottom = false;

                    //record the button
                    CalculateInventory.everythingButton = selfButton;

                    
                    CalculateInventory.everythingSR.sprite = CalculateInventory.allCloth[currentSprite.name];
                    CalculateInventory.everythingSSR.sprite = CalculateInventory.allSubwayCloth[currentSprite.name];
                    //change clothes in advertisement
                    CalculateInventory.everythingASR.sprite = CalculateInventory.allAdCloth[currentSprite.name];
                    //Take off workcloth
                    CalculateInventory.workClothASR.enabled = false;

                    //take off other cloths
                    CalculateInventory.otherSR.sprite = transparent;
                    CalculateInventory.topSR.sprite = transparent;

                    CalculateInventory.otherASR.sprite = transparent;
                    CalculateInventory.topASR.sprite = transparent;

                    CalculateInventory.otherSSR.sprite = transparent;
                    CalculateInventory.topSSR.sprite = transparent;


                    print("change everything");

                    CalculateInventory.workCloth.enabled = false;

                    CalculateInventory.workClothS.enabled = false;

                    CalculateInventory.topSR.sprite = transparent;
                    CalculateInventory.otherSR.sprite = transparent;

                    CalculateInventory.blackPants.enabled = true;
                    CalculateInventory.whiteShirt.enabled = true;

                    CalculateInventory.whiteShirtS.enabled = true;
                    CalculateInventory.blackPantsS.enabled = true;

                    CalculateInventory.whiteShirtASR.sprite = CalculateInventory.allAdCloth["WhiteShirt"];
                    CalculateInventory.blackPantsASR.sprite = CalculateInventory.allAdCloth["BlackPants"];

                     
                    //change the tag on all clothes on karara
                    CalculateInventory.everythingSR.gameObject.tag = this.tag;
                    CalculateInventory.everythingASR.gameObject.tag = this.tag;
                    CalculateInventory.everythingSSR.gameObject.tag = this.tag;
                }
                else
                {
                    //if click on the question marks
                    checkImage.enabled = true;
                    selfButton.enabled = false;

                    firstTime = false;
                }
            }
        }
    }

    public void ChangeWorkCloth()
    {
        if (FinalCameraController.isTutorial)
        {
//            FinalCameraController.TutorialManager.tutorialNumber = 13;
            FinalCameraController.TutorialManager.kararaText.text = "Workwear No";

        }

        CancelReturn();

//        workCloth.enabled = !workCloth.enabled;
//        workClothS.enabled = !workClothS.enabled;
//        CalculateInventory.workClothASR.enabled = !CalculateInventory.workClothASR.enabled;

        CalculateInventory.wearingWorkCloth = !CalculateInventory.wearingWorkCloth;


        CalculateInventory.topSR.sprite = transparent;
        CalculateInventory.otherSR.sprite = transparent;
        CalculateInventory.everythingSR.sprite = transparent;
        CalculateInventory.shoeSR.sprite = transparent;

        CalculateInventory.topSSR.sprite = transparent;
        CalculateInventory.otherSSR.sprite = transparent;
        CalculateInventory.everythingSSR.sprite = transparent;
        CalculateInventory.shoeSSR.sprite = transparent;


        CalculateInventory.topASR.sprite = transparent;
        CalculateInventory.otherASR.sprite = transparent;
        CalculateInventory.everythingASR.sprite = transparent;
        CalculateInventory.shoeASR.sprite = transparent;



        CalculateInventory.blackPants.enabled = true;
        CalculateInventory.whiteShirt.enabled = true;

        CalculateInventory.whiteShirtS.enabled = true;
        CalculateInventory.blackPantsS.enabled = true;


        if (CalculateInventory.wearingWorkCloth)
        {
            CalculateInventory.workShoeASR.enabled = true;

            CalculateInventory.whiteShirtASR.sprite = CalculateInventory.allAdCloth["WhiteShirt"];
            CalculateInventory.blackPantsASR.sprite = CalculateInventory.allAdCloth["BlackPants"];
            CalculateInventory.workShoeASR.sprite = CalculateInventory.allAdCloth["WorkShoe"];
            CalculateInventory.workShoeSR.enabled = true;
            CalculateInventory.workShoeSSR.enabled = true;

            CalculateInventory.workCloth.enabled = true;
            CalculateInventory.workClothS.enabled = true;
            CalculateInventory.workClothASR.enabled = true;

            if (FinalCameraController.isTutorial)
            {
                FinalCameraController.TutorialManager.wearNothing = false;
            }
        }
        else
        {
            CalculateInventory.workShoeASR.enabled = false;
            CalculateInventory.workShoeSR.enabled = false;
            CalculateInventory.workShoeSSR.enabled = false;

            CalculateInventory.workCloth.enabled = false;
            CalculateInventory.workClothS.enabled = false;
            CalculateInventory.workClothASR.enabled = false;
            
            CalculateInventory.whiteShirtASR.sprite = CalculateInventory.allAdCloth["WhiteShirt"];
            CalculateInventory.blackPantsASR.sprite = CalculateInventory.allAdCloth["BlackPants"];

            if (FinalCameraController.isTutorial)
            {
                FinalCameraController.TutorialManager.wearNothing = true;

            }


//        //enable all buttons
//        for (var i = 0; i < inventoryButtonList.Count; i++)
//        {
//            inventoryButtonList[i].enabled = true;
//        }
//        
//        //disable all checks
//        for (var i = 0; i < CalculateInventory.inventoryCheckList.Count; i++)
//        {
//            CalculateInventory.inventoryCheckList[i].enabled = false;
//        }

        }
    }

    public void CancelReturn()
    {
        foreach (var returnImage in CalculateInventory.returnImageList)
        {
            returnImage.enabled = false;
        }
    }
    private void buttonChangeBack()
    {

        if (CalculateInventory.allCloth.ContainsKey(currentSprite.name))
        {
            if (currentSprite.name.Contains("Top") || currentSprite.name != CalculateInventory.topSR.sprite.name)
            {
                if (CalculateInventory.wearingTop)
                {
                    print("toppppppp");
                    CalculateInventory.topButton.enabled = true;
                    CalculateInventory.topButton.GetComponent<ClothChanging>().checkImage.enabled = false;
                }
                else if (CalculateInventory.wearingEverything)
                {
                    //set everything button back
                    CalculateInventory.everythingButton.enabled = true;
                    CalculateInventory.everythingButton.GetComponent<ClothChanging>().checkImage.enabled = false;
                }
            }
            else if (currentSprite.name.Contains("Bottom") || currentSprite.name != CalculateInventory.otherSR.sprite.name)
            {
                if (CalculateInventory.wearingBottom)
                {
                    CalculateInventory.bottomButton.enabled = true;
                    CalculateInventory.bottomButton.GetComponent<ClothChanging>().checkImage.enabled = false;
                }

                else if (CalculateInventory.wearingEverything)
                {
                    //set everything button back
                    CalculateInventory.everythingButton.enabled = true;
                    CalculateInventory.everythingButton.GetComponent<ClothChanging>().checkImage.enabled = false;
                }
            }
            else if (currentSprite.name.Contains("Shoe") && CalculateInventory.wearingShoe)
            {
                CalculateInventory.shoeButton.enabled = true;
                CalculateInventory.shoeButton.GetComponent<ClothChanging>().checkImage.enabled = false;
            }
            else if (currentSprite.name.Contains("Everything") ||
                     currentSprite.name != CalculateInventory.everythingSR.sprite.name)
            {
                {
                    if (CalculateInventory.wearingTop)
                    {
                        //set top and bottom buttons back
                        CalculateInventory.topButton.enabled = true;
                        CalculateInventory.topButton.GetComponent<ClothChanging>().checkImage.enabled = false;
                    }

                    if (CalculateInventory.wearingBottom)
                    {
                        CalculateInventory.bottomButton.enabled = true;
                        CalculateInventory.bottomButton.GetComponent<ClothChanging>().checkImage.enabled = false;
                    }

                    if (CalculateInventory.wearingEverything)
                    {

                        CalculateInventory.everythingButton.enabled = true;
                        CalculateInventory.everythingButton.GetComponent<ClothChanging>().checkImage.enabled = false;
                    }

                }

            }


//        //get the worn inventory back to the cloth the character is wearing
//        for (int i = 0; i < CalculateInventory.inventory.Count; i++)
//        {
//            if (CalculateInventory.inventory[i].GetComponent<Image>().sprite == worn 
//                && topOrBottomOrShoe != null)
//            {
//                CalculateInventory.inventory[i].GetComponent<Image>().sprite =
//                    CalculateInventory.allClothUI[topOrBottomOrShoe.name];
//
//                break;
//
//            }
//        }
        }


    }
}


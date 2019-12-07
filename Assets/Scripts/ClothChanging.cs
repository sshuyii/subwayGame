using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothChanging : MonoBehaviour
{
    public Text text;
    private Vector3 startPos;
    public Sprite transparent;

    public Image checkImage;

    
    private bool firstTime;
    
    private CalculateInventory CalculateInventory;

    private GameObject InventoryController;

    private bool hittingBody;
    //this dictionary is the player inventory

    //a list that stores UI location
    private Sprite currentSprite;
    public SpriteRenderer workCloth;

    public SpriteRenderer whiteShirt;
    public SpriteRenderer blackPants;
    
    //subway clothes
    public Image workClothS;

    public Image whiteShirtS;
    public Image blackPantsS;
    
    //drag
    private float dist;
    private bool dragging = false;
    private Vector3 offset;
    private Transform toDrag;

    private Button selfButton;

    private List<Button> inventoryButtonList;
    
    // Start is called before the first frame update
    void Start()
    {
        InventoryController = GameObject.Find("---InventoryController");
        CalculateInventory = InventoryController.GetComponent<CalculateInventory>();
        startPos = transform.position;

        inventoryButtonList = new List<Button>();

        firstTime = true;
//        selfButton.onClick.AddListener(AddClothToInventory);

        //currentSprite = GetComponent<SpriteRenderer>().sprite;
        
        currentSprite = GetComponent<Image>().sprite;

        selfButton = GetComponent<Button>();


        for (var i = 0; i < CalculateInventory.inventory.Count; i++)
        {
            inventoryButtonList.Add(CalculateInventory.inventory[i].GetComponent<Button>());
        }
        

    }

    // Update is called once per frame
    void Update()
    {
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

    public void ChangeCloth()
    {
        //currentSprite = GetComponenft<SpriteRenderer>().sprite;
        currentSprite = GetComponent<Image>().sprite;


        if(CalculateInventory.allCloth.ContainsKey(currentSprite.name))
        {
            
            checkImage.enabled = true;

            if(currentSprite.name.Contains("Top"))
            {
                
                buttonChangeBack();

                //record the button
                CalculateInventory.topButton = selfButton;
                
                
                //change inventory clothes
                CalculateInventory.topSR.sprite = CalculateInventory.allCloth[currentSprite.name];
                print(currentSprite.name);

                //change subway clothes
                CalculateInventory.topSSR.sprite = CalculateInventory.allSubwayCloth[currentSprite.name];
                
                whiteShirt.enabled = false;
                workCloth.enabled = false;
                
                
                workClothS.enabled = false;
                whiteShirtS.enabled = false;

                
                //change clothes in advertisement
                CalculateInventory.topASR.sprite = CalculateInventory.allAdCloth[currentSprite.name];

                
                
                //player talks
                if(currentSprite.name.Contains("A1"))
                {
                    text.text = "<b>Karara</b>: Do people wear Hawaii shirts in Hawaii?";
                }
                
                

            }
            else if(currentSprite.name.Contains("Bottom"))
            {
                buttonChangeBack();

                //record the button
                CalculateInventory.bottomButton = selfButton;
                

                CalculateInventory.otherSR.sprite = CalculateInventory.allCloth[currentSprite.name];
                CalculateInventory.otherSSR.sprite = CalculateInventory.allSubwayCloth[currentSprite.name];
                //change clothes in advertisement
                CalculateInventory.otherASR.sprite = CalculateInventory.allAdCloth[currentSprite.name];

                print("change bottom");
                
                workCloth.enabled = false;
                blackPants.enabled = false;
                
                workClothS.enabled = false;
                blackPantsS.enabled = false;

            }
            else if(currentSprite.name.Contains("Shoe"))
            {
                buttonChangeBack();

                //record the button
                CalculateInventory.shoeButton = selfButton;
                

                CalculateInventory.shoeSR.sprite = CalculateInventory.allCloth[currentSprite.name];
                CalculateInventory.shoeSSR.sprite = CalculateInventory.allSubwayCloth[currentSprite.name];
                //change clothes in advertisement
                CalculateInventory.shoeASR.sprite = CalculateInventory.allAdCloth[currentSprite.name];

                print("change shoe");
               
            }
            else if(currentSprite.name.Contains("Everything"))
            {
                buttonChangeBack();
            
                //record the button
                CalculateInventory.everythingButton = selfButton;
                

                CalculateInventory.everythingSR.sprite = CalculateInventory.allCloth[currentSprite.name];
                CalculateInventory.everythingSSR.sprite = CalculateInventory.allSubwayCloth[currentSprite.name];
                //change clothes in advertisement
                CalculateInventory.everythingASR.sprite = CalculateInventory.allAdCloth[currentSprite.name];

                
                print("change everything");
                
                workCloth.enabled = false;
                
                workClothS.enabled = false;

                CalculateInventory.topSR.sprite = transparent;
                CalculateInventory.otherSR.sprite = transparent;

                blackPants.enabled = true;
                whiteShirt.enabled = true;

                whiteShirtS.enabled = true;
                blackPantsS.enabled = true;


            }
            else
            {
                checkImage.enabled = true;
                selfButton.enabled = false;

                firstTime = false;
            }
        }
        else
        {
            
            //now the button cannot be pressed
          
            
//            //when the button is workCloth
//            workCloth.enabled = !workCloth.enabled;
//            workClothS.enabled = !workClothS.enabled;
//
//
//
//            CalculateInventory.topSR.sprite = null;
//            CalculateInventory.otherSR.sprite = null;
//            CalculateInventory.everythingSR.sprite = null;
//        
//            blackPants.enabled = true;
//            whiteShirt.enabled = true;
//
//
//            whiteShirtS.enabled = true;
//            blackPantsS.enabled = true;
        }
        //GetComponent<Image>().sprite = worn;
        //transform.position = startPos;

        
       

    }

    public void ChangeWorkCloth()
    {
        workCloth.enabled = !workCloth.enabled;
        workClothS.enabled = !workClothS.enabled;


      
        CalculateInventory.topSR.sprite = transparent;
        CalculateInventory.otherSR.sprite = transparent;
        CalculateInventory.everythingSR.sprite = transparent;

        CalculateInventory.topSSR.sprite = transparent;
        CalculateInventory.otherSSR.sprite = transparent;
        CalculateInventory.everythingSSR.sprite = transparent;

        
        
        blackPants.enabled = true;
        whiteShirt.enabled = true;

        whiteShirtS.enabled = true;
        blackPants.enabled = true;
        
        //enable all buttons
        for (var i = 0; i < inventoryButtonList.Count; i++)
        {
            inventoryButtonList[i].enabled = true;
            
        }
        
        //disable all checks
        for (var i = 0; i < CalculateInventory.inventoryCheckList.Count; i++)
        {
            CalculateInventory.inventoryCheckList[i].enabled = false;
        }
        
    }

    private void buttonChangeBack()
    {
        
            if (CalculateInventory.allCloth.ContainsKey(currentSprite.name))
            {
                if (currentSprite.name.Contains("Top") && whiteShirt.enabled == false)
                {
                    print("toppppppp");
                    CalculateInventory.topButton.enabled = true;
                    CalculateInventory.topButton.GetComponent<ClothChanging>().checkImage.enabled = false;
                }
                else if (currentSprite.name.Contains("Bottom") && blackPants.enabled == false)
                {
                    CalculateInventory.bottomButton.enabled = true;
                    CalculateInventory.bottomButton.GetComponent<ClothChanging>().checkImage.enabled = false;
                }
                else if (currentSprite.name.Contains("Shoe"))
                {
                    CalculateInventory.shoeButton.enabled = true;
                    CalculateInventory.shoeButton.GetComponent<ClothChanging>().checkImage.enabled = false;
                }
                else if (currentSprite.name.Contains("Everything") && workCloth.enabled == false)
                {
                    CalculateInventory.everythingButton.enabled = true;
                    CalculateInventory.everythingButton.GetComponent<ClothChanging>().checkImage.enabled = false;
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


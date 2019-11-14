using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothChanging : MonoBehaviour
{
    public Text text;
    private Vector3 startPos;
    public Sprite worn;
    
    private CalculateInventory CalculateInventory;

    private GameObject InventoryController;

    private bool hittingBody;
    //this dictionary is the player inventory

    //a list that stores UI location
    private Sprite currentSprite;
    public SpriteRenderer workCloth;

    public SpriteRenderer whiteShirt;
    public SpriteRenderer blackPants;
    
    //drag
    private float dist;
    private bool dragging = false;
    private Vector3 offset;
    private Transform toDrag;
   

    // Start is called before the first frame update
    void Start()
    {
        InventoryController = GameObject.Find("---InventoryController");
        CalculateInventory = InventoryController.GetComponent<CalculateInventory>();
        startPos = transform.position;

//        selfButton.onClick.AddListener(AddClothToInventory);

        currentSprite = GetComponent<SpriteRenderer>().sprite;


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
        currentSprite = GetComponent<SpriteRenderer>().sprite;

        if(CalculateInventory.allCloth.ContainsKey(currentSprite.name))
        {
            
            
            if(currentSprite.name.Contains("Top"))
            {
                buttonChangeBack(CalculateInventory.topSR.sprite);
                
                CalculateInventory.topSR.sprite = CalculateInventory.allCloth[currentSprite.name];
                print("change top");
                
                whiteShirt.enabled = false;
                workCloth.enabled = false;
                
                //player talks
                if(currentSprite.name.Contains("A1"))
                {
                    text.text = "<b>Karara</b>: Do people wear Hawaii shirts in Hawaii?";
                }
                
                

            }
            else if(currentSprite.name.Contains("Bottom"))
            {
                buttonChangeBack(CalculateInventory.otherSR.sprite);

                CalculateInventory.otherSR.sprite = CalculateInventory.allCloth[currentSprite.name];
                print("change bottom");
                
                workCloth.enabled = false;

                blackPants.enabled = false;
            }
            else if(currentSprite.name.Contains("shoe"))
            {
                buttonChangeBack(CalculateInventory.shoeSR.sprite);

                CalculateInventory.shoeSR.sprite = CalculateInventory.allCloth[currentSprite.name];
                print("change shoe");
            }
            else if(currentSprite.name.Contains("Everything"))
            {
                buttonChangeBack(CalculateInventory.everythingSR.sprite);

                CalculateInventory.everythingSR.sprite = CalculateInventory.allCloth[currentSprite.name];
                print("change everything");
                
                workCloth.enabled = false;
                CalculateInventory.topSR.sprite = null;
                CalculateInventory.otherSR.sprite = null;

                blackPants.enabled = true;
                whiteShirt.enabled = true;
            }
        }
        else
        {
            workCloth.enabled = !workCloth.enabled;


            CalculateInventory.topSR.sprite = null;
            CalculateInventory.otherSR.sprite = null;
            CalculateInventory.everythingSR.sprite = null;
        
            blackPants.enabled = true;
            whiteShirt.enabled = true;
        }
        GetComponent<SpriteRenderer>().sprite = worn;
        transform.position = startPos;
    }

    public void ChangeWorkCloth()
    {
        workCloth.enabled = !workCloth.enabled;


        CalculateInventory.topSR.sprite = null;
        CalculateInventory.otherSR.sprite = null;
        CalculateInventory.everythingSR.sprite = null;
        
        blackPants.enabled = true;
        whiteShirt.enabled = true;
    }

    public void buttonChangeBack(Sprite topOrBottomOrShoe)
    {
        //get the worn inventory back to the cloth the character is wearing
        for (int i = 0; i < CalculateInventory.inventory.Count; i++)
        {
            if (CalculateInventory.inventory[i].GetComponent<SpriteRenderer>().sprite == worn 
                && topOrBottomOrShoe != null)
            {
                CalculateInventory.inventory[i].GetComponent<SpriteRenderer>().sprite =
                    CalculateInventory.allClothUI[topOrBottomOrShoe.name];
                        
            }
        }
    }
    
}


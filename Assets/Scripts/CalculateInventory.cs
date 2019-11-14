using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculateInventory : MonoBehaviour
{
    public Dictionary<string, Sprite> allCloth = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> allClothUI = new Dictionary<string, Sprite>();


    public List<Sprite> allClothList;
    //all the clothes
 
    public List<Sprite> allClothUIList;
    
    public int occupiedI = 0;


    public GameObject player;

    //clothes the character is wearing
    public SpriteRenderer topSR;
    public SpriteRenderer shoeSR;
    public SpriteRenderer otherSR;
    public SpriteRenderer everythingSR;

    
    //three slots above
    public List<GameObject> inventory;

    public List<Sprite> inventorySprite;
//    public Button top;
//    public Button shoe;
//    public Button other;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < allClothList.Count; i++)
        {
            allCloth.Add(allClothList[i].name, allClothList[i]);
        }

        for (int i = 0; i < allClothUIList.Count; i++)
        {
            allClothUI.Add(allClothUIList[i].name, allClothUIList[i]);
        }
        
        
        for (int i = 0; i < inventory.Count; i++)
        {
            //inventory used to be buttons
            //inventorySR.Add(inventory[i].GetComponent<Button>().image.sprite);
            
            inventorySprite.Add(inventory[i].GetComponent<SpriteRenderer>().sprite);

        }


    }

    // Update is called once per frame
    void Update()
    {
    
        
    }
    
//    public void ChangeCloth()
//    {
//        Sprite buttonSprite = gameObject.GetComponent<Button>().image.sprite;
//        
//        if(allCloth.ContainsKey(buttonSprite.name))
//        {
//            if(buttonSprite.name.Contains("Top"))
//            {
//                topSR.sprite = allCloth[buttonSprite.name];
//                print("change top");
//            }
//            else if(buttonSprite.name.Contains("Bottom"))
//            {
//                otherSR.sprite = allCloth[buttonSprite.name];
//                print("change bottom");
//            }
//            else if(buttonSprite.name.Contains("shoe"))
//            {
//                shoeSR.sprite = allCloth[buttonSprite.name];
//                print("change shoe");
//            }
//        }    
//    }
    
//    public void ChangeShoe()
//    {
//        if(allCloth.ContainsKey(shoe.image.sprite.name))
//        {
//            shoeSR.sprite = allCloth[shoe.image.sprite.name];
//        }    
//    }
//    
//    public void ChangeOther()
//    {
//        if (allCloth.ContainsKey(other.image.sprite.name))
//        {
//            otherSR.sprite = allCloth[other.image.sprite.name];
//
//        }
//    }
}

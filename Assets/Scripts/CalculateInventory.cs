using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class CalculateInventory : MonoBehaviour
{
    public int posNum = 0;
    public bool isreturning;

    public bool wearingTop;
    public bool wearingBottom;
    public bool wearingEverything;
    public bool wearingShoe;
    public bool wearingWorkCloth;

    public CanvasGroup InventoryFull;
    
    public Sprite transparent;

    public List<Image> returnImageList;
    
    public Dictionary<string, Sprite> allCloth = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> allClothUI = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> allSubwayCloth = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> allAdCloth = new Dictionary<string, Sprite>();

    public List<Dictionary<string, Sprite>> postureDictionaryList = new List<Dictionary<string, Sprite>>();

    //all clothes for each postures
    public List<Sprite> ClothPos0;
    public List<Sprite> ClothPos1;
    public List<Sprite> ClothPos2;

    public Dictionary<string, Sprite> AllClothPos0 = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> AllClothPos1 = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> AllClothPos2 = new Dictionary<string, Sprite>();

    
    //all the clothes
    public List<Sprite> allClothUIList;
    public List<Sprite> allClothList;
    public List<Sprite> allSubwayClothList;

    
    public int occupiedI = 0;

//    public List<Image> checks;

    //clothes the character is wearing
    public SpriteRenderer topSR;
    public SpriteRenderer shoeSR;
    public SpriteRenderer otherSR;
    public SpriteRenderer everythingSR;
    public SpriteRenderer workShoeSR;
    
    //clothes the character wears on subway
    public Image topSSR;
    public Image shoeSSR;
    public Image otherSSR;
    public Image everythingSSR;
    public Image workShoeSSR;

    //clothes the character wears in advertisement
    public Image topASR;
    public Image shoeASR;
    public Image otherASR;
    public Image everythingASR;
    public Image workClothASR;
    public Image whiteShirtASR;
    public Image blackPantsASR;
    public Image workShoeASR;


    //closet buttons as a record
    public Button topButton;
    public Button bottomButton;
    public Button shoeButton;
    public Button everythingButton;

    
//    public List<Image> inventoryCheckList;

    
    //six slots on the right
    public List<GameObject> inventory;
    

    public List<Sprite> inventorySprite;
//    public Button top;
//    public Button shoe;
//    public Button other;
    
    // Start is called before the first frame update
    
    //this is for tutorial
    public Sprite discoSubway;
    public Sprite discoCloset;
    public Sprite discoAd;
    
    
    
    public SpriteRenderer workCloth;

    public SpriteRenderer whiteShirt;
    public SpriteRenderer blackPants;
    
    //subway clothes
    public Image workClothS;

    public Image whiteShirtS;
    public Image blackPantsS;
    
    //all the bags that are in the game
    public List<bool> allBagTimer;
    
    
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
        
        for (int i = 0; i < allSubwayClothList.Count; i++)
        {
            allSubwayCloth.Add(allSubwayClothList[i].name, allSubwayClothList[i]);
            print(allSubwayClothList[i].name);
        }
        
        
        for (int i = 0; i < inventory.Count; i++)
        {
            //inventory used to be buttons
            inventorySprite.Add(inventory[i].GetComponent<Button>().image.sprite);
            
            //inventorySprite.Add(inventory[i].GetComponent<SpriteRenderer>().sprite);

        }

        for (int i = 0; i < ClothPos1.Count; i++)
        {
            AllClothPos0.Add(ClothPos0[i].name, ClothPos0[i]);
            AllClothPos1.Add(ClothPos1[i].name, ClothPos1[i]);
            AllClothPos2.Add(ClothPos2[i].name, ClothPos2[i]);
            allAdCloth.Add(ClothPos0[i].name, ClothPos0[i]);

        }

        postureDictionaryList.Add(AllClothPos0);
        postureDictionaryList.Add(AllClothPos1);
        postureDictionaryList.Add(AllClothPos2);
        
        
        allAdCloth = postureDictionaryList[posNum];
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

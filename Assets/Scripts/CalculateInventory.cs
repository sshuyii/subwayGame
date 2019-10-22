using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculateInventory : MonoBehaviour
{
    public Dictionary<string, Sprite> allCloth = new Dictionary<string, Sprite>();

    //all the clothes
    public Sprite TopA1;
    public Sprite BottomA1;
    public Sprite ShoeA1;
    public Sprite ShoeA2;
    public Sprite TopB1;
    public Sprite TopB2;
    public Sprite TopB3;
    public Sprite TopB4;
    public Sprite BottomB1;
    public Sprite BottomB2;
    public Sprite ShoeB1;




    public GameObject player;

    //clothes the character is wearing
    public SpriteRenderer topSR;
    public SpriteRenderer shoeSR;
    public SpriteRenderer otherSR;

    
    //three slots above
    public Button top;
    public Button shoe;
    public Button other;
    
    // Start is called before the first frame update
    void Start()
    {
        allCloth.Add(TopA1.name, TopA1);
        allCloth.Add(TopB1.name, TopB1);
        allCloth.Add(TopB2.name, TopB2);
        allCloth.Add(TopB3.name, TopB3);
        allCloth.Add(TopB4.name, TopB4);
        allCloth.Add(BottomA1.name, BottomA1);
        allCloth.Add(BottomB1.name, BottomB1);
        allCloth.Add(BottomB2.name, BottomB2);
        allCloth.Add(ShoeA1.name, ShoeA1);
        allCloth.Add(ShoeB1.name, ShoeB1);
        allCloth.Add(ShoeA2.name, ShoeA2);




    }

    // Update is called once per frame
    void Update()
    {
    
        
    }
    
    public void ChangeTop()
    {
        print("changetop");
        if(allCloth.ContainsKey(top.image.sprite.name))
        {
            topSR.sprite = allCloth[top.image.sprite.name];
            print("changeTop to image");
        }    
    }
    
    public void ChangeShoe()
    {
        if(allCloth.ContainsKey(shoe.image.sprite.name))
        {
            shoeSR.sprite = allCloth[shoe.image.sprite.name];
        }    
    }
    
    public void ChangeOther()
    {
        if (allCloth.ContainsKey(other.image.sprite.name))
        {
            otherSR.sprite = allCloth[other.image.sprite.name];

        }
    }
}

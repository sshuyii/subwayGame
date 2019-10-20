using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculateInventory : MonoBehaviour
{
    public Dictionary<string, Sprite> allCloth = new Dictionary<string, Sprite>();

    //all the clothes
    public Sprite TopA1;
    public Sprite TopA2;


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
        allCloth.Add(TopA2.name, TopA2);

    }

    // Update is called once per frame
    void Update()
    {
    
        
    }
    
    public void ChangeTop()
    {
        if(allCloth.ContainsKey(top.image.sprite.name))
        {
            topSR.sprite = allCloth[top.image.sprite.name];
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

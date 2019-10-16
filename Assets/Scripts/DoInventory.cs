using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class DoInventory : MonoBehaviour
{
    //this dictionary is the player inventory
    public Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>();

    //a list that stores UI location
    private Sprite currentSprite;

    // Start is called before the first frame update
    void Start()
    {
        

        currentSprite = GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddClothToInventory()
    {
        inventory.Add(currentSprite.name, currentSprite);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothChanging : MonoBehaviour
{
    SpriteRenderer top;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        top = GetComponent<SpriteRenderer>();
    }

    public void ChangeClothes(Sprite futureCloth)
    {
        top.sprite = futureCloth;
    }
}


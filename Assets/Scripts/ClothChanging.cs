﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothChanging : MonoBehaviour
{
    SpriteRenderer mySR;
    // Start is called before the first frame update
    void Start()
    {
        mySR = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeClothes(Sprite futureCloth)
    {
        mySR.sprite = futureCloth;
    }
}


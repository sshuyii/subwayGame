﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeBackground : MonoBehaviour
{
    //public List<Sprite> subwayAdList;


    private InstagramController InstagramController;
    public Image photoBackground;
    
    //public Dictionary<string, Sprite> allSubAd = new Dictionary<string, Sprite>();


    
    // Start is called before the first frame update
    void Start()
    {
//        for (int i = 0; i < subwayAdList.Count; i++)
//        {
//            allSubAd.Add(subwayAdList[i].name, subwayAdList[i]);
//        }

        InstagramController = GameObject.Find("---InstagramController").GetComponent<InstagramController>();

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void clickBackground()
    {
        photoBackground.sprite = InstagramController.allBackAd[transform.name];
    }
}
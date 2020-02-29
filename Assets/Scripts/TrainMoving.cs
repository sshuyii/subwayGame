using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMoving : MonoBehaviour
{

    private bool trainMoving;
    public float speedA;

    private RectTransform myRectTransform;
    // Start is called before the first frame update
    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (trainMoving && myRectTransform.anchoredPosition.x > -1000)
        {
            myRectTransform.anchoredPosition -= new Vector2(speedA, 0);

        }
        else if(trainMoving)
        {
            myRectTransform.anchoredPosition = new Vector2(-1500, myRectTransform.anchoredPosition.y);
        }
    }

}

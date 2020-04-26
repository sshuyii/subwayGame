using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMovement : MonoBehaviour
{
    public float speedA;

    public float startingPoint;
    public float endPoint;
    // Start is called before the first frame update
    void Start()
    {
        startingPoint = transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        //for background moving
        if(transform.localPosition.x < endPoint)
        {
            transform.localPosition += new Vector3(speedA, 0, 0);
        }
        else
        {
            transform.localPosition = new Vector3(startingPoint, transform.localPosition.y, transform.localPosition.z);
        }
        
    }
}

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
        
    }

    // Update is called once per frame
    void Update()
    {
        //for background moving
        if(transform.position.x < endPoint)
        {
            transform.position += new Vector3(speedA, 0, 0);
        }
        else
        {
            transform.position += new Vector3(startingPoint, 0, 0);
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoving : MonoBehaviour
{
    public float speed;

    public float endPoint;

    private float startPoint;
    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < endPoint)
        {
            transform.position += new Vector3(speed, 0, 0);
        }
        else
        {
            transform.position -= new Vector3(endPoint - startPoint, 0, 0);
        }
        
    }
}

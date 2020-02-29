using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMoving : MonoBehaviour
{
    public float speed;

    public float endPoint;

    private float startPoint;
    private SubwayMovement SubwayMovement;
    private SpriteRenderer mySR;

    public Sprite stopSprite;

    public bool isStation;

    private Sprite myStartSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position.x;
        
        SubwayMovement = GameObject.Find("---StationController").GetComponent<SubwayMovement>();
        mySR = GetComponent<SpriteRenderer>();
        myStartSprite = mySR.sprite;
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

        if (isStation)
        {
            if(SubwayMovement.isMoving == false)
            {
                mySR.sprite = stopSprite;
            }
            else
            {
                mySR.sprite = myStartSprite;
            }
        }
        
    }
}

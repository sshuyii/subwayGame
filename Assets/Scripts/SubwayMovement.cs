using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubwayMovement : MonoBehaviour
{

    public int currentStation;
    public GameObject highlight;
    private SpriteRenderer hSR;
    
    public GameObject arrow;
    private SpriteRenderer aSR;

    public float doorMovement;
    
    public List<GameObject> arrows;

    public List<Vector3> stationPos;

    public List<Vector3> bagPos;
    public List<Button> clothBags;
    private bool bagFirst = true;
    public GameObject clothBagGroup;


    
    private Dictionary<string, int> allStation = new Dictionary<string, int>();
    public List<string> stationNames;

    public bool isMoving = false;//if true, the train is moving
    
    //doors
    public GameObject left1;
    public GameObject right1;

    public float moveTime;
    public float stayTime;


    // Start is called before the first frame update
    void Start()
    {

        aSR = arrow.GetComponent<SpriteRenderer>();
        hSR = highlight.GetComponent<SpriteRenderer>();

        
        currentStation = 0;

        //get all station names into the dictionary
        for (var i = 0; i < stationNames.Count; ++i)
        {
            allStation.Add(stationNames[i], i);
        }
        
        //train stays at the first station for 30seconds
        //isMoving = false for 30 seconds, then isMoving = true for 60 seconds
        InvokeRepeating("trainMove", stayTime, stayTime + moveTime);
        InvokeRepeating("trainStop", stayTime + moveTime, stayTime + moveTime);




    }

    // Update is called once per frame
    void Update()
    {
        
        
        
        //train needs 1 minute on its way
        print("currentStation = " + currentStation);
        print("isMoving =  " + isMoving);

        //decide which station is highlighted on screen
        if (isMoving == false)
        {
            highlight.transform.localPosition = stationPos[currentStation];
            aSR.enabled = false;
            hSR.enabled = true;

            //open doors
            if (left1.transform.position.x > -11.2)
            {
                left1.transform.position -= new Vector3(doorMovement, 0, 0);
            }
            
            if (right1.transform.position.x < -5.4)
            {
                right1.transform.position += new Vector3(doorMovement, 0, 0);
            }
            else
            {
                //if arrives at a station and door already opened
                //这里的衣服应该随机产生，现在先写成了固定两包衣服：Alex + 路人
                if (currentStation == 0 && bagFirst)
                {
                    //generate a bag of clothsPos
                    Button clothBag1 = Instantiate(clothBags[0], bagPos[0], Quaternion.identity) as Button;
                    Button clothBag2 = Instantiate(clothBags[1], bagPos[1], Quaternion.identity) as Button;
    
                    clothBag1.transform.SetParent(clothBagGroup.transform, false);
                    clothBag2.transform.SetParent(clothBagGroup.transform, false);


                    bagFirst = false;
            
                }
            }
            
            
        }
        else
        {
            aSR.enabled = true;
            hSR.enabled = false;

            arrow.transform.position = arrows[currentStation].transform.position;
            arrow.transform.rotation = Quaternion.Euler(new Vector3 (arrows[currentStation].transform.rotation.eulerAngles.x, arrows[currentStation].transform.rotation.eulerAngles.y, arrows[currentStation].transform.rotation.eulerAngles.z));
            
            //close doors
            if (left1.transform.position.x < -9)
            {
                left1.transform.position += new Vector3(doorMovement, 0, 0);
            }
            
            if (right1.transform.position.x > -7.2)
            {
                right1.transform.position -= new Vector3(doorMovement, 0, 0);
            }
        }
    }

    void trainMove()
    {
        if(currentStation < 2)
        {
            currentStation++;
        }
        else
        {
            currentStation = 0;
        }
        isMoving = true;
    }

    void trainStop()
    {
        isMoving = false;
    }
    
    IEnumerator MyCoroutine(float time)
    {
        //This is a coroutine
        //train stays at a station for 30 seconds
        yield return new WaitForSeconds (time);
        currentStation++;

    }
    
    
}

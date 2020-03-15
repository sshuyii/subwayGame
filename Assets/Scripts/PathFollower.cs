using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    // Array of waypoints to walk from one to the next one
    [SerializeField]
    private RectTransform[] waypoints;

    // Walk speed that can be set in Inspector
    [SerializeField]
    private float distance = 2f;

    // Index of current waypoint from which Enemy walks
    // to the next one
    public int waypointIndex = 0;
    private RectTransform myRT;
    private SubwayMovement SubwayMovement;

    public List<int> stationNumList;
    private float moveSpeed;
    private bool trainMove;
    private Vector3 currentEulerAngles;

    // Use this for initialization
    private void Start () {

        // Set position of Enemy as position of the first waypoint
        myRT = GetComponent<RectTransform>();
        myRT.anchoredPosition= waypoints[waypointIndex].anchoredPosition;
        
        SubwayMovement = GameObject.Find("---StationController").GetComponent<SubwayMovement>();

        moveSpeed = distance / SubwayMovement.moveTime;
        currentEulerAngles = myRT.eulerAngles;

    }
	
    // Update is called once per frame
    private void Update () {

        // Move Enemy
        Move();
    }

    private float timer;

    private bool timeToGo;
    // Method that actually make Enemy walk
    private void Move()
    {
//        print("isWorking");
        // If Enemy didn't reach last waypoint it can move
        // If enemy reached last waypoint then it stops

        

        if (!trainMove)
        {
            if (SubwayMovement.isMoving)
            {
                trainMove = true;
                moveSpeed = distance / SubwayMovement.moveTime;
            }
            else
            {
                moveSpeed = 0;
                myRT.eulerAngles = currentEulerAngles;
            }
        }


        if (waypointIndex <= waypoints.Length - 1 && trainMove)
        {
            currentEulerAngles = myRT.eulerAngles;

            //print(waypointIndex + "<1");
            // Move Enemy from current waypoint to the next one
            // using MoveTowards method
            myRT.anchoredPosition = Vector2.MoveTowards(myRT.anchoredPosition,
                waypoints[waypointIndex].anchoredPosition,
                moveSpeed * Time.deltaTime);

            //car rotates!
            var dir = waypoints[waypointIndex].anchoredPosition - myRT.anchoredPosition;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            myRT.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                
            // If Enemy reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and Enemy starts to walk to the next waypoint
            if (myRT.anchoredPosition == waypoints[waypointIndex].anchoredPosition)
            {
                waypointIndex += 1;
                
                for (int i = 0; i < stationNumList.Count; i++)
                {
                    if (waypointIndex == stationNumList[i])
                    {
                        trainMove = false;
                        break;
                    }
                }
            }
        }
        else if(trainMove)
        {
            waypointIndex = 0;
            trainMove = false;
//            print("=00000");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    // Array of waypoints to walk from one to the next one
    [SerializeField]
    private RectTransform[] waypoints;

    // Walk speed that can be set in Inspector
    [SerializeField]
    private float moveSpeed = 2f;

    // Index of current waypoint from which Enemy walks
    // to the next one
    private int waypointIndex = 0;
    private RectTransform myRT;

    // Use this for initialization
    private void Start () {

        // Set position of Enemy as position of the first waypoint
        myRT = GetComponent<RectTransform>();
        myRT.anchoredPosition= waypoints[waypointIndex].anchoredPosition;
    }
	
    // Update is called once per frame
    private void Update () {

        // Move Enemy
        Move();
    }

    // Method that actually make Enemy walk
    private void Move()
    {
//        print("isWorking");
        // If Enemy didn't reach last waypoint it can move
        // If enemy reached last waypoint then it stops
        if (waypointIndex <= waypoints.Length - 1)
        {
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
            }
        }
        else
        {
            waypointIndex = 0;
            print("=00000");
        }
    }
}
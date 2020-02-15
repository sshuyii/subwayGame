using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationForButton : MonoBehaviour
{
    public SubwayMovement SubwayMovement;
    public int stationNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pressStationForButton()
    {
        SubwayMovement.StationDetails(stationNum);
    }
}

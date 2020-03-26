using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class ClockRotate : MonoBehaviour
{
    public float zRotation;
    private SubwayMovement SubwayMovement;
    private RectTransform myRectT;
    public int bagEmptyNum = 0;
    public int machineEmptyNum = 0;

    public Image YellowClock;
    
    private AllMachines AllMachines;

    public bool fastForward;
    public CanvasGroup bubble;

    private Button myButton;
    // Start is called before the first frame update
    void Start()
    {
        SubwayMovement = GameObject.Find("---StationController").GetComponent<SubwayMovement>();
        AllMachines = GameObject.Find("---ClothInMachineController").GetComponent<AllMachines>();

        //used when the timer is always on screen
        //zRotation = 360 / (SubwayMovement.stayTime + SubwayMovement.moveTime);
        
        //now the timer is placed on the bag
        zRotation = 360 / (2 *(SubwayMovement.stayTime + SubwayMovement.moveTime));

        myRectT = GetComponent<RectTransform>();

        //SubwayMovement.Hide(bubble);
        myButton = GetComponent<Button>();

        YellowClock.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        myRectT.Rotate( new Vector3( 0, 0, -zRotation * Time.deltaTime) );
        YellowClock.fillAmount += 1/(2 *(SubwayMovement.stayTime + SubwayMovement.moveTime) + SubwayMovement.stayTime) * Time.deltaTime;
            
        bagEmptyNum = 0;
        machineEmptyNum = 0;
        //Calculate how many bagPos are available
        for (int i = 0; i < 3; i++)
        {
            if (SubwayMovement.bagPosAvailable[i] == false)
            {
                bagEmptyNum++;
                if (bagEmptyNum == 3)
                {
                    break;
                }
            }
        }
        
        //if all machines are empty
        for (int i = 0; i < 3; i++)
        {
            if (AllMachines.WashingMachines[i].GetComponent<WasherController>().myMachineState ==
                AllMachines.MachineState.empty)
            {
                machineEmptyNum++;
                if (machineEmptyNum == 3)
                {
                    break;
                }
            }
        }

        if (bagEmptyNum == 3 && machineEmptyNum == 3 && SubwayMovement.isMoving)
        {
            fastForward = true;
            //SubwayMovement.Show(bubble);
            //myButton.enabled = true;

        }
        else
        {
            fastForward = false;
            //SubwayMovement.Hide(bubble);
            //myButton.enabled = false;
        }
        
    }

    public void clickClock()
    {
        SubwayMovement.trainStop();
        SubwayMovement.newTimer2 = 0;
        SubwayMovement.newTimer1 = SubwayMovement.moveTime;
        //SubwayMovement.Hide(bubble);

    }

    public void toggleUI()
    {
//        if(fastForward)
//        {
//            if (bubble.alpha == 0f)
//            {
//                SubwayMovement.Show(bubble);
//            }
//            else
//            {
//                SubwayMovement.Hide(bubble);
//
//            }
//        }    
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public GameObject camera;

    private CameraController CameraController;

    private Vector3 offset;

    public float WaitTime;
    private IEnumerator coroutine;
    private bool isMoving;

    public SpriteRenderer playerBody;
    public SpriteRenderer playerHelmet;
    public SpriteRenderer playerCloth;
    public SpriteRenderer playerShoe;
    
    //original cloth the player is wearing
    private Sprite wornHelmet;
    private Sprite wornCloth;
    private Sprite wornShoe;
//    
//    public Sprite playerIdleBody;
//    public Sprite playerIdleCloth;
//    public Sprite playerIdleHelmet;
//    public Sprite playerIdleShoe;
//    
    public Sprite playerWalkingBody;
//    public Sprite playerWalkingCloth;
//    public Sprite playerWalkingHelmet;
//    public Sprite playerWalkingShoe;

    private enum MovingState {
        Left,
        Right,
        Idle
    }
    MovingState myMovingState;

    
    // Start is called before the first frame update
    void Start()
    {
        CameraController = camera.GetComponent<CameraController>();
        offset = transform.position - camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //no delay if using the codes below
//        Vector3 target = new Vector3(camera.transform.position.x + offset.x, camera.transform.position.y + offset.y,transform.position.z);
//        if(CameraController.shut == 0)
//        {
//            if (transform.position.x != camera.transform.position.x + offset.x)
//            {
//                transform.position = Vector3.MoveTowards(transform.position, target, speed);
//            }
//        }
        
//        transform.position = camera.transform.position + offset;
        coroutine = MovementDelay(WaitTime);


        print("moving = " + CheckMoving());
        if (CameraController.shut == 0 && transform.position.x != camera.transform.position.x + offset.x)
        {
            if (CheckMoving() == false)
            {
                wornHelmet = playerHelmet.sprite;
                wornCloth = playerCloth.sprite;
                wornShoe = playerShoe.sprite;

                
                StartCoroutine(coroutine);
                PlayerFollowCamera();
//                playerBody.sprite = playerWalkingBody;
//                playerCloth.sprite = null;
//                playerHelmet.sprite = null;
//                playerShoe.sprite = null;



            }
            else //if moving
            {
                if(myMovingState == MovingState.Right)
                {
                    playerBody.flipX = true;
                    
                }
                else if (myMovingState == MovingState.Left)
                {
                    playerBody.flipX = true;
                    

                }
                
               

                PlayerFollowCamera();
//                playerBody.sprite = playerIdleBody;
//                playerCloth.sprite = playerIdleCloth;
//                playerHelmet.sprite = playerIdleHelmet;
//                playerShoe.sprite = playerIdleShoe;

                

            }

        }
        else
        {
//            playerBody.sprite = playerWalkingBody;
//            playerCloth.sprite = wornCloth;
//            playerHelmet.sprite = wornHelmet;
//            playerShoe.sprite = wornShoe;
        }
    }

    
    //delay the player movement
    IEnumerator MovementDelay(float Count){
        yield return new WaitForSeconds(Count); //Count is the amount of time in seconds that you want to wait.
        
        //And here goes your function

        yield return null;
    }

    void PlayerFollowCamera()
    {
        Vector3 target = new Vector3(camera.transform.position.x + offset.x, camera.transform.position.y + offset.y,transform.position.z);
        if(CameraController.shut == 0)
        {
            if (transform.position.x != camera.transform.position.x + offset.x)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed);
            }
        }
    }

    private Vector3 curPos;
    private Vector3 lastPos;
    bool CheckMoving()
    {
        
        curPos = transform.position;

        if(curPos == lastPos) {
            isMoving = false;
            myMovingState = MovingState.Idle;

        }
        else if(curPos.x < lastPos.x)
        {
            isMoving = true;
            myMovingState = MovingState.Left;
        }
        else if(curPos.x > lastPos.x)
        {
            isMoving = true;
            myMovingState = MovingState.Right;
        }
        lastPos = curPos;
        
        return isMoving;
    }
}

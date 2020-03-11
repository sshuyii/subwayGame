using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAnimation : MonoBehaviour
{

    private Animator myAnimator;

    private float offset;
    private Animation myAnimation;

    private SubwayMovement SubwayMovement;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myAnimation = GetComponent<Animation>();


        float randomIdleStart;
        randomIdleStart = Random.Range(0,myAnimator.GetCurrentAnimatorStateInfo(0).length); //Set a random part of the animation to start from
        myAnimator.Play("ClickHandleAnimation", 0, randomIdleStart);
        
        SubwayMovement = GameObject.Find("---StationController").GetComponent<SubwayMovement>();


    }

    // Update is called once per frame
    void Update()
    {
        if (SubwayMovement.isMoving)
        {
            myAnimator.SetBool("inStation", false);
        }
        else
        {
            myAnimator.SetBool("inStation", true);

        }
    }

    public void clickHandle()
    {
        myAnimator.SetTrigger("clicked");
    }
}

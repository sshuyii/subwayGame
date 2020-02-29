using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class StartTutorial : MonoBehaviour
{

    public float speedA;

    public float startingPoint;
    public float endPoint;

    public CanvasGroup MachineGroup;
    public bool isComic0;
    public bool isComic1;
    private bool isTutorial;


    public GameObject comic1;
    public GameObject comic2;
   
    

    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < endPoint)
        {
            GetComponent<RectTransform>().anchoredPosition += new Vector2(speedA, 0);
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(startingPoint, -4);
        }

        if (isComic0)
        {
            //(comic1, 0, speed, isComic1, true);
            if(comic1.GetComponent<RectTransform>().anchoredPosition.x < 0)
            {
                comic1.GetComponent<RectTransform>().anchoredPosition += new Vector2(speed, 0);
            }
            else
            {
                isComic0 = false;
            }
        }
        else if (isComic1)
        {
            if(comic2.GetComponent<RectTransform>().anchoredPosition.x > 0)
            {
                comic2.GetComponent<RectTransform>().anchoredPosition -= new Vector2(speed, 0);
            }
            else
            {
                isComic1 = false;
                isTutorial = true;
            }
        }
    }
    
    public void TutorialStartPre()
    {
        Hide(MachineGroup);
        //Comic starts
        isComic0 = true;
        //SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);

    }

    private void ComicFlyIn(GameObject comic, float stopPoint, float speed, bool returnTrue, bool fromLeft)
    {
        if (fromLeft)
        {
            if(comic.GetComponent<RectTransform>().anchoredPosition.x < stopPoint)
            {
                comic.GetComponent<RectTransform>().anchoredPosition += new Vector2(speed, 0);
            }
            else
            {
                returnTrue = true;
                isComic0 = false;
                print("trrrrrrrr");
            }
        }
        else
        {
            if(comic.GetComponent<RectTransform>().anchoredPosition.x > stopPoint)
            {
                comic.GetComponent<RectTransform>().anchoredPosition += new Vector2(speed, 0);
            }
            else
            {
                returnTrue = true;
            }
        }
    }
    
    void Hide(CanvasGroup UIGroup) {
        UIGroup.alpha = 0f; //this makes everything transparent
        UIGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
        UIGroup.interactable = false;

    }
    
    void Show(CanvasGroup UIGroup) {
        UIGroup.alpha = 1f; //this makes everything transparent
        UIGroup.blocksRaycasts = true; //this prevents the UI element to receive input events
        UIGroup.interactable = true;

    }

    private int clickTime = 0;
    public void ComicClick()
    {
        if(isTutorial)
        {
            SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);

        }
        if(isComic0 == false)
        {
            isComic1 = true;
//            if (clickTime == 1)
//            {
//                comic1.enabled = true;
//            }
//            else if (clickTime == 2)
//            {
//                comic2.enabled = true;
//            }
//            else if (clickTime == 3)
//            {
//                comic3.enabled = true;
//            }
//            else if (clickTime == 4)
//            {
//                comic4.enabled = true;
//            }
//            else if (clickTime == 5)
//            {
//                SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);
//            }
        }

    }
}

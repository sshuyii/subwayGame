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
    
    public bool isComic;
    public bool isComic0;
    public bool isComic1;
    public bool isComic2;
    public bool isComic3;

    private bool isTutorial;

    public GameObject comicBackground;
    public GameObject comic1;
    public GameObject comic2;
    public GameObject comic3;
    public GameObject comic4;
    public GameObject comic5;


    private bool flash;
    public CanvasGroup myFlash;
    

    public float speed = 1f;

    private bool startTutorial;
    // Start is called before the first frame update
    void Start()
    {
        comicBackground.SetActive(false);
        comic1.SetActive(false);
        comic2.SetActive(false);
        comic3.SetActive(false);
        comic4.SetActive(false);
        comic5.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        //for flash
        if (flash)
        {
                myFlash.alpha = myFlash.alpha - Time.deltaTime;

                if (myFlash.alpha <= 0)
                {
                    myFlash.alpha = 0;
                    flash = false;

                    if (startTutorial)
                    {
                        SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);
                        startTutorial = false;
                    }
                    else isComic = true;
                }
                
        }

        //for background moving
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
                isComic2 = true;
            }
        }
        else if(isComic3)
        {
            comic3.GetComponent<Image>().enabled = true;
            isTutorial = true;
        }
    }
    
    public void TutorialStartPre()
    {
        Hide(MachineGroup);
        //Comic starts
        flash = true;
        myFlash.alpha = 1;
        comicBackground.SetActive(true);

        //SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);
        //comic starts to fly in

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
            flash = true;
            
        }
      
//        if(isComic0 == false)
//        {
//            flash = true;
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
//        }
        else if(isComic)
        {
            clickTime++;
            if (clickTime == 1)
            {
                comic1.SetActive(true);
            }
            else if (clickTime == 2)
            {
                comic2.SetActive(true);
            }
            else if (clickTime == 3)
            {
                comic3.SetActive(true);
            }
            else if (clickTime == 4)
            {
                comic4.SetActive(true);
            }
            else if (clickTime == 5)
            {
                comic1.SetActive(false);
                comic2.SetActive(false);
                comic3.SetActive(false);
                comic4.SetActive(false);

                comic5.SetActive(true);
            }
            else if(clickTime == 6)
            {
                SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);
                myFlash.alpha = 1;
                startTutorial = true;

            }
        }

    }
}

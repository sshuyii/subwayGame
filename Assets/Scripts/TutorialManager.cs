using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;


public class TutorialManager : MonoBehaviour
{
    public Image KararaImage;
    public GameObject DialogueBubble;
    
    
    private HorizontalScrollSnap myHSS;
    public Image[] DialogueImageList;
    public TextMeshProUGUI myText;
    
    // Start is called before the first frame update
    void Start()
    {
        myHSS = GameObject.Find("Horizontal Scroll Snap").GetComponent<HorizontalScrollSnap>();

        StartCoroutine("TrainMoveIn");

       
        
        DialogueImageList = DialogueBubble.GetComponentsInChildren<Image>();
        myText = DialogueBubble.GetComponentInChildren<TextMeshProUGUI>();

        
        //disable the dialogues
        for (int a = 0; a < DialogueImageList.Length; a++)
        {
            DialogueImageList[a].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    IEnumerator TrainMoveIn() 
    {
        yield return new WaitForSeconds(5);

        
        for(int i = 0; i < 4; i ++)
        {
            yield return new WaitForSeconds(0);
            myHSS.GoToScreen(i);

            if (i == 3)
            {
                //karara appears
                yield return new WaitForSeconds(1);

                KararaImage.enabled = true;
                
                yield return new WaitForSeconds(1);

                for (int a = 0; a < DialogueImageList.Length; a++)
                {
                    DialogueImageList[a].enabled = true;
                    myText.text = "I should take a selfie in front of that poster!";

                }

            }
        }
        
       
    }

}

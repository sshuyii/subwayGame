using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstagramButtons : MonoBehaviour
{
    private Sprite currentSprite;
    public Image gotoImage;

    private Text username;
    
    // Start is called before the first frame update
    void Start()
    {
        currentSprite = GetComponent<Image>().sprite;
        username = GetComponentInChildren<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToPersonalPage()
    {
        if (username.text == "Karara")
        {
            Camera.main.transform.position =
                new Vector3(45, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }    
        else if (username.text == "Nico")
        {
            Camera.main.transform.position =
                new Vector3(55, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }       }
    
    public void ClickInsPost()
    {
        Camera.main.transform.position = new Vector3(65, Camera.main.transform.position.y, Camera.main.transform.position.z);

        gotoImage.sprite = currentSprite;

    }
}

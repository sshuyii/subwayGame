using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfieName : MonoBehaviour
{

    
    private InstagramController InstagramController;

    private Text username;

    private Sprite currentSprite;
    // Start is called before the first frame update
    void Start()
    {
        
        InstagramController = GameObject.Find("---InstagramController").GetComponent<InstagramController>();

        currentSprite = GetComponent<Image>().sprite;
        username = GetComponentInChildren<Text>();

        currentSprite = InstagramController.allProfile[username.text];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

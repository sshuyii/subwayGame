using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectScreenRatio : MonoBehaviour
{
    public Transform PlayerFolder;
    // Start is called before the first frame update
    void Start()
    {
        if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX)
        {
            PlayerFolder.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            
        }
        else if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone7)
        {
            PlayerFolder.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

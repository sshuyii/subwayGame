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
    public float A;
    
    // Start is called before the first frame update
    void Start()
    {
        CameraController = camera.GetComponent<CameraController>();
        offset = transform.position - camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = new Vector3(camera.transform.position.x + offset.x, camera.transform.position.y + offset.y,transform.position.z);
        if(CameraController.shut == 0)
        {
            if (transform.position.x != camera.transform.position.x + offset.x)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed);
            }
        }
        
//        transform.position = camera.transform.position + offset;
    }
}

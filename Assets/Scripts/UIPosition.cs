using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPosition : MonoBehaviour
{
    public GameObject camera;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = camera.transform.position - transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = camera.transform.position - offset;
    }
}

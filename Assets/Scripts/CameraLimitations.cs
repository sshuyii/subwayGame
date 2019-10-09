using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLimitations : MonoBehaviour
{
    public float leftL;
    public float rightL;

    private Lean.Touch.LeanDragCamera LeanDragCamera;
    // Start is called before the first frame update
    void Start()
    {
        LeanDragCamera = this.gameObject.GetComponent<Lean.Touch.LeanDragCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.x < leftL || this.gameObject.transform.position.x > rightL)
        {
            LeanDragCamera.Sensitivity = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecalculateMesh : MonoBehaviour
{
    
    private MeshFilter[] meshFilter;

    private List<Mesh> mesh;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponentsInChildren<MeshFilter>();
        
        for(int i = 0; i < meshFilter.Length; i++)
        {
            mesh.Add(meshFilter[i].mesh); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        mesh.Clear();

    }
}

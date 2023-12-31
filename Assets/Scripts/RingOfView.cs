using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOfView : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[3];
        Vector2[] uv = new Vector2[3];
        int[] triangles = new int[3];

        vertices[0] = Vector3.zero;
        vertices[1] = new Vector3(25, 25);
        vertices[2] = new Vector3(0, -25);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    void Update()
    {
        
    }
}

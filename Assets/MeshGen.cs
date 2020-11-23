using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeshGen : MonoBehaviour
{
    //map info 
    public int width;
    public int height;
    private float[,] map;


    //noise info
    public float noiseScaler;
    private object Square;

    public Mesh mesh;
    public Vector3[] vertices;
    public int[] triangles;
    public List<Vector3> vertexList = new List<Vector3>();


    // Square square = new Square(Vector3.one, 1);
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateVertexGrid();
    }

    void CreateVertexGrid()
    {
        vertices = new Vector3[(width + 1) * (height + 1)];

        for (int z = 0, index = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++, index++)
            {
                var noise = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                vertices[index] = new Vector3(x, noise, z);
            }
        }

        mesh.vertices = vertices;

        // triangles = new int[6];
        // triangles[0] = 0;
        // triangles[1] = width + 1;
        // triangles[2] = 1;
        //
        // triangles[3] = 1;
        // triangles[4] = width + 1;
        // triangles[5] = width + 2;
        // mesh.triangles = triangles;

        CreateTriangles();
        mesh.RecalculateNormals();
    }


    void CreateTriangles()
    {
        triangles = new int[width *  height * 6];

        // ---------

        //--------
        for (int z = 0, triIndex = 0, vertIndex = 0; z < height; z++, vertIndex++)
        {
            for (int x = 0; x < width; x++, triIndex += 6, vertIndex++)
            {
                triangles[triIndex] = vertIndex;
                triangles[triIndex + 1] = vertIndex + width + 1;
                triangles[triIndex + 2] = vertIndex + 1;

                triangles[triIndex + 3] = vertIndex + 1;
                triangles[triIndex + 4] = vertIndex + width + 1;
                triangles[triIndex + 5] = vertIndex + width + 2;
            }
        }


        mesh.triangles = triangles;
        Debug.Log("Created Triangles");
    }

    private void OnDrawGizmos()
    {
        // var col = width;
        // var row = height;
        // for (int x = 0; x < col; x++)
        // {
        //     for (int y = 0; y < row; y++)
        //     {
        //         // // Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + +.5f);
        //         Vector3 pos = new Vector3(x, 0, y);
        //         // Vector3 size = new Vector3(scale, 0, 1);
        //         var yRand = Mathf.PerlinNoise(x * .3f, y * .3f) * 2f;
        //     }
        // }


        // for (int z = 0, i = 0; z < height; z++)
        // {
        //     for (int x = 0; x < width; x++, i++)
        //     {
        //         Gizmos.color = Color.black;
        //         Gizmos.DrawSphere(vertices[i], 0.05f);
        //     }
        // }
    }
}
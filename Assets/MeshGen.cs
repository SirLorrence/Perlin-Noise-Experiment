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
    public PerlinGen pn;

    public MeshRenderer testColor;
    public Texture2D HeightMapTexture2D;

    public int mapRes;


    // Square square = new Square(Vector3.one, 1);
    private void Start()
    {
        pn.gridSizeY = height;
        pn.gridSizeX = width;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        testColor = GetComponent<MeshRenderer>();
        pn.Generate();
        CreateVertexGrid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mesh.Clear();
            pn.Generate();
            CreateVertexGrid();
            print("new grid");
        }
    }


    void CreateVertexGrid()
    {
        vertices = new Vector3[(width + 1) * (height + 1)];
        
        HeightMapTexture2D = new Texture2D(pn.perlinTextureX,pn.perlinTextureY);

        for (int z = 0, index = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++, index++)
            {
                var noise = pn.GetSampleFromNoise(x, z);
                vertices[index] = new Vector3(x, noise * pn.heightScale, z);
                // HeightMapTexture2D.SetPixel(x,z, pn.grad.Evaluate(pn.GetSampleFromNoise(x, z)));
            }
        }
        HeightMapTexture2D.Apply();

        mesh.vertices = vertices;

        testColor.material.mainTexture = HeightMapTexture2D;
        CreateTriangles();
        mesh.RecalculateNormals();
    }


    void CreateTriangles()
    {
        triangles = new int[width * height * 6];

        // ---------

        //--------
        for (int z = 0, triIndex = 0, vertIndex = 0; z < height; z++, vertIndex++)
        {
            for (int x = 0; x < width; x++, triIndex += 6, vertIndex++)
            {
                triangles[triIndex] = vertIndex;
                triangles[triIndex + 1] = vertIndex  + width + 1;
                triangles[triIndex + 2] = vertIndex + 1;

                triangles[triIndex + 3] = vertIndex + 1;
                triangles[triIndex + 4] = vertIndex + width + 1;
                triangles[triIndex + 5] = vertIndex + width + 2;
            }
        }


        mesh.triangles = triangles;
        Debug.Log("Created Triangles");
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

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
    private Vector3[] vertices;
    public int[] triangles;
    private List<Vector3> vertexList = new List<Vector3>();


    // Square square = new Square(Vector3.one, 1);
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateSquareGrid();
    }

    private void Update()
    {
        // CreateSquareGrid();
        // Square square = new Square(Vector3.zero, scale / 2);
        // CreateShape();

        // Vector3 newPos = new Vector3(square.pos.x + (2 * scsale), 0, square.pos.z);
        // Square square2 = new Square(newPos, scale / 2);

        // vertexList.AddRange(square.VertexPoints);
        // vertexList.AddRange(square2.VertexPoints);


        // UpdateMesh(square);
        // UpdateMesh(square2);
        // CreateShape();


        // MapGen();
        // var num = Mathf.PerlinNoise(width * Time.time, height * Time.time);
        // print(num);
    }

    private void CreateShape()
    {
        triangles = new[]
        {
            0, 1, 2,
            1, 3, 2,
        };
    }

    void UpdateMesh(Square square)
    {
        // mesh.Clear();
        mesh.vertices = square.VertexPoints;
        // mesh.triangles = triangles;
    }


    // void TestNoise()
    // {
    //     for (int x = 0; x < width; x++)
    //     {
    //         for (int y = 0; y < height; y++)
    //         {
    //             var ix = x + width * noiseScaler;
    //             var iy = y + height * noiseScaler;
    //             var num = Mathf.PerlinNoise(ix, iy) / 6f;
    //             print(num);
    //         }
    //     }
    // }


    void CreateSquareGrid()
    {
        var col = width ;
        var row = height ;
        // triangles = new int[12];
        for (int x = 0; x < col; x++)
        {
            for (int y = 0; y < row; y++)
            {
                var pos = new Vector3(x, 0, y);
                Square square = new Square(pos );
                vertexList.AddRange(square.VertexPoints);
                // triangles[0] =  triangles[3] = 0;
                // triangles[1] = 1;
                // triangles[2] = triangles[4]=  x + 2;
                // triangles[5] = x + 1;
                // //

                // UpdateMesh(square);
            }
        }

        mesh.vertices = vertexList.ToArray();

        StartCoroutine(CreateTriangles());

        // works
        // triangles[0] = 0;
        // triangles[1] = triangles[3] =1;
        // triangles[2] =triangles[5] = 2;
        // triangles[4] = 3; 

        // triangles[0] = 0;
        // triangles[1] = 1;
        // triangles[2] = 2;
        //
        // triangles[3] = 1;
        // triangles[4] = 3;
        // triangles[5] = 2;


        // triangles[3] = 8 + 4;
        // triangles[4] = 9 + 4;
        // triangles[5] = 10 + 4;
        //
        //
        // triangles[6] = 12 ;
        // triangles[7] = 13 ;
        // triangles[8] = 14 ;
        // triangles[6] = 4 ;
        // triangles[7] = 5 ;
        // triangles[8] = 6 ;  
        //
        // triangles[9] = 5;
        // triangles[10] = 7;
        // triangles[11] = 6;


        // triangles[0] = triangles[3] = 0;
        // triangles[1] = 1;
        // triangles[2] = triangles[4]=  2;
        // triangles[5] = 1;
        // mesh.triangles = triangles;
    }

    IEnumerator CreateTriangles()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.05f);
        var col = width ;
        var row = height ;
        triangles = new int[width * height * 6]; 

        // ---------
        //vertex index needs to increase by 4, there's four vertices in the square
        //--------
        for (int y = 0 ,triIndex = 0, vertIndex = 0; y < row; y++)
        {
            for (int x = 0 ; x < col; x++, triIndex += 6, vertIndex += 4)
            {
                // triangles[triIndex] = vertIndex;
                // triangles[triIndex + 1] = triangles[triIndex + 3] = vertIndex + 1;
                // triangles[triIndex + 2] = triangles[triIndex + 5] = vertIndex + 2;
                // triangles[triIndex + 4] = vertIndex + 3;

                triangles[triIndex] = vertIndex;
                triangles[triIndex + 1] = vertIndex + 1;
                triangles[triIndex + 2] = vertIndex + 2;

                mesh.triangles = triangles;
                yield return waitForSeconds;

                triangles[triIndex + 3] = vertIndex + 1;
                triangles[triIndex + 4] = vertIndex + 3;
                triangles[triIndex + 5] = vertIndex + 2;
                // triangles[triIndex] = triangles[triIndex + 3] = vertIndex;
                // triangles[triIndex + 1] = vertIndex + 1;
                // triangles[triIndex + 2] = triangles[triIndex + 4] = vertIndex + 2;
                // triangles[triIndex + 5] = vertIndex + 1;


                mesh.triangles = triangles;
                yield return waitForSeconds;
            }

            print(triangles.Length);

            Debug.Log("next row");
        }


        Debug.Log("Created Triangles");
    }


    //
    // void RandomFill()
    // {
    //     for (int x = 0; x < width; x++)
    //     {
    //         for (int y = 0; y < height; y++)
    //         {
    //             //boarder
    //             if (x == 0 || x == width - 1 || y == 0 || y == height - 1) map[x, y] = 1;
    //
    //             map[x, y] = Mathf.PerlinNoise(width * noiseScaler, height * noiseScaler);
    //         }
    //     }
    // }


    private void OnDrawGizmos()
    {
        var col = width ;
        var row = height ;
        for (int x = 0; x < col; x++)
        {
            for (int y = 0; y < row; y++)
            {
                // // Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + +.5f);
                Vector3 pos = new Vector3(x, 0, y);
                // Vector3 size = new Vector3(scale, 0, 1);
                //
                Square square = new Square(pos); // cuts is in have to prevent over lapping 
                Gizmos.color = Color.black; // top left
                Gizmos.DrawSphere(square.VertexPoints[0], .05f);
                // Gizmos.color = Color.blue; // top right
                Gizmos.DrawSphere(square.VertexPoints[1], .05f);
                // Gizmos.color = Color.green; // bottom left 
                Gizmos.DrawSphere(square.VertexPoints[2], .05f);
                // Gizmos.color = Color.red; // bottom right
                Gizmos.DrawSphere(square.VertexPoints[3], .05f);

                // Gizmos.color = Color.cyan;
                // Gizmos.DrawLine(square.VertexPoints[2], square.VertexPoints[1]);
                //
                // Gizmos.color = Color.black;
                // Gizmos.DrawLine(square.VertexPoints[0], square.VertexPoints[2]); // left side
                // Gizmos.DrawLine(square.VertexPoints[0], square.VertexPoints[1]); // top side
                // Gizmos.DrawLine(square.VertexPoints[1], square.VertexPoints[3]); // right side
                // Gizmos.DrawLine(square.VertexPoints[2], square.VertexPoints[3]); // bottom side

                // Gizmos.DrawWireCube(pos, size);
                // Vector3 pos2 = new Vector3(x * scale, 0, y * scale);
                // Gizmos.DrawSphere(pos2, .2f);
            }
        }

        // Square square = new Square(Vector3.one, scale);
        // Gizmos.color = Color.black; // top left
        // Gizmos.DrawSphere(square.Veritces[0], .1f);
        // Gizmos.color = Color.blue; // top right
        // Gizmos.DrawSphere(square.Veritces[1], .1f);
        // Gizmos.color = Color.green; // bottom left 
        // Gizmos.DrawSphere(square.Veritces[2], .1f);
        // Gizmos.color = Color.red; // bottom right
        // Gizmos.DrawSphere(square.Veritces[3], .1f);
        //
        //
        // Vector3 newPos = new Vector3(square.pos.x + (2 * scale ), 0,  square.pos.z);
        // Square square2 = new Square(newPos, scale);
        // Gizmos.color = Color.black; // top left
        // Gizmos.DrawSphere(square2.Veritces[0], .1f);
        // Gizmos.color = Color.blue; // top right
        // Gizmos.DrawSphere(square2.Veritces[1], .1f);
        // Gizmos.color = Color.green; // bottom left 
        // Gizmos.DrawSphere(square2.Veritces[2], .1f);
        // Gizmos.color = Color.red; // bottom right
        // Gizmos.DrawSphere(square2.Veritces[3], .1f);
        //
        // Gizmos.color = Color.cyan;
        // Gizmos.DrawLine(square.Veritces[2], square.Veritces[1]);
        // Gizmos.DrawLine(square2.Veritces[2], square2.Veritces[1]);
        //
        // Gizmos.color = Color.black;
        // Gizmos.DrawLine(square.Veritces[0], square.Veritces[2]); // left side
        // Gizmos.DrawLine(square.Veritces[0], square.Veritces[1]); // top side
        // Gizmos.DrawLine(square.Veritces[1], square.Veritces[3]); // right side
        // Gizmos.DrawLine(square.Veritces[2], square.Veritces[3]); // bottom side
    }
}
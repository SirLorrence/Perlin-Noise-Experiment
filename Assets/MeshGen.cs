using System.Collections.Generic;
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
    public Vector3[] vertices;
    public int[] triangles;

    public List<Vector3> vertexList = new List<Vector3>();
    // public PerlinGen pn;

    // public MeshRenderer testColor;
    // public Texture2D HeightMapTexture2D;

    public int mapResolution = 1;


    [Range(1, 10)] public int octaves = 1;


    public float frequency;

    public Gradient color;
    
    
    

    // Square square = new Square(Vector3.one, 1);
    private void Start()
    {
        // pn.gridSizeY = height;
        // pn.gridSizeX = width;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        // testColor = GetComponent<MeshRenderer>();
        // pn.Generate();
        CreateVertexResGrid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mesh.Clear();
            // pn.Generate();
            CreateVertexResGrid();
            print("new grid");
        }
    }


    #region Scale Grid

    // void CreateVertexGrid()
    //    {
    //        vertices = new Vector3[(width + 1) * (height + 1)];
    //        Vector2[] uv = new Vector2[vertices.Length];
    //        Color[] colors = new Color[vertices.Length];
    //        // HeightMapTexture2D = new Texture2D(pn.perlinTextureX,pn.perlinTextureY);
    //
    //        for (int z = 0, index = 0; z <= height; z++)
    //        {
    //            for (int x = 0; x <= width; x++, index++)
    //            {
    //                // var noise = pn.GetSampleFromNoise(x, z);
    //                // vertices[index] = new Vector3(x, noise * pn.heightScale, z);
    //                vertices[index] = new Vector3(x, 0, z);
    //                // uv[index] = new Vector2(x,z);
    //                // colors[index] = pn.grad.Evaluate(noise);
    //                // HeightMapTexture2D.SetPixel(x,z, pn.grad.Evaluate(pn.GetSampleFromNoise(x, z)));
    //            }
    //        }
    //
    //        // HeightMapTexture2D.Apply();
    //
    //        mesh.vertices = vertices;
    //        mesh.uv = uv;
    //        // mesh.colors = colors;
    //
    //        // testColor.material.mainTexture = HeightMapTexture2D;
    //        CreateTriangles();
    //        mesh.RecalculateNormals();
    //    }
    //
    //    void CreateTriangles()
    //    {
    //        triangles = new int[width * height * 6];
    //
    //        // ---------
    //
    //        //--------
    //        for (int z = 0, triIndex = 0, vertIndex = 0; z < height; z++, vertIndex++)
    //        {
    //            for (int x = 0; x < width; x++, triIndex += 6, vertIndex++)
    //            {
    //                triangles[triIndex] = vertIndex;
    //                triangles[triIndex + 1] = vertIndex + width + 1;
    //                triangles[triIndex + 2] = vertIndex + 1;
    //
    //                triangles[triIndex + 3] = vertIndex + 1;
    //                triangles[triIndex + 4] = vertIndex + width + 1;
    //                triangles[triIndex + 5] = vertIndex + width + 2;
    //            }
    //        }
    //
    //
    //        mesh.triangles = triangles;
    //        Debug.Log("Created Triangles");
    //    }

    #endregion


    #region resolution Scaling

    void CreateVertexResGrid()
    {
        List<Vector3> ver = new List<Vector3>();
        List<Color> heightColor = new List<Color>();
        
        
        float stepSize = 1f / mapResolution;

        Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
        Vector3 point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
        Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        Vector3 point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));

        // makes 1x1
        for (int z = 0; z <= mapResolution; z++)
        {
            var point0 = Vector3.Lerp(point00, point01, (z + .5f) * stepSize);
            var point1 = Vector3.Lerp(point10, point11, (z + .5f) * stepSize);
            for (int x = 0; x <= mapResolution; x++)
            {
                var point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                float sample = Noise.Layer2D(point, frequency, octaves);

                // var noise = pn.GetSampleFromNoise(x, z);
                sample = sample * .5f + .5f;
                ver.Add(new Vector3((x * stepSize - .5f), sample, (z * stepSize - .5f)));
                heightColor.Add(color.Evaluate(sample));
                // ver.Add(new Vector3((x * stepSize - .5f), noise * pn.heightScale, (z * stepSize - .5f)));
                // vertices[index] = new Vector3(xIndex +(x * stepSize - .5f), 0, zIndex+ (z * stepSize - .5f));
                // HeightMapTexture2D.SetPixel(x,z, pn.grad.Evaluate(pn.GetSampleFromNoise(x, z)));
            }
        }

        mesh.vertices = ver.ToArray();
        CreateResTriangles();
        mesh.colors = heightColor.ToArray();
        mesh.RecalculateNormals();
    }


    void CreateResTriangles()
    {
        triangles = new int[Mathf.FloorToInt(Mathf.Pow(mapResolution, 2) * 6)];

        // ---------

        //--------
        for (int z = 0, triIndex = 0, vertIndex = 0; z < mapResolution; z++, vertIndex++)
        {
            for (int x = 0; x < mapResolution; x++, triIndex += 6, vertIndex++)
            {
                triangles[triIndex] = vertIndex;
                triangles[triIndex + 1] = vertIndex + mapResolution + 1;
                triangles[triIndex + 2] = vertIndex + 1;

                triangles[triIndex + 3] = vertIndex + 1;
                triangles[triIndex + 4] = vertIndex + mapResolution + 1;
                triangles[triIndex + 5] = vertIndex + mapResolution + 2;
            }
        }


        mesh.triangles = triangles;
        Debug.Log("Created Triangles");
    }

    #endregion
}
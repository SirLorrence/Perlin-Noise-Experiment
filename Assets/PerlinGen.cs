using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PerlinGen : MonoBehaviour
{
    [Header("Perlin Map Set UP")] public int perlinTextureX;
    public int perlinTextureY;
    public int gridSizeX;
    public int gridSizeY;
    [Header("Test Cases")] public bool testWithPrefab = false;
    public bool testWithMesh = false;

    [Header("Map Settings & Location")] public Vector2 perlinOffset;
    private Texture2D perlinTexture2D;
    public float noiseScale = 1f, heightScale;
    public MeshRenderer testImage;
    public GameObject GamePrefab;
    public Gradient grad;

    public float freq;
    private Vector3[] vertices;
    private Mesh mesh;
    public GameObject childMesh;

    private void Awake()
    {
        mesh = new Mesh();
        childMesh.GetComponent<MeshFilter>().mesh = mesh;

        testImage = GetComponent<MeshRenderer>();
        perlinOffset = new Vector2(Random.Range(0, 99999), Random.Range(0, 99999));
    }

    private void Start()
    {
        Generate();
        if (testWithPrefab) TerrianGen();
        if (testWithMesh) CreateVertexGrid();
    }

    private void Update()
    {
        Generate();
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);
    }

    #region Creating Perlin Data

    public void Generate()
    {
        perlinTexture2D = new Texture2D(perlinTextureX, perlinTextureY);

        for (int x = 0; x < perlinTextureX; x++)
        {
            for (int y = 0; y < perlinTextureY; y++)
            {
                perlinTexture2D.SetPixel(x, y, Sample(x, y)); // write a pixel into an texture
                // perlinTexture2D.SetPixel(x, y, Color.white * Sample(x, y)); // write a pixel into an texture
            }
        }

        perlinTexture2D.Apply();
        testImage.material.mainTexture = perlinTexture2D;
    }

    public Color Sample(int x, int y)
    {
        var xCoord = (float) x / perlinTextureX * noiseScale + perlinOffset.x;
        var yCoord = (float) y / perlinTextureY * noiseScale + perlinOffset.y;

        var sample = Mathf.PerlinNoise(xCoord, yCoord);

        Color color = new Color(sample, sample, sample);
        return color;
    }

    #endregion

    #region Generate Terrian

    void TerrianGen()
    {
        for (int z = 0; z < gridSizeY; z++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 pos = new Vector3(x, GetSampleFromNoise(x, z) * heightScale, z);
                var cube = Instantiate(GamePrefab, pos, Quaternion.identity);
                var objectMat = cube.GetComponent<MeshRenderer>();
                var color = grad.Evaluate(GetSampleFromNoise(x, z));
                objectMat.material.color = color;
            }
        }
    }

    public float GetSampleFromNoise(int x, int y)
    {
        var gridStepX = perlinTextureX / gridSizeX;
        var gridStepY = perlinTextureY / gridSizeY;

        float sampledNoiseFloat =
            perlinTexture2D.GetPixel(Mathf.FloorToInt(x * gridStepX), Mathf.FloorToInt(y * gridStepY))
                .grayscale; // getting the pixel out of the texture, then get the grayscale value (0-1)

        return sampledNoiseFloat;
    }


    void CreateVertexGrid()
    {
        vertices = new Vector3[(gridSizeX + 1) * (gridSizeY + 1)];
        Color[] colors = new Color[vertices.Length];

        for (int z = 0, index = 0; z <= gridSizeY; z++)
        {
            for (int x = 0; x <= gridSizeX; x++, index++)
            {
                var noise = GetSampleFromNoise(x, z);
                vertices[index] = new Vector3(x, noise * heightScale, z);
                colors[index] = grad.Evaluate(GetSampleFromNoise(x, z));
            }
        }


        mesh.vertices = vertices;
        mesh.colors = colors;
        CreateTriangles();
        mesh.RecalculateNormals();
    }

    void CreateTriangles()
    {
        var triangles = new int[gridSizeX * gridSizeY * 6];

        // ---------

        //--------
        for (int z = 0, triIndex = 0, vertIndex = 0; z < gridSizeY; z++, vertIndex++)
        {
            for (int x = 0; x < gridSizeX; x++, triIndex += 6, vertIndex++)
            {
                triangles[triIndex] = vertIndex;
                triangles[triIndex + 1] = vertIndex + gridSizeX + 1;
                triangles[triIndex + 2] = vertIndex + 1;

                triangles[triIndex + 3] = vertIndex + 1;
                triangles[triIndex + 4] = vertIndex + gridSizeX + 1;
                triangles[triIndex + 5] = vertIndex + gridSizeX + 2;
            }
        }


        mesh.triangles = triangles;
        Debug.Log("Created Triangles");
    }

    #endregion
}
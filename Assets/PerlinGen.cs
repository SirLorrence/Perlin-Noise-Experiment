using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PerlinGen : MonoBehaviour
{
    [Header("Perlin Map Set UP")] public int perlinTextureX;
    public int perlinTextureY;
    public int gridSizeX;
    public int gridSizeY;
    public bool testWithPrefab = false;

    [Header("Map Settings & Location")] public Vector2 perlinOffset;
    private Texture2D perlinTexture2D;
    public float noiseScale = 1f, heightScale;
    public MeshRenderer testImage;
    public GameObject GamePrefab;
    public Gradient grad;

    private void Awake()
    {
        testImage = GetComponent<MeshRenderer>();
        perlinOffset = new Vector2(Random.Range(0, 99999), Random.Range(0, 99999));
    }

    private void Start()
    {
        Generate();
        if (testWithPrefab) TerrianGen();
    }

    private void Update()
    {
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

    #endregion
}
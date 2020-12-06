using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PerlinGen : MonoBehaviour
{
    private Vector2 perlinOffset;
    private Texture2D perlinTexture2D;
    public float noiseScale = 1f;
    private MeshRenderer testImage;
    public int perlinTextureX, perlinTextureY;

    private void Awake()
    {
        testImage = GetComponent<MeshRenderer>();
        perlinOffset = new Vector2(Random.Range(0, 99999), Random.Range(0, 99999));
    }

    private void Update()
    {
        Generate();
        testImage.material.mainTexture = perlinTexture2D;
    }

    void Generate()
    {
       

        perlinTexture2D = new Texture2D(perlinTextureX, perlinTextureY);

       for (int x = 0; x < perlinTextureX; x++) 
        {
            for (int y = 0; y < perlinTextureY; y++)
            {
                perlinTexture2D.SetPixel(x, y, Sample(x,y));
            }
        }
        perlinTexture2D.Apply();
    }

    Color Sample(int x, int y)
    {
        var xCoord = (float) x / perlinTextureX  * noiseScale+ perlinOffset.x;
        var yCoord = (float) y / perlinTextureY  * noiseScale+ perlinOffset.y;

        var sample = Mathf.PerlinNoise(xCoord, yCoord);

        Color color = new Color(sample, sample, sample);
        return color;
    }
}
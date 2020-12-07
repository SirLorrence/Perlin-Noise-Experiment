
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class TextureCreator : MonoBehaviour
{
    [Range(2, 512)] //inspector slider
    public int resolution = 256; // size of the texture

    public float freq;
    private Texture2D texure;

    private void OnEnable()
    {
        //creates a new texture 
        if (texure == null)
        {
            texure = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
            texure.name = "Procedural Texture";
            texure.wrapMode = TextureWrapMode.Clamp; // wrap around
            texure.filterMode = FilterMode.Trilinear;
            texure.anisoLevel = 9; // anisotropic filtering
            GetComponent<MeshRenderer>().material.mainTexture = texure; // applies it
        }

        FillTexture();
    }

    private void Update()
    {
        if (transform.hasChanged)
        {
            transform.hasChanged = false;
            FillTexture();
        }
    }

    public void FillTexture()
    {
        if (texure.width != resolution) texure.Resize(resolution, resolution);

        Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
        Vector3 point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
        Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        Vector3 point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));

        float stepSize = 1f / resolution;


        for (int y = 0; y < resolution; y++)
        {
            var point0 = Vector3.Lerp(point00, point01, (y + .5f) * stepSize);
            var point1 = Vector3.Lerp(point10, point11, (y + .5f) * stepSize);
            for (int x = 0; x < resolution; x++)
            {
                var point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                float sample = Noise.Perlin2D(point, freq);
                sample = sample * .5f + .5f;
                texure.SetPixel(x, y, Color.white *sample);
            }
        }

        texure.Apply();
    }
}
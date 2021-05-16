using System;
using System.Collections.Generic;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{
    //number of triangles to the power of 2 
    [Range(0, 255)]
    [SerializeField] private int meshResolution = 255;

    //Amount of detail
    [Range(1, 10)]
    [SerializeField] private int octaves = 1;

    //Amount of detail add/removed from octaves
    [SerializeField] private float frequency;

    [SerializeField] private Gradient color;

    private int[] _triangles;
    private Mesh _mesh;
    private Vector3 _offset;
    private void Start(){
        _mesh = new Mesh();
        _mesh.name = "Perlin Terrain";
        GetComponent<MeshFilter>().mesh = _mesh;
        CreateVertexResGrid();
    }

    public void Rebuild(){
        _mesh.Clear();
        CreateVertexResGrid();

    }
    private void Update(){
        if(transform.hasChanged){
            transform.hasChanged = false;
            Rebuild();
            print("new grid");
        }
    }
    void CreateVertexResGrid(){
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var heightMapColor = new List<Color>();

        float stepSize;

        //vertex positions for a quad
        Vector3 bottomLeft; // 00
        Vector3 bottomRight; // 10
        Vector3 topLeft; // 01
        Vector3 topRight; // 11

        _offset.y = transform.position.z;
        stepSize = 1f / meshResolution ;
        bottomLeft = transform.TransformPoint(new Vector3(0f, 0f)) + _offset;
        bottomRight = transform.TransformPoint(new Vector3(1f, 0f)) + _offset;
        topLeft = transform.TransformPoint(new Vector3(0f, 1f)) + _offset;
        topRight = transform.TransformPoint(new Vector3(1f, 1f)) + _offset;

        for(int z = 0; z <= meshResolution ; z++){

            var leftCorners = Vector3.Lerp(bottomLeft, topLeft, z * stepSize); // gets the interpolated point of the left side
            var rightCorners = Vector3.Lerp(bottomRight, topRight, z * stepSize); // gets the interpolated point of the right

            for(int x = 0; x <= meshResolution; x++){

                var point = Vector3.Lerp(leftCorners, rightCorners, x * stepSize); //bil

                var noiseSamplePoint = Noise.Layer2D(point, frequency, octaves);

                noiseSamplePoint = noiseSamplePoint * 0.5f + 0.5f;

                vertices.Add(new Vector3(x * stepSize , noiseSamplePoint, z * stepSize));

                heightMapColor.Add(color.Evaluate(noiseSamplePoint));
            }
        }

        _mesh.vertices = vertices.ToArray();
        CreateResTriangles();
        _mesh.normals = normals.ToArray();
        _mesh.colors = heightMapColor.ToArray();
        _mesh.RecalculateNormals();
    }
    void CreateResTriangles(){

        _triangles = new int[Mathf.FloorToInt(Mathf.Pow(meshResolution, 2) * 6)];
        for(int z = 0, triIndex = 0, vertIndex = 0; z < meshResolution; z++, vertIndex++){
            for(int x = 0; x < meshResolution; x++, triIndex += 6, vertIndex++){
                _triangles[triIndex] = vertIndex;
                _triangles[triIndex + 1] = vertIndex + meshResolution + 1;
                _triangles[triIndex + 2] = vertIndex + 1;

                _triangles[triIndex + 3] = vertIndex + 1;
                _triangles[triIndex + 4] = vertIndex + meshResolution + 1;
                _triangles[triIndex + 5] = vertIndex + meshResolution + 2;
            }
        }

        _mesh.triangles = _triangles;
        Debug.Log("Created Triangles");
    }
}

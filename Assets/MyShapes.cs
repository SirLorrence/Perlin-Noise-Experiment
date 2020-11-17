using System.Collections.Generic;
using UnityEngine;

public struct Vertex
{
    public Vector3 pos;

    public Vertex(float x, float y, float z)
    {
        pos = new Vector3(x, y, z);
    }
}

// Square is 1 x 1 
public struct Square
{
    public Vertex TopLeft, TopRight, BottomLeft, BottomRight;
    public Vector3[] VertexPoints;
    public Vector3 pos;
    public int[] triangles;


    public Square(Vector3 position, float PerlinY, float scale = 0.5f)
    {
        pos = position;


        TopLeft = new Vertex(position.x - scale, PerlinY, position.z + scale);
        TopRight = new Vertex(position.x + scale, PerlinY, position.z + scale);
        BottomLeft = new Vertex(position.x - scale, PerlinY, position.z - scale);
        BottomRight = new Vertex(position.x + scale, PerlinY, position.z - scale);

        VertexPoints = new Vector3[]
        {
            TopLeft.pos,
            TopRight.pos,
            BottomLeft.pos,
            BottomRight.pos,
        };
        // Triangle tri = new Triangle(VertexPoints[0],VertexPoints[1],VertexPoints[2],VertexPoints[3]);

        triangles = new[]
        {
            0, 1, 2,
            1, 3, 2,
        };

        // VertextPoints[0] = (TopLeft.pos);
        // VertextPoints[1] = (TopRight.pos);
        // VertextPoints[2] = (BottomLeft.pos);
        // VertextPoints[3] = (BottomRight.pos);
    }
}

public struct Triangle
{
    public int vertexA, vertexB, vertexC;
    public int[] veritces;

    public Triangle(int p1, int p2, int p3, int p4)
    {
        vertexA = p1;
        vertexB = p2;
        vertexC = p3;
        veritces = new[]
        {
            p1, p2, p3
        };
    }
}
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Vertex
{
    public Vector2 worldPosition;

    public int id;
    public int[] neighbours;


    public Vertex(int id, Vector2 worldPosition)
    {
        this.id = id;
        this.worldPosition = worldPosition;
        neighbours = null;
    }

    public void Bind(in List<int> neighbours)
    {
        this.neighbours = neighbours.ToArray();
    }

}

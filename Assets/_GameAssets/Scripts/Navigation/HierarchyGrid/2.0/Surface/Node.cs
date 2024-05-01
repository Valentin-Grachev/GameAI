using UnityEngine;

public struct Node
{
    public ushort vertexId;
    public Vector2 position;

    public Node(int vertexId, Vector2 position)
    {
        this.vertexId = (ushort)vertexId;
        this.position = position;
    }
}

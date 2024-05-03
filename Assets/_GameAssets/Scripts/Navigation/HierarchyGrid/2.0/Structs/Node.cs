using UnityEngine;

public struct Node
{
    public ushort vertexId;
    public Vector2 position;
    public bool pathsBuilt;

    public void MarkAsBuilt() => pathsBuilt = true;

    public Node(int vertexId, Vector2 position)
    {
        this.vertexId = (ushort)vertexId;
        this.position = position;
        pathsBuilt = false;
    }
}



[System.Serializable]
public struct Edge
{
    public ushort toVertexId;
    public int weight;

    public Edge(ushort toVertexId, int weight)
    {
        this.toVertexId = toVertexId;
        this.weight = weight;
    }
}



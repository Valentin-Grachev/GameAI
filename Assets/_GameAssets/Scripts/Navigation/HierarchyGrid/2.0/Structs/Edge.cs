

[System.Serializable]
public struct Edge
{
    public ushort toVertexId;
    public int weight;
    public Path path;

    public Edge(ushort toVertexId, int weight)
    {
        this.toVertexId = toVertexId;
        this.weight = weight;
        path = new Path(betweenPoints: null);
    }

    public Edge(ushort toVertexId, int weight, Path path)
    {
        this.toVertexId = toVertexId;
        this.weight = weight;
        this.path = path;
    }

}



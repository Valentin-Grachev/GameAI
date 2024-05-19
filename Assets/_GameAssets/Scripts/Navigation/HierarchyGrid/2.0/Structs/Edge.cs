

[System.Serializable]
public struct Edge
{
    public ushort toVertexId;
    public ushort weight;
    public Path path;

    public Edge(ushort toVertexId, int weight)
    {
        this.toVertexId = toVertexId;
        this.weight = (ushort)weight;
        path = new Path(betweenPoints: null);
    }

    public Edge(ushort toVertexId, int weight, Path path)
    {
        this.toVertexId = toVertexId;
        this.weight = (ushort)weight;
        this.path = path;
    }

    



}



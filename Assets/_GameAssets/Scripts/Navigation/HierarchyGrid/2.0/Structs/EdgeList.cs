using System.Collections.Generic;

[System.Serializable]
public struct EdgeList
{
    public List<Edge> edges;

    public void Initialize() => edges = new List<Edge>();
    public EdgeList(EdgeList copy) => edges = new List<Edge>(copy.edges);

}

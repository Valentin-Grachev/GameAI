using System.Collections.Generic;

[System.Serializable]
public struct EdgeList
{
    public List<Edge> edges;
    public EdgeList(bool initialize) => edges = new List<Edge>();

}

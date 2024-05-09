using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = nameof(SurfaceData), fileName = nameof(SurfaceData))]
public class SurfaceData : ScriptableObject
{
    public Vector2 startPosition;
    public Vector2 size;
    public float surfaceDensity;
    public float localGridDensity;
    [Space(10)]
    public List<Vector2> nodePositions;
    public List<EdgeList> vertexEdges;





}

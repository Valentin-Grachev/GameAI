using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Path
{
    public List<Vector2> betweenPoints;

    public Path(List<Vector2> betweenPoints) => this.betweenPoints = betweenPoints;

    public Path GetReversed()
    {
        var reversedPoints = new List<Vector2>(betweenPoints);
        reversedPoints.Reverse();
        return new Path(reversedPoints);
    }

    public void DrawGizmos(Vector2 start, Vector2 finish)
    {
        Vector2 from = start;
        Vector2 to;
        for (int i = 0; i < betweenPoints.Count; i++)
        {
            to = betweenPoints[i];
            Gizmos.DrawLine(from, to);
            from = to;
        }
        to = finish;
        Gizmos.DrawLine(from, to);
    }
}

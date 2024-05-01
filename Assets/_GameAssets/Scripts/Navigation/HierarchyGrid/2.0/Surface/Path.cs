using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Path
{
    public List<Vector2> betweenPoints;

    public Path(List<Vector2> betweenPoints) => this.betweenPoints = betweenPoints;

}

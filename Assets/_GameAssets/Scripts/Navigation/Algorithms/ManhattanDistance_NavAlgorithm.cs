using UnityEngine;

namespace VG.GameAI.Navigation2D
{
    public class ManhattanDistance_NavAlgorithm : HeuristicNavAlgorithm
    {
        protected override float GetHeuristic(NavVertex current, NavVertex begin, NavVertex end)
        {
            current.GridPosition(out int x, out int y);
            end.GridPosition(out int endX, out int endY);

            return Mathf.Abs(x - endX) + Mathf.Abs(y - endY);
        }



    }

}








namespace VG.GameAI.Navigation2D
{
    public class Greedy_NavAlgorithm : HeuristicNavAlgorithm
    {
        protected override float GetHeuristic(NavVertex current, NavVertex begin, NavVertex end)
            => NavVertex.Distance(current, end);
    }
}


    


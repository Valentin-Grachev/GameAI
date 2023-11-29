
namespace VG.GameAI.Navigation2D
{
    public static class NavMemory
    {
        private static SortedList<HeuristicVertex> _sortedList;

        private const int capacity = 1000;


        public static void Add(HeuristicVertex value)
        {
            _sortedList ??= new SortedList<HeuristicVertex>(capacity);
            _sortedList.Add(value.heuristic, value);
        }

        public static HeuristicVertex GetMin() => _sortedList.GetMin();

        public static HeuristicVertex PopMin() => _sortedList.PopMin();

        public static void Clear() => _sortedList?.Clear();

        public static bool isEmpty => _sortedList == null || _sortedList.isEmpty;




    }
}
    

using System.Collections.Generic;

namespace VG.GameAI.Navigation2D
{
    public class BFS_NavAlgorithm : NavAlgorithm
    {

        public override List<NavVertex> FindPath(List<NavVertex> vertices, NavVertex begin, NavVertex end)
        {
            foreach (var vertex in vertices)
                vertex.ClearState();

            bool pathExists = false;

            var queue = new Queue<NavVertex>();
            queue.Enqueue(begin);
            begin.Visit(null);

            while (queue.Count > 0)
            {
                NavVertex processedVertex = queue.Dequeue();

                foreach (var neighbourId in processedVertex.neighbourIds)
                {
                    NavVertex neighbourVertex = vertices[neighbourId];

                    if (neighbourVertex.state == NavVertex.State.Empty)
                    {
                        neighbourVertex.Visit(from: processedVertex);
                        queue.Enqueue(neighbourVertex);
                        if (neighbourVertex.id == end.id)
                        {
                            pathExists = true;
                            break;
                        }
                    }
                        
                }

                if (pathExists) break;
            }

            if (pathExists) return BuildPath(end);
            return null;
        }


        



    }
}




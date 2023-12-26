using System.Collections.Generic;
using UnityEngine;

namespace VG.GameAI.Navigation2D
{
    [System.Serializable]
    public class LocalGrid
    {
        public enum TransitSide { Down, Right }


        private Vector2 _position;
        private float _vertexSize = 1f;
        private float _graphSize = 10f;

        private int _rows, _cols;

        private List<GridVertex> _gridVertices;
        private List<int> _localTransitVerticesIds;



        public LocalGrid(Vector2 position, float vertexSize, float graphSize)
        {
            _position = position;
            _vertexSize = vertexSize;
            _graphSize = graphSize;
            _rows = _cols = (int)(_graphSize / _vertexSize);

            Build();
        }


        public void Build()
        {
            CreateVertices();
            MakeObstacles();
            BindVertices();
        }


        private void CreateVertices()
        {
            _gridVertices = new List<GridVertex>();

            for (int y = 0; y < _rows; y++)
                for (int x = 0; x < _cols; x++)
                {
                    var vertex = new GridVertex(
                    position: new Vector2(
                        x: _position.x + _vertexSize * x + _vertexSize / 2f,
                        y: _position.y - _vertexSize * y - _vertexSize / 2f));

                    vertex.isObstacle = false;
                    _gridVertices.Add(vertex);
                }

            

        }

        private void MakeObstacles()
        {
            foreach (var obstacle in Object.FindObjectsOfType<Obstacle>())
                foreach (var vertex in _gridVertices)
                    if (obstacle.IsInside(vertex.position))
                        vertex.isObstacle = true;
        }

        private void BindVertices()
        {
            for (int y = 0; y < _rows; y++)
                for (int x = 0; x < _cols; x++)
                {
                    var vertex = _gridVertices[y * _cols + x];
                    int[] neighbours = { -1, -1, -1, -1 };

                    if (VertexAvailable(x, y - 1)) neighbours[0] = (y - 1) * _cols + x; // 0 - Up
                    if (VertexAvailable(x + 1, y)) neighbours[1] = y * _cols + x + 1; // 1 - Right
                    if (VertexAvailable(x, y + 1)) neighbours[2] = (y + 1) * _cols + x; // 2 - Down
                    if (VertexAvailable(x - 1, y)) neighbours[3] = y * _cols + x - 1; // 3 - Left

                    if (vertex.isObstacle)
                        for (int i = 0; i < 4; i++) neighbours[i] = -1;

                    vertex.Bind(neighbours);
                }
        }
           

        public List<TransitVertex> MakeTransitions(LocalGrid secondGrid, TransitSide side)
        {
            List<TransitVertex> transitions = new List<TransitVertex>();

            // »нициализаци€ списков под вершины на месте стыковки
            List<GridVertex> firstVertexRow = new List<GridVertex>(capacity: _cols);
            List<GridVertex> secondVertexRow = new List<GridVertex>(capacity: _cols);

            if (side == TransitSide.Down)
            {
                // ” первой сетки берутс€ нижние вершины
                for (int x = 0; x < _cols; x++)
                    firstVertexRow.Add(_gridVertices[(_rows - 1) * _cols + x]); 

                // ” второй сетки берутс€ верхние вершины
                for (int x = 0; x < _cols; x++)
                    secondVertexRow.Add(secondGrid._gridVertices[x]); 
            }

            else if (side == TransitSide.Right)
            {
                // ” первой сетки берутс€ правые вершины
                for (int y = 0; y < _rows; y++)
                    firstVertexRow.Add(_gridVertices[y * _cols + (_cols - 1)]); 

                // ” второй сетки берутс€ левые вершины
                for (int y = 0; y < _rows; y++)
                    secondVertexRow.Add(secondGrid._gridVertices[y * _cols]); 
            }

            List<Vector2> vertexGroupPositions = new List<Vector2>();
            for (int i = 0; i < firstVertexRow.Count; i++)
            {
                // ћежду вершинами есть проход
                if (firstVertexRow[i].isObstacle == false 
                    && secondVertexRow[i].isObstacle == false)
                {
                    Vector2 betweenPosition = 
                        (firstVertexRow[i].position + secondVertexRow[i].position) / 2f;
                    vertexGroupPositions.Add(betweenPosition);
                }
                // ћежду вершинами нет прохода
                else if (vertexGroupPositions.Count > 0)
                    // —оздание глобальной вершины посередине прохода
                    AddAverageToTransitions(vertexGroupPositions, transitions);
            }

            if (vertexGroupPositions.Count > 0)
                // —оздание глобальной вершины посередине прохода
                AddAverageToTransitions(vertexGroupPositions, transitions);

            // ¬ пару локальных сеток добавл€ютс€ идентификаторы глобальных вершин,
            // которые принадлежат этим сеткам
            _localTransitVerticesIds ??= new List<int>();
            secondGrid._localTransitVerticesIds ??= new List<int>();
            foreach (var transitVertex in transitions)
            {
                _localTransitVerticesIds.Add(transitVertex.id);
                secondGrid._localTransitVerticesIds.Add(transitVertex.id);
            }


            return transitions;
        }

        private void AddAverageToTransitions(List<Vector2> positions, List<TransitVertex> transitions)
        {
            var firstPosition = positions[0];
            var lastPosition = positions[positions.Count - 1];
            Vector2 averagePosition = (firstPosition + lastPosition) / 2f;

            var transitVertex = new TransitVertex(averagePosition, id: GlobalGrid.transitVertexCount);
            GlobalGrid.transitVertexCount++;
            transitions.Add(transitVertex);
            positions.Clear();
        }

        /// <summary>—в€зывает вершины перехода между собой в рамках одной локальной сетки.</summary>
        public void BindTransitVertices(List<TransitVertex> globalTransitVertices)
        {
            List<TransitVertex> localTransitVertices = new List<TransitVertex>();
            foreach (var transitVertexId in _localTransitVerticesIds)
                localTransitVertices.Add(globalTransitVertices[transitVertexId]);

            for (int i = 0; i < localTransitVertices.Count; i++)
            {
                for (int j = i + 1; j < localTransitVertices.Count; j++)
                {
                    var startGridVertex = GetGridVertex(localTransitVertices[i].position);
                    var finishGridVertex = GetGridVertex(localTransitVertices[j].position);

                    if (PathFinder.FindPathBFS(graph: _gridVertices,
                        start: startGridVertex, finish: finishGridVertex, out List<Vector2> path))
                        localTransitVertices[i].Bind(localTransitVertices[j], path);
                }
            }

            foreach (var localVertex in localTransitVertices)
                localVertex.FinishBinding();

        }



        private bool VertexAvailable(int x, int y) 
            => x.Between(0, _cols) && y.Between(0, _rows) && 
            _gridVertices[y * _cols + x].isObstacle == false;


        private GridVertex GetGridVertex(Vector2 position)
        {
            Vector2 offset = position - _position;
            offset.y = -offset.y;

            int x = Mathf.Clamp((int)(offset.x / _vertexSize), min: 0, max: _cols - 1);
            int y = Mathf.Clamp((int)(offset.y / _vertexSize), min: 0, max: _rows - 1);

            Debug.Log($"Get vertex: {position} => {_gridVertices[y * _cols + x].position}");
            return _gridVertices[y * _cols + x];
        }


        public void DrawGizmos()
        {
            Vector2 center = new Vector2(
                x: _position.x + _graphSize / 2,
                y: _position.y - _graphSize / 2);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(center, _graphSize * Vector2.one);

            if (_gridVertices != null)
                foreach (var vertex in _gridVertices)
                    vertex.DrawGizmos(_vertexSize);

        }



    }
}



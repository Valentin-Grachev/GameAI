using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using NaughtyAttributes;


namespace VG.GameAI.Navigation2D
{
    [System.Serializable]
    public class NavGrid : MonoBehaviour
    {
        [SerializeField] private bool _visualisation;
        [Space(10)]
        [SerializeField] private NavGridData _data; public NavGridData data { get => _data; }
        [SerializeField] private NavAlgorithm _navAlgorithm;
        [Space(10)]
        [SerializeField] private Vector2 _areaSize; public Vector2 areaSize { get => _areaSize; }
        [SerializeField] private float _vertexSize = 1; public float vertexSize { get => _vertexSize; }
        

        public const float eps = 10e-4f;

        public int cols { get => _data.cols; }
        public int rows { get => _data.rows; }


        private void Awake()
        {
            NavVertex.visualisation = _visualisation;
            LoadGrid();
        }

        private void LoadGrid()
        {
            foreach (var vertex in _data.vertices)
                vertex.navGrid = this;
        }

        public List<Vector2> FindPath(Vector2 begin, Vector2 end)
        {
            var beginVertex = _data.vertices[WorldPositionToId(begin, onlyInside: true)];
            var endVertex = _data.vertices[WorldPositionToId(end, onlyInside: true)];
            
            var vertexPath = _navAlgorithm.FindPath(_data.vertices, beginVertex, endVertex);
            
            List<Vector2> worldVertexPath = new List<Vector2>();
            if (vertexPath != null)
                foreach (var vertex in vertexPath)
                    worldVertexPath.Add(vertex.worldPosition);
            
            return SmoothPath(worldVertexPath);
        }



        [Button(nameof(Bake))]
        private void Bake()
        {
            NavVertex.visualisation = _visualisation;
            EditorUtility.SetDirty(_data);

            _data.cols = (int)(_areaSize.x / _vertexSize);
            _data.rows = (int)(_areaSize.y / _vertexSize);

            _data.vertices = new List<NavVertex>(capacity: cols * rows);

            for (int i = 0; i < cols * rows; i++)
            {
                IdToGridPosition(i, out int x, out int y);
                List<int> neighbours = new List<int>(capacity: 4);

                if (GridPositionExists(x - 1, y)) neighbours.Add(GridPositionToId(x - 1, y)); // Left = neighbours[0]
                if (GridPositionExists(x + 1, y)) neighbours.Add(GridPositionToId(x + 1, y)); // Right = neighbours[1]
                if (GridPositionExists(x, y - 1)) neighbours.Add(GridPositionToId(x, y - 1)); // Down = neighbours[2] 
                if (GridPositionExists(x, y + 1)) neighbours.Add(GridPositionToId(x, y + 1)); // Up = neighbours[3] 

                NavVertex vertex = new NavVertex(this, id: i, neighbours);
                _data.vertices.Add(vertex);
            }

            NavObstacle[] obstacles = FindObjectsOfType<NavObstacle>();
            foreach (var obstacle in obstacles)
            {
                obstacle.Place(this);
                print("Place: " + obstacle.name);
            }
            Visualise();
        }


        #region PositionConverter


        private bool GridPositionExists(int x, int y)
            => 0 <= x && x < cols && 0 <= y && y < rows;

        private void IdToGridPosition(int id, out int x, out int y)
        {
            x = id % cols;
            y = id / cols;
        }
        public int GridPositionToId(int x, int y) => y * cols + x;

        /// <summary>Convert world position to vertex id of navigation grid.</summary>
        /// <param name="worldPosition"></param>
        /// <param name="onlyInside">If true and world position doesn`t exist in grid, then will be selected nearest grid position. <br/>
        /// If false, then can be returned -1.</param>
        /// <returns>Vertex grid id, or -1 if it doesn`t exists.</returns>
        private int WorldPositionToId(Vector2 worldPosition, bool onlyInside)
        {
            if (WorldPositionToGridPosition(worldPosition, onlyInside, out int x, out int y))
                return GridPositionToId(x, y);

            return -1;
        }

        /// <returns>Grid position exists.</returns>
        public bool WorldPositionToGridPosition(Vector2 worldPosition, bool onlyInside, out int x, out int y)
        {
            x = -1; y = -1;
            Vector2 localPosition = worldPosition + _areaSize / 2 - (Vector2)transform.position;

            if (!onlyInside && (localPosition.x < 0f || localPosition.x > _areaSize.x
                || localPosition.y < 0f || localPosition.y > _areaSize.y)) return false;

            if (localPosition.x < 0f) localPosition.x = 0f;
            else if (localPosition.x >= _areaSize.x)
                localPosition.x = _areaSize.x - eps;

            if (localPosition.y < 0f) localPosition.y = 0f;
            else if (localPosition.y >= _areaSize.y)
                localPosition.y = _areaSize.y - eps;

            x = (int)(localPosition.x / _vertexSize);
            y = (int)(localPosition.y / _vertexSize);
            return true;
        }

        #endregion



        private void Visualise()
        {
            while (transform.childCount > 0)
                DestroyImmediate(transform.GetChild(0).gameObject);

            foreach (var vertex in _data.vertices)
                vertex.VisualiseState();
        }


        private List<Vector2> SmoothPath(List<Vector2> worldVertexPath)
        {
            List<Vector2> smoothedPath = new List<Vector2>();

            int fromIndex = 0;
            int endIndex = worldVertexPath.Count - 1;
            int handleIndex = endIndex;

            while (fromIndex < endIndex)
            {
                Vector2 from = worldVertexPath[fromIndex];
                Vector2 to = worldVertexPath[handleIndex];
                
                if (Physics2D.Raycast(origin: from, direction: to - from,
                    distance: Vector2.Distance(from, to), layerMask: 1 << LayerMask.NameToLayer("NavObstacle")))
                {
                    handleIndex = (fromIndex + handleIndex) / 2;
                }
                else
                {
                    smoothedPath.Add(to);
                    fromIndex = handleIndex;
                    handleIndex = endIndex;
                }
            }

            //smoothedPath.Add(worldVertexPath[endIndex]);

            //DrawPath(smoothedPath);

            return smoothedPath;
        }



        private void OnDrawGizmosSelected()
        {
            DrawGrid(Color.blue);

        }


        private void DrawGrid(Color color)
        {
            int cols = (int)(_areaSize.x / _vertexSize);
            int rows = (int)(_areaSize.y / _vertexSize);

            Gizmos.color = color;

            Vector2 startDrawPosition = (Vector2)transform.position - _areaSize / 2;
            for (int i = 0; i <= rows; i++)
            {
                Vector2 from = startDrawPosition + Vector2.up * i * _vertexSize;
                Vector2 to = from + Vector2.right * _areaSize.x;
                Gizmos.DrawLine(from, to);
            }
            for (int i = 0; i <= cols; i++)
            {
                Vector2 from = startDrawPosition + Vector2.right * i * _vertexSize;
                Vector2 to = from + Vector2.up * _areaSize.y;
                Gizmos.DrawLine(from, to);
            }
        }


        private void DrawPath(List<Vector2> path)
        {
            

            for (int i = 0; i < path.Count; i++)
            {
                Instantiate(data.obstacleVertex, position: path[i], Quaternion.identity);
            }
                
        }

    }
}




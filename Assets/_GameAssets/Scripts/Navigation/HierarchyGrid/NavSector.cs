using System.Collections.Generic;
using UnityEngine;


namespace VG.GameAI.Navigation2D
{
    public class NavSector : MonoBehaviour
    {
        private float size => 10f;

        public float vertexSize => 0.5f;

        private Neighbors<NavSector> _neighborSectors;




        /// <summary>Создает внутреннюю часть сектора, состоящую из сетки.</summary>
        public void Build()
        {
            int columns = (int)(size / vertexSize);


            for (int i = 0; i < columns * columns; i++)
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
        }

        /// <summary>Вносит в секторный граф связь данного сектора с соседними</summary>
        public void Bind(Neighbors<NavSector> neighbours)
        {

        }


        private void OnDrawGizmosSelected()
        {
            DrawGrid(Color.blue);

        }


        private void DrawGrid(Color color)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, _size);
        }



    }


}




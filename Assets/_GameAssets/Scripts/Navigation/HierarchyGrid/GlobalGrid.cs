using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace VG.GameAI.Navigation2D
{
    public class GlobalGrid : MonoBehaviour
    {
        [SerializeField] private Vector2 _size;
        [SerializeField] private float _vertexSize;
        [SerializeField] private float _localGraphSize;
        [Space(10)]
        [SerializeField] private bool _drawGizmos;

        private List<LocalGrid> _localGrids;
        private List<TransitVertex> _transitVertices;

        private int _rows, _cols;


        [Button("Build")]
        public void Build()
        {
            TransitVertex.ClearIds();
            DeleteGrid();

            CreateLocalGrids();
            BindLocalGrids();
        }

        [Button("Delete")] private void DeleteGrid()
        {
            _localGrids?.Clear();
            _transitVertices?.Clear();
        }
        
        
        private void CreateLocalGrids()
        {
            _localGrids = new List<LocalGrid>();

            _cols = (int)(_size.x / _localGraphSize);
            _rows = (int)(_size.y / _localGraphSize);

            for (int y = 0; y < _rows; y++)
                for (int x = 0; x < _cols; x++)
                    _localGrids.Add(new LocalGrid(
                        position: (Vector2)transform.position
                        + Vector2.right * x * _localGraphSize
                        - Vector2.up * y * _localGraphSize,
                        vertexSize: _vertexSize,
                        graphSize: _localGraphSize));
        }


        private void BindLocalGrids()
        {
            _transitVertices = new List<TransitVertex>();

            // —в€зывание проходит по всем локальным сеткам
            for (int y = 0; y < _rows; y++)
                for (int x = 0; x < _cols; x++)
                {
                    // —в€зывание по нижнему краю
                    if (y != _rows - 1)
                    {
                        var downVertices = _localGrids[y * _cols + x].MakeTransitions
                        (_localGrids[(y + 1) * _cols + x], LocalGrid.TransitSide.Down);

                        _transitVertices.AddRange(downVertices);
                    }
                    // —в€зывание по правому краю
                    if (x != _cols - 1)
                    {
                        var rightVertices = _localGrids[y * _cols + x].MakeTransitions
                        (_localGrids[y * _cols + x + 1], LocalGrid.TransitSide.Right);

                        _transitVertices.AddRange(rightVertices);
                    }
                }

            // ѕостроение ребер между глобальными вершинами
            // относительной каждой локальной сетки
            foreach (var localGrid in _localGrids)
                localGrid.BindTransitVertices(_transitVertices);
        }




        private void OnDrawGizmos()
        {
            if (!_drawGizmos) return;

            Vector2 center = new Vector2(
                x: transform.position.x + _size.x / 2,
                y: transform.position.y - _size.y / 2);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, _size);

            if (_localGrids != null)
                foreach (var localGrid in _localGrids)
                    localGrid.DrawGizmos();

            if (_transitVertices != null)
                foreach (var transitVertex in _transitVertices)
                    transitVertex.DrawGizmos(_vertexSize, _transitVertices);
        }



    }
}



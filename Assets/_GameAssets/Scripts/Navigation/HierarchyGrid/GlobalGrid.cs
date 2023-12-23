using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VG.GameAI.Navigation2D
{
    public class GlobalGrid : MonoBehaviour
    {
        [SerializeField] private Vector2 _size;
        [SerializeField] private float _vertexSize;
        [SerializeField] private float _localGraphSize;

        private List<LocalGrid> _localGrids;



        [Button("Build")]
        public void Build()
        {
            CreateLocalGrids();
            BindLocalGrids();
        }


        
        private void CreateLocalGrids()
        {
            _localGrids = new List<LocalGrid>();

            int cols = (int)(_size.x / _localGraphSize);
            int rows = (int)(_size.y / _localGraphSize);

            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                    _localGrids.Add(new LocalGrid(
                        position: (Vector2)transform.position
                        + Vector2.right * x * _localGraphSize
                        - Vector2.up * y * _localGraphSize,
                        vertexSize: _vertexSize,
                        graphSize: _localGraphSize));
        }


        private void BindLocalGrids()
        {

        }


        private void OnDrawGizmosSelected()
        {
            Vector2 center = new Vector2(
                x: transform.position.x + _size.x / 2,
                y: transform.position.y - _size.y / 2);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, _size);

            if (_localGrids != null)
                foreach (var localGrid in _localGrids)
                    localGrid.DrawGizmos();
        }



    }
}



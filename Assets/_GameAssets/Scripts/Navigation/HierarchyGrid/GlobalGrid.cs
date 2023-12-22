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




        public void Build()
        {
            CreateLocalGrids();
            BindLocalGrids();
        }


        private void CreateLocalGrids()
        {
            _localGrids = new List<LocalGrid>();

        }


        private void BindLocalGrids()
        {

        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, _size);

            if (_localGrids != null)
                foreach (var localGrid in _localGrids)
                    localGrid.DrawGizmos();
        }



    }
}



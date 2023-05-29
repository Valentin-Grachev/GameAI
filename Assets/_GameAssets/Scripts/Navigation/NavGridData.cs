using System.Collections.Generic;
using UnityEngine;

namespace VG.GameAI.Navigation2D
{
    [CreateAssetMenu(menuName = "VG/Navigation/NavGridData")]
    public class NavGridData : ScriptableObject
    {

        [Header("Visualization:")]
        public GameObject emptyVertex;
        public GameObject visitedVertex;
        public GameObject pathVertex;
        public GameObject startVertex;
        public GameObject obstacleVertex;

        public List<NavVertex> vertices;

        [HideInInspector] public int rows;
        [HideInInspector] public int cols;
        



    }
}



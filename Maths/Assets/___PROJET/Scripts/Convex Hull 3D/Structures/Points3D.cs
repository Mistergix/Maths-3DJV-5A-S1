using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using PGSauce.Core.Utilities;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace ESGI.ConvexHull3D
{
    [CreateAssetMenu(menuName = "ESGI/Points 3D")]
    public class Points3D : ScriptableObject, IEnumerable<Vector3>
    {
        [SerializeField, MinValue(3), MaxValue("MaxPossibleQ")] private int maxQ = 4;
        public bool stopAtColoring;
        public List<Vector3> positions;

        public int MaxQ => Mathf.Min(positions.Count - 1, maxQ);
        public Vector3 MaxQPoint => positions[MaxQ];

        [Button]
        private void GenerateRandomPoints(MinMax<Vector3> range, int count = 20)
        {
            positions.Clear();
            for (var i = 0; i < count; i++)
            {
                positions.Add(PGUtils.RandomVector3(range));
            }
        }

        public void AddPoint(Vector3 worldPos)
        {
            positions.Add(worldPos);
        }

        public IEnumerator<Vector3> GetEnumerator()
        {
            return ((IEnumerable<Vector3>) positions).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [UsedImplicitly]
        private int MaxPossibleQ()
        {
            return positions.Count - 1;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ESGI.ConvexHull3D
{
    public abstract class Points3DBase : ScriptableObject, IEnumerable<Vector3>
    {
        [SerializeField, MinValue(3), MaxValue("MaxPossibleQ")] private int maxQ = 4;
        public bool stopAtColoring;
        public abstract List<Vector3> Positions { get; }
        public int MaxQ => Mathf.Min(Positions.Count - 1, maxQ);
        public Vector3 MaxQPoint => Positions[MaxQ];
        
        public abstract IEnumerator<Vector3> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        [UsedImplicitly]
        private int MaxPossibleQ()
        {
            return Positions.Count - 1;
        }
    }
}
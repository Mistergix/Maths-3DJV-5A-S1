using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ESGI.ConvexHull3D
{
    [CreateAssetMenu(menuName = "ESGI/Points 3D")]
    public class Points3D : ScriptableObject, IEnumerable<Vector3>
    {
        public List<Vector3> positions;
        
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
    }
}
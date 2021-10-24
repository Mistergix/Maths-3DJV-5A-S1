using System.Collections.Generic;
using PGSauce.Core.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ESGI.Common
{
    [CreateAssetMenu(menuName = "ESGI/Points")]
    public class Points : ScriptableObject
    {
        public List<Vector2> positions;
        
        [Button]
        private void GenerateRandomPoints(MinMax<Vector3> range, int count = 20)
        {
            positions.Clear();
            range.max.z = 0;
            range.min.z = 0;
            for (var i = 0; i < count; i++)
            {
                positions.Add(PGUtils.RandomVector3(range));
            }
        }

        public void AddPoint(Vector3 worldPos)
        {
            positions.Add(worldPos);
        }
    }
}
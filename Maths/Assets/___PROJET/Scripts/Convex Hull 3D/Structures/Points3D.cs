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
    public class Points3D : Points3DBase
    {
        public List<Vector3> positions;
        

        [Button]
        private void GenerateRandomPoints(MinMax<Vector3> range, int count = 20)
        {
            Positions.Clear();
            for (var i = 0; i < count; i++)
            {
                Positions.Add(PGUtils.RandomVector3(range));
            }
        }

        public void AddPoint(Vector3 worldPos)
        {
            Positions.Add(worldPos);
        }

        public override List<Vector3> Positions => positions;

        public override IEnumerator<Vector3> GetEnumerator()
        {
            return ((IEnumerable<Vector3>) Positions).GetEnumerator();
        }


        
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ESGI.ConvexHull2D
{
    public class Jarvis : ConvexHull2D
    {
        protected override List<Vector2> ComputeHull(List<Vector2> vectors)
        {
            if (vectors.Count <= 2)
            {
                return vectors.ToList();
            }
            var pivotIndex = GetPivotPoint(vectors);
            var pivot = vectors[pivotIndex];
            var hull = new List<Vector2>();
            var i = 0;
            Vector2 endpoint;
            do
            {
                hull.Add(pivot);
                endpoint = vectors[0];
                foreach (var point in vectors)
                {
                    if (endpoint.Equals(pivot) || IsToTheRight(point, new OrientatedLine(endpoint, hull[i])))
                    {
                        endpoint = point;
                    }
                }

                i++;
                pivot = endpoint;
            } while (! endpoint.Equals(hull[0]));

            return hull;
        }

    }
}

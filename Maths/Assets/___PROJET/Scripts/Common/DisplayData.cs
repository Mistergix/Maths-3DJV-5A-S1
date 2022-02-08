using ESGI.ConvexHull3D;
using PGSauce.Core;
using UnityEngine;

namespace ESGI.Common
{
    [CreateAssetMenu(menuName = "ESGI/Display Data")]
    public class DisplayData : ScriptableObject
    {
        public Color pointColor = Color.white;
        public float pointSize = 0.5f;
        public float arrowSize = 0.2f;
        public float arrowStretchFactor = 1;
        public float arrowOffset = 0.2f;
        public float planeSize = 1;
        public Color planeColor = Color.white;
    }
}
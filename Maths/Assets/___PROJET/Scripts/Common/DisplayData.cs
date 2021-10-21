using PGSauce.Core;
using UnityEngine;

namespace ESGI.Common
{
    [CreateAssetMenu(menuName = "ESGI/Display Data")]
    public class DisplayData : ScriptableObject
    {
        public Color pointColor = Color.white;
        public Color barycentreColor = PGColors.Greenish;
        public float pointSize = 0.5f;
    }
}
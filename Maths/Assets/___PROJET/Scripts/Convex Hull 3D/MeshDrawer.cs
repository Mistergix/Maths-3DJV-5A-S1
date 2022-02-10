using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace ESGI.ConvexHull3D
{
    public class MeshDrawer : MonoBehaviour
    {
        public void DrawMesh(IncidenceGraph convexHull)
        {
            PGDebug.Message($"DRAW MESH CEDRIC, {convexHull.vertices.Count} vertices et {convexHull.faces.Count} faces").LogTodo();
        }
    }
}
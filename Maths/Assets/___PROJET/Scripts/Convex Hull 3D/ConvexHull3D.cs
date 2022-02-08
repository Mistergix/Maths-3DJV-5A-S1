using System;
using System.Collections.Generic;
using System.Linq;
using ESGI.Common;
using PGSauce.Core.PGDebugging;
using Shapes;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ESGI.ConvexHull3D
{
    public class ConvexHull3D : ImmediateModeShapeDrawer
    {
        [SerializeField] private Points3D points;
        public DisplayData Data => displayData;

        [SerializeField] private DisplayData displayData;
        
        private IncidenceGraph _tetrahedre;

        private void Update()
        {
            ComputeHull();
        }

        public override void DrawShapes(Camera cam)
        {
            base.DrawShapes(cam);
            using (Draw.Command(cam))
            {
                if(_tetrahedre == null){return;}
                _tetrahedre.DrawGraph(Data);
            }
        }

        [Button]
        public void ComputeHull()
        {
            _tetrahedre = ComputeTetrahedre();
        }

        private IncidenceGraph ComputeTetrahedre()
        {
            if (points.positions.Count < 4)
            {
                PGDebug.Message($"Pas assez de points dans {points.name} (au moins 4)").LogError();
            }

            var iGraph = new IncidenceGraph();

            var four = points.positions.Take(4).ToList();
            
            var vertices = four.Select(point => new Vertex3D(point)).ToList();

            var edge01 = new Edge3D(vertices[0], vertices[1]);
            var edge02 = new Edge3D(vertices[0], vertices[2]);
            var edge12 = new Edge3D(vertices[1], vertices[2]);
            var edge03 = new Edge3D(vertices[0], vertices[3]);
            var edge23 = new Edge3D(vertices[2], vertices[3]);
            var edge13 = new Edge3D(vertices[1], vertices[3]);

            var face012 = new Face3D(edge01, edge12, edge02);
            var face013 = new Face3D(edge01, edge13, edge03);
            var face123 = new Face3D(edge12, edge23, edge13);
            var face023 = new Face3D(edge02, edge03, edge23);

            edge01.face1 = face012;
            edge01.face2 = face013;

            edge02.face1 = face023;
            edge02.face1 = face012;

            edge03.face1 = face013;
            edge03.face2 = face023;

            edge12.face1 = face012;
            edge12.face2 = face123;

            edge23.face1 = face023;
            edge23.face2 = face123;

            edge13.face1 = face123;
            edge13.face2 = face013;

            iGraph.AddVertices(vertices);
            iGraph.AddEdges(new List<Edge3D>() {edge01, edge02, edge03, edge12, edge13, edge23});
            iGraph.AddFaces(new List<Face3D>() {face012, face013, face023, face123});

            return iGraph;
        }
    }
}
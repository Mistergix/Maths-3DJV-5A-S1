using System;
using System.Collections.Generic;
using System.Linq;
using ESGI.Common;
using Kit;
using PGSauce.Core;
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
        [SerializeField, Min(4)] private int currentQ = 4;

        public DisplayData Data => displayData;

        [SerializeField] private DisplayData displayData;
        
        private IncidenceGraph _convexHull;

        private void Update()
        {
            ComputeHull();
        }

        public override void DrawShapes(Camera cam)
        {
            base.DrawShapes(cam);
            using (Draw.Command(cam))
            {
                if(_convexHull == null){return;}
                _convexHull.DrawGraph(Data);
            }
        }

        [Button]
        public void ComputeHull()
        {
            _convexHull = ComputeTetrahedre();
            for (int q = 4; q < Math.Min(points.positions.Count, currentQ + 1); q++)
            {
                var point = this.points.positions[q];
                foreach (var face in _convexHull.faces)
                {
                    face.SetColorFromPoint(point);
                }
            }
            Drawing.Draw.WireSphere(points.positions[currentQ], 0.5f, PGColors.Redish);
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
            var edge20 = new Edge3D(vertices[2], vertices[0]);
            var edge12 = new Edge3D(vertices[1], vertices[2]);
            var edge30 = new Edge3D(vertices[3], vertices[0]);
            var edge23 = new Edge3D(vertices[2], vertices[3]);
            var edge31 = new Edge3D(vertices[3], vertices[1]);

            var face012 = new Face3D(vertices[2], vertices[1], vertices[0]);
            var face013 = new Face3D(vertices[0], vertices[1], vertices[3]);
            var face123 = new Face3D(vertices[1], vertices[2], vertices[3]);
            var face023 = new Face3D(vertices[3], vertices[2], vertices[0]);

            edge01.face1 = face012;
            edge01.face2 = face013;

            edge20.face1 = face023;
            edge20.face1 = face012;

            edge30.face1 = face013;
            edge30.face2 = face023;

            edge12.face1 = face012;
            edge12.face2 = face123;

            edge23.face1 = face023;
            edge23.face2 = face123;

            edge31.face1 = face123;
            edge31.face2 = face013;

            iGraph.AddVertices(vertices);
            iGraph.AddEdges(new List<Edge3D>() {edge01, edge20, edge30, edge12, edge31, edge23});
            iGraph.AddFaces(new List<Face3D>() {face012, face013, face023, face123});

            return iGraph;
        }
    }
}
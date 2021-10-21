using System;
using System.Collections.Generic;
using System.Linq;
using ESGI.Common;
using ESGI.Structures;
using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace ESGI.Triangulation
{
    public class IncrementalTriangulation : TriangulationBase
    {
        public override Color LineColor => Color.white;

        protected override List<Triangle> Triangulate(List<Vertex> sites)
        {
            return Geom.IncrementalTriangulate2D(sites);
        }
        
        
    }
}
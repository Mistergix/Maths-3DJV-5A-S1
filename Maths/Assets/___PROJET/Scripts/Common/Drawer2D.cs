using System;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

namespace ESGI.Common
{
    public abstract class Drawer2D : ImmediateModeShapeDrawer
    {
        [SerializeField] private Points points;
        protected List<Vector2> Positions => points.positions;

        public DisplayData Data => displayData;

        [SerializeField] private DisplayData displayData;

        protected void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var mousePos = Input.mousePosition;
                mousePos.z = 0;
                var worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                for (var i = 0; i < Positions.Count; i++)
                {
                    var newDist = Vector2.Distance(Positions[i], worldPos);
                    if (newDist <= displayData.pointSize)
                    {
                        points.positions[i] = worldPos;
                    }
                }
            }
            CustomUpdate();
        }

        protected abstract void CustomUpdate();
    }
}
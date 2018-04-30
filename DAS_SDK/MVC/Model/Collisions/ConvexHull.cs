using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DAS_SDK.MVC.Model.Collisions
{
    class ConvexHull
    {
        Front_END.Front_END front_END;
        List<Point> points = new List<Point>();

        public ConvexHull(Front_END.Front_END front_END)
        {
            this.front_END = front_END;
        }

        public List<Point> GetConvexHull(List<Point> points)
        {
            List<Point> pointsSwap = new List<Point>(points.Capacity);
            double lastAngle = 0;
            double currentAngle = 0;
            for (int i = 0; i < points.Count-1; i++)
            {
                currentAngle = Vector.AngleBetween(new Vector(points[i].X, points[i].Y), new Vector(points[i + 1].X, points[i + 1].Y));
                if (((points[i].X <= points[i + 1].X || currentAngle < lastAngle) && (points[i].Y < points[i + 1].Y || currentAngle > lastAngle)) 
                    ||
                    ((points[i].X >= points[i + 1].X || currentAngle < lastAngle) && (points[i].Y > points[i + 1].Y || currentAngle > lastAngle))
                    )
                {
                    pointsSwap.Add(points[i]);
                }
                lastAngle = currentAngle;
            }
            return this.points = pointsSwap;
        }

    }
}

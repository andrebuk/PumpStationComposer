using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace SeverMineralsCommon
{
    class CalcPoints
    {
        public XYZ startPoint;
        public XYZ endPoint;
        public double segmentLength;
        //public bool isAligned;
        //public List<XYZ> resultPoints;

    public List<XYZ> calcPOintsList()
        {
            List<XYZ> result = new List<XYZ>();
            double fullLength = startPoint.DistanceTo(endPoint);
            int numberOfPoints = (int)(fullLength / segmentLength);

            double alfa = Math.Atan((endPoint.Y-startPoint.Y) / (endPoint.X-startPoint.X));
            if (alfa==0.0 & startPoint.X>endPoint.X)
            {
                alfa = -1 * Math.PI;
            }
                double deltaXForSegment = segmentLength * Math.Cos(alfa);
            double deltaYForSegment = segmentLength * Math.Sin(alfa);
            XYZ deltaXYZForSegment = new XYZ(deltaXForSegment, deltaYForSegment, 0.0);

            XYZ currentXYZ = startPoint;
            
            for (int i = 0; i < numberOfPoints; i++)
            {
                result.Add(currentXYZ);
                currentXYZ = currentXYZ + deltaXYZForSegment;
            }


            return result;
        }
        public double CalcAngle()
        {
            double alfa = Math.Atan((endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X));
            if (alfa == 0.0 & startPoint.X > endPoint.X)
            {
                alfa = -1 * Math.PI;
            }
            return alfa;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace CoreAnimation
{
    public class UIBezierPath : IDisposable
    {
        private CanvasPathBuilder mBuilder;  
        private CanvasFigureLoop mFigureLoop = CanvasFigureLoop.Open;
        public float LineWidth { get; set; }
        public UIBezierPath()
        {
            mBuilder = new CanvasPathBuilder(CanvasDevice.GetSharedDevice());
        }

        public UIBezierPath(Rect rect)
        {
            mBuilder = new CanvasPathBuilder(CanvasDevice.GetSharedDevice()); 
            mBuilder.BeginFigure((float) rect.X, (float) rect.Y);
            mBuilder.AddLine((float)rect.Right, (float)rect.Y);
            mBuilder.AddLine((float)rect.Right, (float)rect.Bottom);
            mBuilder.AddLine((float)rect.X, (float)rect.Bottom);
        }

        public UIBezierPath(Vector2 center, float radius, float startAngle, float endAngle, bool clockwise)
        {
            mBuilder = new CanvasPathBuilder(CanvasDevice.GetSharedDevice());
            float sweepAngle = endAngle;
            //float sa = RadianToDegrees(startAngle);
            //float ea = RadianToDegrees(endAngle);
            mBuilder.AddArc(center, radius, radius, startAngle, sweepAngle);
        }

        public UIBezierPath(Vector2 center, float radius)
        {
            mBuilder = new CanvasPathBuilder(CanvasDevice.GetSharedDevice());

            mBuilder.AddArc(center, radius, radius, DegreesToRadians(0), DegreesToRadians(360));
        }

        public void Dispose()
        {
            mBuilder?.Dispose();
        }

        public void MoveToPoint(Vector2 point)
        {
            mBuilder.BeginFigure(point);
        }

        public void AddLineTo(Vector2 point)
        {
            mBuilder.AddLine(point);
        }

        public void AddCurveToPoint(Vector2 pointEnd, Vector2 controlPoint1, Vector2 controlPoint2)
        {
            mBuilder.AddCubicBezier(controlPoint1, controlPoint2, pointEnd);
        }

        public void AddQuadCurveToPoint(Vector2 endPoint, Vector2 controlPoint)
        {
            mBuilder.AddQuadraticBezier(controlPoint, endPoint);
        }
        public void ClosePath()
        {
            mFigureLoop = CanvasFigureLoop.Closed;
        }

        public void SetLineDash()
        {
            //CanvasStrokeStyle
        }

        internal CanvasGeometry GetPath()
        {
            //if (_OtherGeometry == null)
            //{
            //    mBuilder.EndFigure(CanvasFigureLoop.Closed);
            //    CanvasGeometry geometry = CanvasGeometry.CreatePath(mBuilder);
            //    mBuilder.Dispose();
            //    return geometry;
            //}

            //return _OtherGeometry; 
            mBuilder.EndFigure(mFigureLoop);
            CanvasGeometry geometry = CanvasGeometry.CreatePath(mBuilder);
            return geometry;
        }


        static float DegreesToRadians(float angle)
        {
            return angle * (float)Math.PI / 180;
        }

        static float RadianToDegrees(float angle)
        {
            return angle * 180 / (float) Math.PI;
        }
         
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Microsoft.Graphics.Canvas.Brushes;

namespace CoreAnimation.Controls
{
    public sealed class SlideLayer : CALayer
    {
        public float BackgroundWidth { get; set; }
        public float Distance { get; set; }
        public bool IsAnimating { get; set; }
        public ICanvasBrush Brush { get; set; }
        public float ScreenHeight { get; set; }
        public override void Draw(TimerTick tick)
        {
            UIBezierPath _bezier = new UIBezierPath();
            Vector2 pointStart, pointA, pointB, pointEnd;
            if (IsAnimating)
            {
                pointStart = new Vector2(Distance, 0);
                pointA = new Vector2(BackgroundWidth, ScreenHeight / 4);
                pointB = new Vector2(BackgroundWidth, ScreenHeight * 3 / 4);
                pointEnd = new Vector2(Distance, ScreenHeight);
                _bezier.MoveToPoint(Vector2.Zero);
                _bezier.AddLineTo(pointStart);
                _bezier.AddCurveToPoint(pointEnd,pointA,pointB);
                _bezier.AddLineTo(new Vector2(0,ScreenHeight));
            }
            else
            {
                pointStart = Vector2.Zero;
                pointA = new Vector2(Distance, ScreenHeight / 4);
                pointB = new Vector2(Distance, ScreenHeight * 3 / 4);
                pointEnd = new Vector2(0, ScreenHeight);
                _bezier.MoveToPoint(pointStart);
                _bezier.AddCurveToPoint(pointEnd, pointA, pointB);
            }
            _bezier.ClosePath();
            Drawer.FillGeometry(_bezier.GetPath(), Brush);
            base.Draw(tick);
        }

        protected override void OnChanged(float position, string propertyName)
        {
            switch (propertyName)
            {
                case "distance":
                    Distance = position;
                    break;
                case "radius":
                    break;
            }
        }
    }
}

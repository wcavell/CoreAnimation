using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas.Geometry;

namespace CoreAnimation
{
    public class Path : IDisposable
    {
        internal CanvasGeometry Geometry { get; set; }
        internal DrawType DrawType { get; set; }
        internal float Width { get; set; }
        public void Dispose()
        {
            Geometry?.Dispose();
        }
    }
}

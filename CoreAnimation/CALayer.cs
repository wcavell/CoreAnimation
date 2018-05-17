using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace CoreAnimation
{ 
    public abstract class CALayer:DependencyObject
    {
        internal void SetDrawer(CanvasDrawingSession draw)
        {
            Drawer = draw;
        }
        protected CanvasDrawingSession Drawer;
        public List<CALayer> Layers { get; set; } = new List<CALayer>();
        public virtual void Draw()
        {
            
        }

        public void AddSubLayer(CALayer layer)
        {
            Layers.Add(layer);
        }

        public void InsertSubLayer(CALayer layer,int index)
        {
            Layers.Insert(index,layer);
        }
        public void RemoveLayer(CALayer layer)
        {
            Layers.Remove(layer);
        }

        public void RemoveLayer(int index)
        {
            if (Layers.Count > index)
            {
                Layers.RemoveAt(index);
            }
        }

        public void AddAnimation(CAAnimation animation)
        {
            animation.PropertyChanged -= OnPropertyChanged;
            animation.PropertyChanged += OnPropertyChanged; 
        }

        protected virtual void OnPropertyChanged(object sender,  PropertyChangedEventArgs e)
        {
            
        }

        protected float GetPosition(object obj)
        {
            if (obj is CAAnimation anim)
            {
                return anim.Position;
            }
            return 0;
        }
    }
}

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
        public List<CALayer> Childs { get; set; } = new List<CALayer>();
        protected List<CAAnimation> Animations { get; set; } = new List<CAAnimation>();

        public void UpdateAnimation(TimerTick tick)
        {
            foreach (var animation in Animations)
            {
                animation.Update(tick);
            }
            //foreach (var layer in Childs)
            //{
            //    layer.UpdateAnimation(tick);
            //}
        }

        public virtual void Draw(TimerTick tick)
        {
            foreach (var layer in Childs)
            {
                layer.SetDrawer(Drawer);
                layer.UpdateAnimation(tick);
                layer.Draw(tick);
            }
        }

        public void AddSubLayer(CALayer layer)
        {
            Childs.Add(layer);
        }

        public void InsertSubLayer(CALayer layer,int index)
        {
            Childs.Insert(index,layer);
        }
        public void RemoveLayer(CALayer layer)
        {
            Childs.Remove(layer);
        }

        public void RemoveLayer(int index)
        {
            if (Childs.Count > index)
            {
                Childs.RemoveAt(index);
            }
        }

        public void AddAnimation(CAAnimation animation)
        {
            Animations.Add(animation);
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

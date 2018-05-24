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
using CoreAnimation.Tweens;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace CoreAnimation
{
    public delegate void AnimationDidStop(CAAnimation animation, bool flag);
    public abstract class CALayer:DependencyObject
    {
        internal void SetDrawer(CanvasDrawingSession draw)
        {
            Drawer = draw;
        }

        public event AnimationDidStop AnimationDidStop;
        protected CanvasDrawingSession Drawer;
        public List<CALayer> Childs { get; protected set; } = new List<CALayer>();
        protected Dictionary<string,CAAnimation> Animations { get; set; } = new Dictionary<string, CAAnimation>();
        private bool mNeedDraw = true;
        
        public bool NeedDraw
        {
            get
            {
                if (!mNeedDraw)
                {
                    //foreach (var layer in Childs)
                    //{
                    //    layer.NeedDraw
                    //}
                }
                return mNeedDraw;
            }
            protected set { mNeedDraw = value; }
        }

        public void UpdateAnimation(TimerTick tick)
        {
            foreach (var animation in Animations)
            {
                animation.Value.Update(tick);
            }
            //foreach (var layer in Childs)
            //{
            //    layer.UpdateAnimation(tick);
            //}
        }

        public void UnLoaded()
        {
            foreach (var child in Childs)
            {
                child.UnLoaded();
            }

            foreach (var an in Animations)
            {
                AnimationDidStop?.Invoke(an.Value, false);
            }
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
            NeedDraw = true;
            Animations[animation.ForKey] = animation;
            animation.PropertyChanged -= OnPropertyChanged;
            animation.PropertyChanged += OnPropertyChanged;
            animation.Completed -= OnAnimationCompleted;
            animation.Completed += OnAnimationCompleted;
        }

        public void RemoveAnimation(CAAnimation animation)
        {
            animation.PropertyChanged -= OnPropertyChanged;
            animation.Completed -= OnAnimationCompleted;
            Animations.Remove(animation.ForKey);
        }

        public void RemoveAllAnimations()
        {
            foreach (var an in Animations)
            {
                an.Value.PropertyChanged -= OnPropertyChanged;
                an.Value.Completed -= OnAnimationCompleted;
                Animations.Remove(an.Key);
            }
        }
        protected virtual void OnChanged(float position, string propertyName)
        {
            
        }

        protected virtual void OnCompleted(CAAnimation animation,bool finished)
        {

        }
        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnChanged(GetPosition(sender), e.PropertyName);
        }

        float GetPosition(object obj)
        {
            if (obj is CAAnimation anim)
            {
                return anim.Position;
            }
            return 0;
        }

        private void OnAnimationCompleted(ITweener tweener)
        {
            if (tweener.IsCompleted)
            {
                CAAnimation am = (CAAnimation) tweener;
                AnimationDidStop?.Invoke(am,true);
                OnCompleted(am, true);
                Animations.Remove(am.ForKey);
            }
        }
    }
}

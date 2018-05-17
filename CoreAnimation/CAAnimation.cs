using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CoreAnimation.Tweens;

namespace CoreAnimation
{
    public abstract class CAAnimation:BaseTweener<float>
    {
        public string ForKey { get; set; }
        protected CAAnimation(string propertyName, float from, float to, float duration, TweeningFunction tweeningFunction) : base(propertyName,from, to, duration, tweeningFunction)
        {
        }

        protected CAAnimation(string propertyName, float from, float to, float duration) : base(propertyName,from, to, duration)
        {
        }

        protected CAAnimation(string propertyName, float from, float to, TimeSpan duration, TweeningFunction tweeningFunction) : base( propertyName, from, to, duration, tweeningFunction)
        {
        }

        protected CAAnimation(string propertyName, float from, float to, TimeSpan duration) : base(propertyName, from, to, duration)
        {
        }

        protected CAAnimation(TweeningFunction tweeningFunction) : base(tweeningFunction)
        {
        }

        protected CAAnimation(string propertyName, float from, float to, TweeningFunction tweeningFunction, float speed)
            : base(propertyName, from, to, tweeningFunction, speed)
        {
        }
    }
}

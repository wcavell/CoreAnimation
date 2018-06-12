using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreAnimation.Tweens;

namespace CoreAnimation
{
    public class CAKeyFrameAnimation:CAAnimation
    {
        protected override void UpdatePosition(float elapsed, float @from, float change, float duration)
        {
            Position = tweeningFunction(elapsed, from, change, duration);
        }

        protected override float CalculateChange(float to, float @from)
        {
            return to - from;
        }

        protected override float CalculateEndPosition()
        {
            return from + change;
        }

        protected override float CalculateDurationFromSpeed(float speed)
        {
            return change / speed;
        }

        public CAKeyFrameAnimation(string propertyName, float @from, float to, float duration, TweeningFunction tweeningFunction) : base(propertyName, @from, to, duration, tweeningFunction)
        {
        }

        public CAKeyFrameAnimation(string propertyName, float @from, float to, float duration) : base(propertyName, @from, to, duration)
        {
        }

        public CAKeyFrameAnimation(string propertyName, float @from, float to, TimeSpan duration, TweeningFunction tweeningFunction) : base(propertyName, @from, to, duration, tweeningFunction)
        {
        }

        public CAKeyFrameAnimation(string propertyName, float @from, float to, TimeSpan duration) : base(propertyName, @from, to, duration)
        {
        }

        public CAKeyFrameAnimation(TweeningFunction tweeningFunction) : base(tweeningFunction)
        {
        }

        public CAKeyFrameAnimation(string propertyName, float @from, float to, TweeningFunction tweeningFunction, float speed) : base(propertyName, @from, to, tweeningFunction, speed)
        {
        }
    }
}

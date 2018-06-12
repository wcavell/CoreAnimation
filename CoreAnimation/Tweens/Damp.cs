using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAnimation.Tweens
{
    /// <summary>
    /// 弹性
    /// </summary>
    public class Damp:EasingFunction
    {
        public float Damping { get; set; }
        public float Velocity { get; set; }
        private const float DampingFactor = 20.0f;
        private const float VelocityFactor = 15.0f;
        public override float Tween(float timeElapsed, float start, float change, float duration)
        {
            var damping = Damping * DampingFactor;
            var velocity = Velocity * VelocityFactor;  
            float timeRadio = timeElapsed / duration;
            return (start + change) - change * (float)Math.Pow(Math.E, -damping * timeRadio) *
                   (float)Math.Cos(velocity * timeRadio);
        }
    }
}

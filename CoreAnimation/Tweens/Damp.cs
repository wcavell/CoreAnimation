using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAnimation.Tweens
{
    public class Damp:EasingFunction
    {
        public float Damping { get; set; }
        public float Velocity { get; set; }
        private const float DampingFactor = 20.0f;
        private const float VelocityFactor = 15.0f;
        public override float Tween(float timeElapsed, float start, float change, float duration)
        {
            var damping = Damping;
            var velocity = Velocity;
            damping = damping * DampingFactor;
            velocity = velocity * VelocityFactor;
            double pg = timeElapsed / duration;
            return (start + change) - change * (float)Math.Pow(Math.E, -damping * pg) *
                   (float)Math.Cos(velocity * pg);
        }
    }
}

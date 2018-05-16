using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAnimation
{
    public abstract class CAAnimation
    {
        public float Value { get; protected set; }
        public float From { get; set; }
        public float To { get; set; }
        public float Duration { get; set; }
        //public abstract float UpdatePosition(float elapsed, float from, float change, float duration);
    }
}

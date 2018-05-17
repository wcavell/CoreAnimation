﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAnimation.Tweens
{
    public static class Sinusoidal
    {
        public static float EaseIn(float t, float b, float c, float d)
        {
            return -c * (float)Math.Cos(t / d * (Math.PI / 2)) + c + b;
        }
        public static float EaseOut(float t, float b, float c, float d)
        {
            return c * (float)Math.Sin(t / d * (Math.PI / 2)) + b;
        }
        public static float EaseInOut(float t, float b, float c, float d)
        {
            return -c / 2 * ((float)Math.Cos(Math.PI * t / d) - 1) + b;
        }
    }
}

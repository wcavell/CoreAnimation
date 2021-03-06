﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAnimation.Tweens
{
    public delegate void PositionChangedHandler<T>(T newPosition);
    public delegate void EndHandler();
    public delegate void EndExHandler(ITweener tweener);

    public interface ITweener
    {
        bool IsCompleted { get; }
        bool Playing { get; }
        event EndHandler Ended;
        event EndExHandler Completed;
        void Update(TimerTick gameTime);  
        void Play();
        void Pause();
        void Reset();
        void Restart();
        void Reverse();
    }

    public interface ITweener<T> : ITweener
    {
        T Position { get; }
        event PositionChangedHandler<T> PositionChanged;

        void Reset(T to);
        void Reset(T to, TimeSpan duration);
        void Reset(T to, float speed);
        void Reset(T from, T to, TimeSpan duration);
        void Reset(T from, T to, float speed);
    }
}

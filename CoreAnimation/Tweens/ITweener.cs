using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAnimation.Tweens
{
    public delegate void PositionChangedHandler<T>(T newPosition);
    public delegate void EndHandler();

    public interface ITweener
    { 
        bool Playing { get; }
        event EndHandler Ended;

        void Update(float gameTime);  
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

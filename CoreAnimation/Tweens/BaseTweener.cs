using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CoreAnimation.Tweens
{
    /// <summary>
    /// This is the delegate of the tweening functions.
    /// All functions must calculate the current position of the tweener based on how long has elapsed,
    /// where to start, the total amount to move and the total duration.
    /// See the library classes for several useful tweening functions.
    /// </summary>
    /// <param name="timeElapsed">The time that has elapsed since the beginning of the tweener.</param>
    /// <param name="start">Where did the tweener start</param>
    /// <param name="change">How much will the tweener move from start to end</param>
    /// <param name="duration">The total duration of tweening.</param>
    /// <returns></returns>
    public delegate float TweeningFunction(float timeElapsed, float start, float change, float duration);

    /// <summary>
    /// The BaseTweener class handles moving a Position from start to end in the specified time using a specific function.
    /// Whenever the Tweener is updated, which is done by a call to Update, it will move the Position further along 
    /// the path to completion. On each update an updated event is called so you can respond to the Position change.
    /// When the Tweener has reached the end it will stop and signal that is it finished using the Ended event.
    /// It is possible to stop the Tweener, pausing it until it is started again.
    /// You can also reset the Tweener to repeat the same movement, reset it with new parameters or even reverse the
    /// direction of the tweener.
    /// 
    /// Note that this is an abstract class, refer to the concrete subclasses for tweening the value you want tweened.
    /// </summary>
    public abstract class BaseTweener<T> : ITweener<T>,INotifyPropertyChanged
    {
        public static readonly TweeningFunction Linear = Tweens.Linear.EaseNone;
        public static readonly TweeningFunction BounceEaseIn = Tweens.Bounce.EaseIn;
        public static readonly TweeningFunction BounceEaseOut = Tweens.Bounce.EaseOut;
        public static readonly TweeningFunction BouncecEaseInOut = Tweens.Bounce.EaseInOut;
        public static readonly TweeningFunction CubicEaseIn = Tweens.Cubic.EaseIn;
        public static readonly TweeningFunction CubicEaseOut = Tweens.Cubic.EaseOut;
        public static readonly TweeningFunction CubicEaseInOut = Tweens.Cubic.EaseInOut;
        public static readonly TweeningFunction ElasticEaseIn = Tweens.Elastic.EaseIn;
        public static readonly TweeningFunction ElasticcEaseOut = Tweens.Elastic.EaseOut;
        public static readonly TweeningFunction ElasticEaseInOut = Tweens.Elastic.EaseInOut;
        public static readonly TweeningFunction QuarticEaseIn = Tweens.Quartic.EaseIn;
        public static readonly TweeningFunction QuarticEaseOut = Tweens.Quartic.EaseOut;
        public static readonly TweeningFunction QuarticEaseInOut = Tweens.Quartic.EaseInOut;
        public static readonly TweeningFunction QuadraticEaseIn = Tweens.Quadratic.EaseIn;
        public static readonly TweeningFunction QuadraticEaseOut = Tweens.Quadratic.EaseOut;
        public static readonly TweeningFunction QuadraticEaseInOut = Tweens.Quadratic.EaseInOut;
        public static readonly TweeningFunction SineEaseIn = Tweens.Sinusoidal.EaseIn;
        public static readonly TweeningFunction SineEaseOut = Tweens.Sinusoidal.EaseOut;
        public static readonly TweeningFunction SineEaseInOut = Tweens.Sinusoidal.EaseInOut;
        public static readonly TweeningFunction ExponentialEaseIn = Tweens.Exponential.EaseIn;
        public static readonly TweeningFunction ExponentialEaseOut = Tweens.Exponential.EaseOut;
        public static readonly TweeningFunction ExponentialEaseInOut = Tweens.Exponential.EaseInOut;
        #region Constructors
        /// <summary>
        /// Create a Tweener with info on where to move from and to, how long it should take and the function to use.
        /// </summary>
        /// <param name="from">The starting position</param>
        /// <param name="to">The position reached at the end</param>
        /// <param name="duration">How long befor we reach the end?</param>
        /// <param name="tweeningFunction">Which function to use for calculating the current position.</param>
        protected BaseTweener(string propertyName, T from, T to, float duration, TweeningFunction tweeningFunction)
        {
            PropertyName = propertyName;
            this.@from = from;
            _position = from;
            change = CalculateChange(to, from);
            this.tweeningFunction = tweeningFunction;
            _duration = duration;
        }
        protected BaseTweener(string propertyName, T from, T to, float duration)
        {
            PropertyName = propertyName;
            this.@from = from;
            _position = from;
            change = CalculateChange(to, from);
            this.tweeningFunction = Linear;
            _duration = duration;
        }

        protected string PropertyName { get;  set; } 
        /// <summary>
        /// Create a Tweener with info on where to move from and to, how long it should take and the function to use.
        /// </summary>
        /// <param name="from">The starting position</param>
        /// <param name="to">The position reached at the end</param>
        /// <param name="duration">How long befor we reach the end?</param>
        /// <param name="tweeningFunction">Which function to use for calculating the current position.</param>
        protected BaseTweener(string propertyName, T from, T to, TimeSpan duration, TweeningFunction tweeningFunction)
            : this(propertyName,from, to, (float)duration.TotalSeconds, tweeningFunction)
        {
        }
        protected BaseTweener(string propertyName, T from, T to, TimeSpan duration)
            : this(propertyName,from, to, (float)duration.TotalSeconds)
        {
        }
        /// <summary>
        /// Create a stopped tweener with no information on where to move from and to.
        /// Useful in conjunction with the Reset(from, to) call to ready a tweener for later use or lazy
        /// instantiation of a tweener in a property.
        /// </summary>
        /// <param name="duration">The duration of tweening.</param>
        /// <param name="tweeningFunction">Which function to use for calculating the current position.</param>
        protected BaseTweener(TweeningFunction tweeningFunction)
        {
            this.tweeningFunction = tweeningFunction;
            Playing = false;
        }

        /// <summary>
        /// Create a stopped tweener with no information on where to move from and to.
        /// Useful in conjunction with the Reset(from, to) call to ready a tweener for later use or lazy
        /// instantiation of a tweener in a property.
        /// </summary>
        /// <param name="duration">The duration of tweening.</param>
        /// <param name="tweeningFunction">Which function to use for calculating the current position.</param>
        protected BaseTweener(string propertyName, T from, T to, TweeningFunction tweeningFunction, float speed)
        {
            PropertyName = propertyName;
            this.@from = from;
            _position = from;
            change = CalculateChange(to, from);
            this.tweeningFunction = tweeningFunction;
            _duration = CalculateDurationFromSpeed(speed);
        }
        #endregion

        #region Properties
        private T _position;
        /// <summary>
        /// This is the current position of the tweener. It cannot be manipulted directly.
        /// Use the Reset method to alter the behaviour of the tweener.
        /// </summary>
        public T Position
        {
            get
            {
                return _position;
            }
            protected set
            {
                _position = value;
                OnPropertyChanged(PropertyName);
                PositionChanged?.Invoke(_position);
            }
        }

        public bool IsCompleted { get; private set; } = false;
        /// <summary>
        /// This is the positon where the tweener started.
        /// </summary>
        protected T from { get; set; }

        /// <summary>
        /// This is the change to the tweener over its lifetime.
        /// </summary>
        protected T change { get; set; }

        private float _duration;
        /// <summary>
        /// This is the duration of the tweener in seconds.
        /// </summary>
        protected float duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
            }
        }

        /// <summary>
        /// This is the total time that has elapsed since the tweener last started.
        /// </summary>
        protected float elapsed { get; set; } = 0.0f;

        /// <summary>
        /// <para>Is the tweener currently playing.</para> 
        /// <para>If the tweener is not playing, calling Update will not move the tweener.</para>
        /// <para>The tweener can be controlled by calling the Play and Pause methods</para>
        /// </summary>
        public bool Playing { get; protected set; } = true;

        /// <summary>
        /// This is the function that determines the actual movement of the tweener.
        /// </summary>
        protected TweeningFunction tweeningFunction { get; private set; }

        private EasingFunction mEasingFunction;

        public EasingFunction EasingFunction
        {
            get
            {
                return mEasingFunction;
            }
            set
            {
                mEasingFunction = value;
                tweeningFunction = mEasingFunction.Tween;
            }
        }

        public LoopHelper Loop
        {
            get
            {
                return new LoopHelper(this);
            }
        }

        /// <summary>
        /// Event that is called whenever the position of the tweener has changed
        /// </summary>
        public event PositionChangedHandler<T> PositionChanged;

        /// <summary>
        /// Event that is called when the tweener reaches the end. At this point in time the tweener is guaranteed to
        /// to be at the ending position no matter how many times it was stopped and started.
        /// </summary>
        public event EndHandler Ended;

        public event EndExHandler Completed;
        #endregion

        #region Methods
        /// <summary>
        /// Update the position of the tweener using the current game time.
        /// If the position is paused or has finished, no update to the position or the elapsed time will happen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public void Update(TimerTick gameTime)
        {
            if (!Playing || (elapsed == duration))
            {
                return;
            }
            elapsed += (float)gameTime.ElapsedTime.TotalSeconds;
            if (elapsed >= duration)
            {
                elapsed = duration;
                Position = CalculateEndPosition();
                IsCompleted = true;
                Ended?.Invoke();
                Completed?.Invoke(this);
            }
            else
            {
                UpdatePosition(elapsed, from, change, duration);
            }
        }

        /// <summary>
        /// Do the actual update of the position.
        /// Usually we will use the tweening function here.
        /// </summary>
        /// <param name="timeElapsed">The time that has elapsed since the beginning of the tweener.</param>
        /// <param name="start">Where did the tweener start</param>
        /// <param name="change">How much will the tweener move from start to end</param>
        /// <param name="duration">The total duration of tweening.</param>
        protected abstract void UpdatePosition(float elapsed, T from, T change, float duration);

        /// <summary>
        /// Calculate the change value. Usually this is to - from.
        /// </summary>
        /// <param name="to">Where do we want to end</param>
        /// <param name="from">Where we are now</param>
        /// <returns>Returns the new change value</returns>
        protected abstract T CalculateChange(T to, T from);

        /// <summary>
        /// Calculate the position we want to end up in. This is nessecary as to is not saved.
        /// Usually this is from + change
        /// </summary>
        /// <returns>Returns the end position when the tweener is finished.</returns>
        protected abstract T CalculateEndPosition();

        /// <summary>
        /// Calculate the duration of the tween in seconds given the average speed of movement.
        /// Usually this is change / speed
        /// </summary>
        /// <param name="speed">The average movement speed</param>
        /// <returns>The duration of the tweener</returns>
        protected abstract float CalculateDurationFromSpeed(float speed);

        

        /// <summary>
        /// Start the tweener from its current position if it is paused. If it is already playing, nothing happens.
        /// </summary>
        public void Play()
        {
            Playing = true;
        }

        /// <summary>
        /// <para>Pause the tweener if it is playing. If it is already paused, nothing happens.</para>
        /// <para>The tweener can be started again by calling Play</para>
        /// </summary>
        public void Pause()
        {
            Playing = false;
        }

        /// <summary>
        /// <para>Reset the tweener to start again from the beginning.</para>
        /// <para>If the tweener is stopped it will not start, use Restart if that is what you want.</para>
        /// </summary>
        public void Reset()
        {
            elapsed = 0.0f;
            Position = from;
            IsCompleted = false;
        }

        /// <summary>
        /// Reset the tweener to move to a new position from the current position.
        /// Great for extending movement from the current position when something happens.
        /// </summary>
        /// <param name="to">The new position to move to</param>
        public void Reset(T to)
        {
            change = CalculateChange(to, Position);
            from = Position;
            elapsed = 0.0f;
            IsCompleted = false;
        }

        /// <summary>
        /// Reset the tweener with a new set of from and to positons.
        /// </summary>
        /// <param name="to">The new position to move to</param>
        /// <param name="duration">The new duration of the tweener</param>
        public void Reset(T to, TimeSpan duration)
        {
            Reset(to);
            this.duration = (float)duration.TotalSeconds;
        }

        /// <summary>
        /// Reset the tweener with a new set of from and to positons.
        /// </summary>
        /// <param name="to">The new position to move to</param>
        /// <param name="duration">The new average speed of tweener movement</param>
        public void Reset(T to, float speed)
        {
            Reset(to);
            this.duration = CalculateDurationFromSpeed(speed);
        }

        /// <summary>
        /// Reset the tweener with a new set of from and to positons.
        /// </summary>
        /// <param name="to">The new position to move to</param>
        /// <param name="duration">The new duration of the tweener</param>
        public void Reset(T from, T to, TimeSpan duration)
        {
            Position = from;
            Reset(to);
            this.duration = (float)duration.TotalSeconds;
        }

        /// <summary>
        /// Reset the tweener with a new set of from and to positons.
        /// </summary>
        /// <param name="to">The new position to move to</param>
        /// <param name="duration">The new average speed of tweener movement</param>
        public void Reset(T from, T to, float speed)
        {
            Position = from;
            Reset(to);
            this.duration = CalculateDurationFromSpeed(speed);
        }

        /// <summary>
        /// Resets the tweener and starts it playing if it is paused.
        /// </summary>
        public void Restart()
        {
            Reset();
            Play();
        }

        /// <summary>
        /// Reverses movement of the tweener from the current position back to where it came.
        /// This can reverse the tweener before it is done, but be aware that reversing the tweener again
        /// later will not return it to its original destination, but to the point where it was reversed
        /// for the first time.
        /// </summary>
        public void Reverse()
        {
            IsCompleted = false;
            elapsed = 0.0f;
            change = CalculateChange(from, Position);
            from = Position;
        }

        /// <summary>
        /// Gives a textual representation of the tweener.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}.{1}. Tween {2} -> {3} in {4}s. Elapsed {5:##0.##}s",
                tweeningFunction.GetMethodInfo().DeclaringType.Name,
                tweeningFunction.GetMethodInfo().Name,
                from,
                CalculateEndPosition(),
                duration,
                elapsed);
        }
        #endregion

        #region Internal classes
        public struct LoopHelper
        {
            internal LoopHelper(ITweener tweener)
            {
                this.tweener = tweener;
            }

            private ITweener tweener;

            public void FrontToBack()
            {
                Tweens.Loop.FrontToBack(tweener);
            }

            public void FrontToBack(int times)
            {
                Tweens.Loop.FrontToBack(tweener, times);
            }

            public void BackAndForth()
            {
                Tweens.Loop.BackAndForth(tweener);
            }

            public void BackAndForth(int times)
            {
                Tweens.Loop.BackAndForth(tweener, times);
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

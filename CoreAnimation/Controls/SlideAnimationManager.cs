using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreAnimation.Tweens;

namespace CoreAnimation.Controls
{
    public sealed class SlideAnimationManager
    {
        const string ANIM_STARTBASE = "startLLSlideBaseAnimate";
        const string ANIM_STARTSPRING = "startLLSlideSpringAnimate";
        const string ANIM_STARTCIRCLE = "startLLCircleAnimate";
        const string ANIM_ENDBASE = "endLLSlideBaseAnimate";
        const string ANIM_ENDSPRING = "endLLSlideSpringAnimate"; 
        const string ANIM_ENDCIRCLE = "endLLCircleAnimate";
        private SlideLayer mSlideLayer;
        public void Init(SlideLayer slideLayer)
        {
            mSlideLayer = slideLayer;
            mSlideLayer.AnimationDidStop -= OnAnimationDidStop;
            mSlideLayer.AnimationDidStop += OnAnimationDidStop;
        }

        private void OnAnimationDidStop(CAAnimation anim, bool flag)
        {
            if (flag)
            {
                if (anim.ForKey == ANIM_STARTBASE)
                {
                    mSlideLayer.RemoveAllAnimations();
                    mSlideLayer.IsAnimating = true;
                    CAKeyFrameAnimation animSpring =
                        new CAKeyFrameAnimation("distance", 0, mSlideLayer.BackgroundWidth, 2f);
                    Damp damp = new Damp();
                    damp.Damping = 0.5f;
                    damp.Velocity = 3f;
                    animSpring.EasingFunction = damp;
                    animSpring.ForKey = ANIM_STARTSPRING;
                    mSlideLayer.AddAnimation(animSpring);
                    //CAKeyframeAnimation animCircle = [self createBaseAnima: @"radius" duration: .8f fromValue:@(0) toValue:@(sqrt(_circleLayer.frame.size.width * _circleLayer.frame.size.width + _circleLayer.frame.size.height * _circleLayer.frame.size.width))];
                    //[_circleLayer addAnimation:animCircle forKey:ANIM_STARTCIRCLE];
                    return;
                }

                if (anim.ForKey == ANIM_STARTSPRING)
                {
                    mSlideLayer.Distance = mSlideLayer.BackgroundWidth;
                    mSlideLayer.RemoveAllAnimations();
                    return;
                }

                if (anim.ForKey == ANIM_STARTCIRCLE)
                {
                    //_circleLayer.radius = sqrt(_circleLayer.frame.size.width * _circleLayer.frame.size.width + _circleLayer.frame.size.height * _circleLayer.frame.size.width);
                    //    [_circleLayer removeAllAnimations];
                    return;
                }

                if (anim.ForKey == ANIM_ENDBASE)
                {
                    mSlideLayer.RemoveAllAnimations();
                    mSlideLayer.Distance = mSlideLayer.BackgroundWidth;
                    mSlideLayer.IsAnimating = false;
                    CAKeyFrameAnimation animSpring =
                        new CAKeyFrameAnimation("distance", mSlideLayer.BackgroundWidth, 0, 2f);
                    Damp damp = new Damp();
                    damp.Damping = 0.5f;
                    damp.Velocity = 3f;
                    animSpring.EasingFunction = damp;
                    animSpring.ForKey = ANIM_ENDSPRING;
                    mSlideLayer.AddAnimation(animSpring);
                }

                if (anim.ForKey == ANIM_ENDSPRING)
                {
                    mSlideLayer.Distance = 0;
                    mSlideLayer.RemoveAllAnimations();
                }

                if (anim.ForKey == ANIM_ENDCIRCLE)
                {
                    //_circleLayer.radius = 0;
                    //    [_circleLayer removeAllAnimations];
                }
            }
            else
            {
                mSlideLayer.Distance = 0;
                mSlideLayer.RemoveAllAnimations();
            }
        }

        public void StartAnimate()
        {
            if (mSlideLayer.Distance >= mSlideLayer.BackgroundWidth)
            {
                mSlideLayer.IsAnimating = true;
                CAAnimation animation = new CAKeyFrameAnimation("distance", 0, mSlideLayer.BackgroundWidth, 2f);
                Damp damp = new Damp();
                damp.Damping = 0.5f;
                damp.Velocity = 3f;
                animation.EasingFunction=damp;
                animation.ForKey = ANIM_STARTSPRING;
                mSlideLayer.AddAnimation(animation);
            }
            else
            {
                mSlideLayer.IsAnimating = false;
                CAAnimation animation = new CAKeyFrameAnimation("distance", mSlideLayer.Distance, mSlideLayer.BackgroundWidth, 0.2f);
                animation.ForKey = ANIM_STARTBASE;
                mSlideLayer.AddAnimation(animation);
                mSlideLayer.Distance = mSlideLayer.BackgroundWidth;
            }
        }

        public void EndAnimate()
        {
            if (mSlideLayer.Distance >= mSlideLayer.BackgroundWidth)
            {
                // 关闭背景layer
                CAKeyFrameAnimation animBase = new CAKeyFrameAnimation("distance", mSlideLayer.BackgroundWidth,0, 0.3f);
                animBase.ForKey = ANIM_ENDBASE;
                mSlideLayer.AddAnimation(animBase);
                // 关闭contentView
                //CAKeyframeAnimation* animCircle = [self createBaseAnima: @"radius" duration: .3f fromValue:@(_circleLayer.frame.size.width)toValue:@(0)];
                //[_circleLayer addAnimation:animCircle forKey:ANIM_ENDCIRCLE];
            }
            else
            {
                mSlideLayer.IsAnimating = false;
                CAKeyFrameAnimation animSpring = new CAKeyFrameAnimation("distance", mSlideLayer.Distance, 0, 1.5f);
                Damp damp = new Damp();
                damp.Damping = 0.5f;
                damp.Velocity = 3f;
                animSpring.ForKey = ANIM_ENDSPRING;
                animSpring.EasingFunction = damp;
                mSlideLayer.AddAnimation(animSpring); 
            }
        }
    }
}

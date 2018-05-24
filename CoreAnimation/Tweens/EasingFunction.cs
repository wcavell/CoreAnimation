using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAnimation.Tweens
{
    public abstract class EasingFunction
    {
        #region EasingMode
        public EasingModes EasingMode
        {
            get;
            set;
        }
        #endregion

        public EasingFunction()
        {
            this.EasingMode = EasingModes.EaseIn;
        }
         
        public virtual float Tween(float timeElapsed, float start, float change, float duration)
        {
            throw new NotImplementedException();
        } 
    }
}

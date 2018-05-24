using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAnimation.Tweens
{
    public static class Loop
    {
        #region Static methods
        public static void FrontToBack(ITweener tweener)
        {
            tweener.Ended -= tweener.Restart;
            tweener.Ended += tweener.Restart;
        }

        public static void FrontToBack(ITweener tweener, int times)
        {
            TimesLoopingHelper helper = new TimesLoopingHelper(tweener, times);
            tweener.Ended -= helper.FrontToBack;
            tweener.Ended += helper.FrontToBack;
        }

        public static void BackAndForth(ITweener tweener)
        {
            tweener.Ended -= tweener.Reverse;
            tweener.Ended += tweener.Reverse;
        }

        public static void BackAndForth(ITweener tweener, int times)
        {
            TimesLoopingHelper helper = new TimesLoopingHelper(tweener, times);
            tweener.Ended -= helper.BackAndForth;
            tweener.Ended += helper.BackAndForth;
        }
        #endregion

        #region Internal classes
        private struct TimesLoopingHelper
        {
            public TimesLoopingHelper(ITweener tweener, int times)
            {
                this.tweener = tweener;
                this.times = times;
            }

            private int times;
            private ITweener tweener;

            private bool Stop()
            {
                return --times == 0;
            }

            public void FrontToBack()
            {
                if (Stop())
                {
                    tweener.Ended -= FrontToBack;
                }
                else
                {
                    tweener.Reset();
                }
            }

            public void BackAndForth()
            {
                if (Stop())
                {
                    tweener.Ended -= BackAndForth;
                }
                else
                {
                    tweener.Reverse();
                }
            }
        }
        #endregion
    }
}

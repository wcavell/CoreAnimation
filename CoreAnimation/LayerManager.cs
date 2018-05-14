using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;

namespace CoreAnimation
{
    public static class LayerManager
    {
        public static bool IsRuning { get; private set; } = false;
        public static bool IsPause { get; private set; } = false;
        private static long Id;
        public static void Init()
        {
            Id = 0;
        }
        public static  void Start()
        {
            var task = ThreadPool.RunAsync(OnLoop, WorkItemPriority.Normal);
        }

        public static void Pause()
        {
            IsPause = true;
        }

        public static void Resume()
        {
            Id = 0;
            IsPause = false;
        }

        private static void OnLoop(object state)
        {
            while (true)
            {
                IsRuning = true;
                Id++;
                if (IsPause)
                {

                }
                else
                {
                    
                }
            }
            IsRuning = false;
        }
    }
}

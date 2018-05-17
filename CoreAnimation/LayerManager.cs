using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System.Threading;

namespace CoreAnimation
{
    public static class LayerManager
    {
        public static bool IsRuning { get; private set; } = false;
        public static bool IsPause { get; private set; } = false;
        private static volatile int Id;
        private static int _lock;
        private static IAsyncAction TaskAsync;
        public static TimerTick Timer = new TimerTick();
        public static void Init()
        {
            Id = 0;
        }
        public static  void Start()
        {
            TaskAsync = ThreadPool.RunAsync(OnLoop, WorkItemPriority.Normal);
            Timer.Reset();
        }

        public static void Pause()
        {
            IsPause = true;
            Timer.Pause();
        }

        public static void CancelRun()
        {
            TaskAsync?.Cancel();
        }
        public static async void Resume()
        {
            Id = 0;
            IsPause = false;
            Timer.Resume();
            await Task.Delay(1000);
            if (Id == 0)
            {
                Start();
            }
        }

        private static async void OnLoop(IAsyncAction state)
        {
            while (true)
            {
                IsRuning = true;
                Id++;
                if(state.Status==AsyncStatus.Canceled)
                    break;
                Timer.Tick();
                if (IsPause)
                {
                    await Task.Delay(250);
                }
                else
                {
                    Update();
                }
            }
            IsRuning = false;
        }

        private static void CmdProc()
        {
            while (true)
            {
                if (0 == Interlocked.Exchange(ref _lock, 1))
                {
                    //layerPanels.Add(layerPanel);
                    Interlocked.Exchange(ref _lock, 0);
                    break;
                }
            }
        }

        private static  void Update()
        {

        }
    }
    
}

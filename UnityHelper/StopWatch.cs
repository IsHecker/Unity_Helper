using System.Threading.Tasks;
using UnityEngine;

namespace UnityHelper
{
    public class StopWatch
    {
        public float CurrentTime { get; private set; }
        public bool IsRunning { get; private set; }

        private StopWatch(bool start)
        {
            CurrentTime = 0.0f;
            IsRunning = start;
        }


        /// <summary>
        /// Starts a new StopWatch.
        /// </summary>
        /// <returns></returns>
        public static StopWatch Start()
        {
            var stopWatch = new StopWatch(true);
            stopWatch.StartStopWatch();
            return stopWatch;
        }

        /// <summary>
        /// Reset StopWatch time.
        /// </summary>
        public void Reset() => CurrentTime = 0;

        /// <summary>
        /// Resumes the StopWatch.
        /// </summary>
        public void Resume()
        {
            IsRunning = true;
            StartStopWatch();
        }

        /// <summary>
        /// Pauses the StopWatch.
        /// </summary>
        public void Pause() => IsRunning = false;

        private async void StartStopWatch()
        {
            while (IsRunning)
            {
                CurrentTime += Time.deltaTime;
                await Task.Yield();
            }
        }
    }
}

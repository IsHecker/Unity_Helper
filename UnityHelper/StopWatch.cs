using System.Threading.Tasks;
using UnityEngine;

namespace UnityHelper
{
    public class StopWatch
    {
        public float time;
        private bool stop;

        private StopWatch(bool stop)
        {
            time = 0.0f;
            this.stop = stop;

            Start();
        }


        /// <summary>
        /// Starts a new StopWatch.
        /// </summary>
        /// <returns></returns>
        public static StopWatch StartStopWatch()
        {
            return new StopWatch(false);
        }

        public float CurrentTime => time;

        /// <summary>
        /// Stops the StopWatch
        /// </summary>
        public void Stop()
        {
            stop = true;
        }

        private async void Start()
        {
            while (!stop)
            {
                time += Time.deltaTime;
                await Task.Yield();
            }
        }
    }
}

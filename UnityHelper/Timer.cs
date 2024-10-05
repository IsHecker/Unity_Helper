using UnityEngine;

namespace UnityHelper
{
    /// <summary>
    /// A class representing a Timer with various functionalities such as start, stop.
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// The initial time set for the countdown.
        /// </summary>
        private float timerValue;

        private float startTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountdownTimer"/> class with a specified initial time and an option to auto tick.
        /// </summary>
        /// <param name="timerValue">The initial time value for the countdown.</param>
        public Timer(float timerValue = 0f)
        {
            SetTimer(timerValue);
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            startTime = Time.time;
        }

        /// <summary>
        /// Checks if the Timer has Finished since it Started.
        /// </summary>
        /// <returns>False if the Timer didn't start yet or the Timer has actually Finished.</returns>
        public bool IsFinsished()
        {
            return Time.time - startTime > timerValue;
        }

        /// <summary>
        /// Stops the Timer.
        /// </summary>
        public void Stop()
        {
            startTime = float.MaxValue;
        }

        /// <summary>
        /// Sets the Timer to new value.
        /// </summary>
        public void SetTimer(float timerValue)
        {
            if (timerValue <= 0f)
                return;

            Stop();
            this.timerValue = timerValue;
        }
    }
}
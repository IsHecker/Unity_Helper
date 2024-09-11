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
        private readonly float timerValue;

        /// <summary>
        /// Gets or sets the current time of the countdown.
        /// </summary>
        private float CurrentTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountdownTimer"/> class with a specified initial time and an option to auto tick.
        /// </summary>
        /// <param name="timerValue">The initial time value for the countdown.</param>
        public Timer(float timerValue)
        {
            this.timerValue = timerValue;
            CurrentTime = float.MaxValue;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            CurrentTime = Time.time;
        }

        /// <summary>
        /// Checks if the Timer has Finished since it Started.
        /// </summary>
        /// <returns>False if the Timer didn't start yet or the Timer has actually Finished.</returns>
        public bool IsFinsished()
        {
            return Time.time - CurrentTime > timerValue;
        }

        /// <summary>
        /// Reset the Timer.
        /// </summary>
        public void Stop()
        {
            CurrentTime = float.MaxValue;
        }
    }
}
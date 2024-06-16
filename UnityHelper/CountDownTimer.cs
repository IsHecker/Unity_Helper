using System.Security.Principal;
using System.Threading.Tasks;
using UnityHelper.Extensions;

namespace UnityHelper
{
    /// <summary>
    /// A class representing a countdown timer with various functionalities such as start, stop, pause, resume, and reset.
    /// </summary>
    public class CountdownTimer
    {
        /// <summary>
        /// The initial time set for the countdown.
        /// </summary>
        private float initialTime;

        /// <summary>
        /// The time at which the timer was paused.
        /// </summary>
        private float pauseTime;

        /// <summary>
        /// Indicates whether the timer should automatically tick.
        /// </summary>
        private readonly bool autoTick;

        /// <summary>
        /// Gets or sets the current time of the countdown.
        /// </summary>
        public float CurrentTime { get; set; }

        /// <summary>
        /// Gets a value indicating whether the timer is currently running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Occurs when the timer starts.
        /// </summary>
        public event System.Action OnTimerStart = () => { };

        /// <summary>
        /// Occurs when the timer stops.
        /// </summary>
        public event System.Action OnTimerStop = () => { };

        /// <summary>
        /// Occurs when the timer finishes.
        /// </summary>
        public event System.Action OnTimerFinish = () => { };

        /// <summary>
        /// Initializes a new instance of the <see cref="CountdownTimer"/> class with a specified initial time and an option to auto tick.
        /// </summary>
        /// <param name="value">The initial time value for the countdown.</param>
        /// <param name="autoTick">Whether the timer should automatically tick. Default is true.</param>
        public CountdownTimer(float value, bool autoTick = true)
        {
            initialTime = value;
            this.autoTick = autoTick;
            CurrentTime = initialTime;
            IsRunning = false;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            if (IsRunning)
                return;

            CurrentTime = initialTime;
            IsRunning = true;
            OnTimerStart.Invoke();

            if (autoTick)
                _ = AutoTick();
        }

        /// <summary>
        /// Handles the automatic ticking of the timer.
        /// </summary>
        private async Task AutoTick()
        {
            while (Tick(UnityEngine.Time.deltaTime))
            {
                await Task.Yield();
            }
            Finish();
        }

        /// <summary>
        /// Finishes the timer and triggers the finish event.
        /// </summary>
        private void Finish()
        {
            if (IsRunning)
            {
                IsRunning = false;
                OnTimerFinish.Invoke();
            }
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                OnTimerStop.Invoke();
            }
        }

        /// <summary>
        /// Ticks the timer by a specified delta time.
        /// </summary>
        /// <param name="deltaTime">The time to decrease the current time by.</param>
        /// <returns>True if the timer is still running; otherwise, false.</returns>
        public bool Tick(float deltaTime)
        {
            if (!IsRunning)
                return false;

            if (!IsFinished)
            {
                CurrentTime -= deltaTime;
                return true;
            }

            if (!autoTick)
                Finish();
            return false;
        }

        /// <summary>
        /// Resumes the timer from where it was paused.
        /// </summary>
        public void Resume()
        {
            if (IsRunning)
                return;

            IsRunning = true;
            Reset(pauseTime);
        }

        /// <summary>
        /// Pauses the timer.
        /// </summary>
        public void Pause()
        {
            if (!IsRunning)
                return;

            IsRunning = false;
            pauseTime = CurrentTime;
        }

        /// <summary>
        /// Gets a value indicating whether the timer has finished.
        /// </summary>
        public bool IsFinished => CurrentTime < 0;

        /// <summary>
        /// Resets the timer to the initial time.
        /// </summary>
        public void Reset() => CurrentTime = initialTime;

        /// <summary>
        /// Resets the timer to a new specified time.
        /// </summary>
        /// <param name="newTime">The new time value to reset the timer to.</param>
        public void Reset(float newTime)
        {
            initialTime = newTime;
            Reset();
        }
    }

}

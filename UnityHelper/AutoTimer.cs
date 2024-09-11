using System.Threading;
using System.Threading.Tasks;

namespace UnityHelper
{
    /// <summary>
    /// A class representing an Automatic Countdown Timer with various functionalities such as start, stop, pause, resume, and reset.
    /// </summary>
    public class AutoTimer
    {
        /// <summary>
        /// The initial time set for the countdown.
        /// </summary>
        private readonly float timerValue;

        /// <summary>
        /// The time at which the Timer was paused.
        /// </summary>
        private float pauseTime;

        private readonly CancellationTokenSource cancellationToken;

        /// <summary>
        /// Gets or sets the current time of the countdown.
        /// </summary>
        public float CurrentTime { get; set; }

        /// <summary>
        /// Gets a value indicating whether the Timer is currently running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Occurs when the Timer starts.
        /// </summary>
        public event System.Action OnTimerStart = () => { };

        /// <summary>
        /// Occurs when the Timer stops.
        /// </summary>
        public event System.Action OnTimerStop = () => { };

        /// <summary>
        /// Occurs when the Timer finishes.
        /// </summary>
        public event System.Action OnTimerFinish = () => { };

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoTimer"/> class with a specified initial time and an option to auto tick.
        /// </summary>
        /// <param name="value">The initial time value for the countdown.</param>
        /// <param name="autoTick">Whether the Timer should automatically tick. Default is true.</param>
        public AutoTimer(float value)
        {
            timerValue = value;
            CurrentTime = timerValue;
            IsRunning = false;
            cancellationToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Starts the Timer.
        /// </summary>
        public void Start()
        {
            Reset();
            OnTimerStart.Invoke();

            if (IsRunning)
                return;

            IsRunning = true;
            _ = AutoTick();
        }

        /// <summary>
        /// Handles the automatic ticking of the Timer.
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
        /// Finishes the Timer and triggers the finish event.
        /// </summary>
        private void Finish()
        {
            if (!IsRunning)
                return;

            IsRunning = false;
            OnTimerFinish.Invoke();
        }

        /// <summary>
        /// Stops the Timer.
        /// </summary>
        public void Stop()
        {
            if (!IsRunning)
                return;

            IsRunning = false;
            OnTimerStop.Invoke();
        }

        /// <summary>
        /// Ticks the timer by a specified delta time.
        /// </summary>
        /// <param name="deltaTime">The time to decrease the current time by.</param>
        /// <returns>True if the Timer is still running; otherwise, false.</returns>
        private bool Tick(float deltaTime)
        {
            if (!IsRunning || IsFinished)
                return false;

            CurrentTime -= deltaTime;
            return true;
        }

        /// <summary>
        /// Resumes the timer from where it was paused.
        /// </summary>
        public void Resume()
        {
            if (IsRunning)
                return;

            IsRunning = true;
            CurrentTime = pauseTime;
        }

        /// <summary>
        /// Pauses the Timer.
        /// </summary>
        public void Pause()
        {
            if (!IsRunning)
                return;

            IsRunning = false;
            pauseTime = CurrentTime;
        }

        /// <summary>
        /// Gets a value indicating whether the Timer has finished.
        /// </summary>
        public bool IsFinished => CurrentTime < 0;

        /// <summary>
        /// Resets the Timer to the initial time.
        /// </summary>
        public void Reset() => CurrentTime = timerValue;
    }
}
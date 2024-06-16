using JetBrains.Annotations;
using System;

namespace UnityHelper.Events
{
    /// <summary>
    /// A static event bus class to handle events of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the event data, which must be a struct and implement the <see cref="IEvent"/> interface.</typeparam>
    public static class EventBus<T> where T : struct, IEvent
    {
        private static Action<T> onEvent = (_) => { };
        private static Action onEventNoArgs = () => { };

        /// <summary>
        /// Occurs when an event of type <typeparamref name="T"/> is raised.
        /// </summary>
        public static event Action<T> OnEvent
        {
            add
            {
                lock (onEvent)
                {
                    var handlers = onEvent;
                    handlers += value;
                    onEvent = handlers;
                }
            }
            remove
            {
                lock (onEvent)
                {
                    var handlers = onEvent;
                    handlers -= value;
                    onEvent = handlers;
                }
            }
        }

        /// <summary>
        /// Occurs when any event is raised, without arguments.
        /// </summary>
        public static event Action OnEventNoArgs
        {
            add
            {
                lock (onEventNoArgs)
                {
                    var handlers = onEventNoArgs;
                    handlers += value;
                    onEventNoArgs = handlers;
                }
            }
            remove
            {
                lock (onEventNoArgs)
                {
                    var handlers = onEventNoArgs;
                    handlers -= value;
                    onEventNoArgs = handlers;
                }
            }
        }

        /// <summary>
        /// Raises an event of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="evt">The event data. Can be <c>null</c>.</param>
        public static void Raise([CanBeNull] T evt)
        {
            Action<T> eventHandlers;
            Action noArgsEventHandlers;

            // Lock to capture the current subscribers to ensure thread safety
            lock (onEvent)
            {
                eventHandlers = onEvent;
            }
            lock (onEventNoArgs)
            {
                noArgsEventHandlers = onEventNoArgs;
            }

            // Invoke the captured event handlers outside the lock to avoid deadlocks
            eventHandlers?.Invoke(evt);
            noArgsEventHandlers?.Invoke();
        }
    }
}

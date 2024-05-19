using System;
using System.Collections.Generic;
namespace UnityHelper
{
    public class EventManager
    {
        // Singleton instance
        private static EventManager instance;

        // Dictionary to store a single delegate for each delegate type
        private readonly Dictionary<string, Delegate> eventListeners = new Dictionary<string, Delegate>();

        // Public access to the singleton instance
        public static EventManager Instance
        {
            get
            {
                instance ??= new EventManager();
                return instance;
            }
        }

        public void AddEvent(string eventName, Delegate _event)
        {
            eventListeners.TryAdd(eventName, _event);
        }

        public void FireEvent(string eventName, params object[] args)
        {
            if (eventListeners.TryGetValue(eventName, out Delegate value))
                value.DynamicInvoke(args);
        }

        // Subscribe to an event
        public void Subscribe(string eventName, Delegate action)
        {
            if (!eventListeners.TryGetValue(eventName, out Delegate value))
            {
                eventListeners[eventName] = action;
                return;
            }
            eventListeners[eventName] = Delegate.Combine(value, action);
        }

        // Unsubscribe from an event
        public void Unsubscribe(string eventName, Delegate action)
        {
            if (eventListeners.TryGetValue(eventName, out Delegate value))
            {
                eventListeners[eventName] = Delegate.Remove(value, action);
            }
        }
    }
}

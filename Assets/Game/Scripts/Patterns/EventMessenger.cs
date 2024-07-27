using System;
using System.Collections.Generic;

namespace BlueGravity.Interview.Patterns
{
    /// <summary>
    /// Basic implementation of EventMessenger pattern.<br/>
    /// Serves as a global static messenger that components can subscribe to and listen to events.<br/>
    /// Events should all inherit from <see cref="GameEvent"/> class.
    /// </summary>
    public class EventMessenger
    {
        static EventMessenger _instance;

        public static EventMessenger Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EventMessenger();

                return _instance;
            }
        }

        public delegate void EventDelegate<T>(T eventData) where T : GameEvent;
        readonly Dictionary<Type, Delegate> _delegates = new Dictionary<Type, Delegate>();

        public void AddListener<T>(EventDelegate<T> listener) where T : GameEvent
        {
            Delegate del;
            if (_delegates.TryGetValue(typeof(T), out del))
                _delegates[typeof(T)] = Delegate.Combine(del, listener);
            else
                _delegates[typeof(T)] = listener;
        }

        public void RemoveListener<T>(EventDelegate<T> listener) where T : GameEvent
        {
            Delegate del;
            if (_delegates.TryGetValue(typeof(T), out del))
            {
                Delegate currentDel = Delegate.Remove(del, listener);

                if (currentDel == null)
                    _delegates.Remove(typeof(T));
                else
                    _delegates[typeof(T)] = currentDel;
            }
        }

        public void Raise<T>(T eventData) where T : GameEvent
        {
            if (eventData == null)
                throw new ArgumentNullException("eventData");
            
            if (eventData.ShowDebug)
                UnityEngine.Debug.Log($"EventMessenger->Fired {eventData.GetDebugText()}");

            Delegate del;
            if (_delegates.TryGetValue(typeof(T), out del))
            {
                EventDelegate<T> callback = del as EventDelegate<T>;

                if (callback != null)
                    callback(eventData);
            }
        }
    }
}
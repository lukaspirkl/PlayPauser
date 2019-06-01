using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayPauser
{
    public class EventAggregator
    {
        private Dictionary<Type, List<object>> subscriptions = new Dictionary<Type, List<object>>();

        public void Publish<T>(T message)
        {
            var type = typeof(T);
            if (subscriptions.ContainsKey(type))
            {
                foreach (var action in subscriptions[type].OfType<Action<T>>())
                {
                    action(message);
                }
            }
        }

        public void Subscribe<T>(Action<T> action)
        {
            var type = typeof(T);
            if (subscriptions.ContainsKey(type))
            {
                subscriptions[type].Add(action);
            }
            else
            {
                subscriptions.Add(type, new List<object>() { action });
            }
        }

        public void Unsubscribe<T>(Action<T> action)
        {
            var type = typeof(T);
            if (subscriptions.ContainsKey(type))
            {
                subscriptions[type].Remove(action);
            }
        }
    }
}

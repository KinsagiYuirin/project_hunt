using System;
using System.Collections.Generic;

namespace MadDuck.Scripts.Utils
{
    public static class EventBus<T> where T : struct
    {
        public static event Action<T> Event;

        /// <summary>
        /// Invokes the event with the specified event data. Note: It is recommended to invoke from the static method of the event data.
        /// </summary>
        /// <param name="eventData">Event data to invoke.</param>
        /// <typeparam name="T">Type of the event data.</typeparam>
        public static void Invoke(T eventData)
        {
            Event?.Invoke(eventData);
        }
    }
}

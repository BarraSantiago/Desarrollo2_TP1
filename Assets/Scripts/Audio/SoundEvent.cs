using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    /// <summary>
    /// The SoundEvent class is a ScriptableObject that manages a list of SoundEventListener objects. 
    /// It provides methods to raise events, register listeners, and unregister listeners.
    /// </summary>
    [CreateAssetMenu(fileName = "SoundEvent")]
    public class SoundEvent : ScriptableObject
    {
        public List<SoundEventListener> listeners;

        /// <summary>
        /// Raises an event for all registered SoundEventListener objects.
        /// </summary>
        public void Raise()
        {
            foreach (SoundEventListener listener in listeners)
            {
                listener.OnEventRaised(this);
            }
        }
        
        /// <summary>
        /// Register listener to event
        /// </summary>
        /// <param name="listener"> SoundEventListener to register </param>
        public void RegisterListener(SoundEventListener listener)
        {
            if(!listeners.Contains(listener)) listeners.Add(listener);
        }
        
        /// <summary>
        /// Unregister listener to event
        /// </summary>
        /// <param name="listener"> SoundEventListener to unregister </param>
        public void UnregisterListener(SoundEventListener listener)
        {
            if(listeners.Contains(listener)) listeners.Remove(listener);
        }
    }
}
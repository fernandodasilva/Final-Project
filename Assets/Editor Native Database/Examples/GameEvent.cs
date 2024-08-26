using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ENDB
{
    public class GameEvent : DatabaseEntry
    {
        //List of all objects subscribed to this GameEvent
        // [SerializeField]
        private List<GameEventListener> listeners = new List<GameEventListener>();

        [TextArea]
        [Tooltip("When this event is raised")]
        public string eventDescription = "";

        public void Raise()
        {
#if UNITY_EDITOR
            Debug.Log($"{this.name} event raised");
#endif
            //Loop through the listener list and run their responses
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }

        //Add the listener to the listener list
        public void RegisterListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        //Remove the listener from the listener list
        public void UnregisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}

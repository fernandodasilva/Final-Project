using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ENDB
{
    
    /// <summary>
    /// Attach this script to object that should listen to an event 
    /// </summary>
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent eventToListen; //event to listen to
        public UnityEvent response; //Reponse to the event 

        [TextArea]
        [Tooltip("What this object does when the event is raised")]
        public string responseDescription = "";

        private void OnEnable()
        {
            // register this listener
            if (eventToListen != null)
            {
                eventToListen.RegisterListener(this);
            }
        }

        private void OnDisable()
        { // also fires when gameObject is destroyed
            //unregister if gameObject is disabled
            if (eventToListen != null)
            {
                eventToListen.UnregisterListener(this);
            }
        }

        /// <summary>
        /// respond to event
        /// </summary>
        public void OnEventRaised()
        {
            response.Invoke();
        }
    }
}

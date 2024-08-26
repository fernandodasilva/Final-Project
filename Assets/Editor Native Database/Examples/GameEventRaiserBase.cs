using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ENDB
{
    /// <summary>
    /// Event raiser Base class. Every event raiser script must inherit from this. GetEventRaisers() uses this class to find objects that raise events.
    /// </summary>
    public abstract class GameEventRaiserBase : MonoBehaviour // absract so that it cant be attached to game object
    {
        public GameEvent eventToRaise = null; 

        public void RaiseEvent()
        {
            if (eventToRaise != null)
                eventToRaise.Raise(); 
        }


    }
}

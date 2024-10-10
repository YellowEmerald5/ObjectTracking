using ObjectTracking.GameEventScripts;
using UnityEngine;
using UnityEngine.Events;

namespace SetUpScripts
{
    public static class GameEventListenerSetup
    {
        /// <summary>
        /// Sets up the game event listener on an object
        /// </summary>
        /// <param name="obj">GameObject the listener should be added to</param>
        /// <param name="method">Method the listener should activate</param>
        /// <param name="ev">Event the listener should listen for</param>
        public static void SetUpEventListener(GameObject obj,UnityAction method, GameEvent ev)
        {
            var listener = obj.AddComponent<GameEventListener>();
            
            listener.gameEvent = ev;
            listener.RegisterListener();
            var reaction =  new UnityEvent();
            reaction.AddListener(method);
            listener.response = reaction;
        }
    }
}
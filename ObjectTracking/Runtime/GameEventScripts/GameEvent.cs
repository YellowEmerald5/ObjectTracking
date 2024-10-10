using System.Collections.Generic;
using UnityEngine;

namespace ObjectTracking.GameEventScripts
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "Event/gameEvent", order = 0)]
    public class GameEvent : ScriptableObject
    {

        private List<GameEventListener> _listeners = new List<GameEventListener>();

        /// <summary>
        /// Registers a game event listener to the game event
        /// </summary>
        /// <param name="listener">Game event listener listening to the given event</param>
        public void RegisterListener(GameEventListener listener)
        {
            _listeners.Add(listener);
        }
        
        /// <summary>
        /// Unregisters a game event listener from the game event
        /// </summary>
        /// <param name="listener">Game event listener which should stop listening to the game event</param>
        public void UnRegisterListener(GameEventListener listener)
        {
            _listeners.Remove(listener);
        }
        
        /// <summary>
        /// Raises the game event which causes registered listeners to react
        /// </summary>
        public void Raise()
        {
            foreach (var listener in _listeners)
            {
                listener.OnEventRaised();
            }
        }
    }
}
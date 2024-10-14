using ObjectTracking.GameEventScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ObjectTracking.GameEventScripts
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent gameEvent;
        public UnityEvent response;
        
        //Registers itself as a listener to the game event. This will not work if the game event is assigned in script
        private void OnEnable()
        {
            if(gameEvent == null) return;
            RegisterListener();
        }

        //Unregisters itself as a listener from the game event
        private void OnDisable()
        {
            gameEvent.UnRegisterListener(this);
        }
        
        /// <summary>
        /// Registers this game event listener as a listener on the game event
        /// </summary>
        public void RegisterListener()
        {
            gameEvent.RegisterListener(this);
        }

        /// <summary>
        /// Performs the given response when the game event is raised
        /// </summary>
        public void OnEventRaised()
        {
            response?.Invoke();
        }
    }
}
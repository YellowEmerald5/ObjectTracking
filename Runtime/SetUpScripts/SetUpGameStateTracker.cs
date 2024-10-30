using BehaviourScripts;
using ObjectTracking.GameEventScripts;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.Events;

namespace SetUpScripts
{
    public class SetUpGameStateTracker : MonoBehaviour
    {
        private GameStateTracker gameStateTracker;
        private RequiredScriptableObjectsStorage gameEvents;
        
        /// <summary>
        /// Sets up the GameStateTracker script and necessary events
        /// </summary>
        private void Start()
        {
            gameEvents = GetComponent<RequiredScriptableObjectsStorageScript>().requiredScriptables;
            gameStateTracker = gameObject.AddComponent<GameStateTracker>();
            gameStateTracker.scriptableObjects = gameEvents;
        }
    }
}
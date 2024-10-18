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
            PrepareGameStateTracker(gameEvents.storage);
        }

        /// <summary>
        /// Sets up the GameStateTracker script
        /// </summary>
        /// <param name="storageSO">Scriptable object containing information for the current session</param>
        /// <param name="completedWritingSO">Game event used to inform parts of the application that it can safely end</param>
        private void PrepareGameStateTracker(StorageSO storageSO)
        {
            gameStateTracker = gameObject.AddComponent<GameStateTracker>();
            gameStateTracker.storage = storageSO;
        }
    }
}
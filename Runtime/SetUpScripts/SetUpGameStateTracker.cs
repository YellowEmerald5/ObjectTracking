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
        
        //Sets up the game event listeners for the game state tracker object and adds the GameStateTracker script
        //The game event listeners are not set up in a separate method due to issues caused in such cases
        private void Start()
        {
            gameEvents = GetComponent<RequiredScriptableObjectsStorageScript>().requiredScriptables;
            PrepareGameStateTracker(gameEvents.storage, gameEvents.completedWriting);
            
            GameEventListenerSetup.SetUpEventListener(gameObject,gameStateTracker.ObjectCreated,gameEvents.objectCreated);
            GameEventListenerSetup.SetUpEventListener(gameObject,gameStateTracker.WaitForDataCollection,gameEvents.addedToList);
        }

        /// <summary>
        /// Sets up the GameStateTracker script
        /// </summary>
        /// <param name="storageSO">Scriptable object containing information for the current session</param>
        /// <param name="completedWritingSO">Game event used to inform parts of the application that it can safely end</param>
        private void PrepareGameStateTracker(StorageSO storageSO, GameEvent completedWritingSO)
        {
            gameStateTracker = gameObject.AddComponent<GameStateTracker>();
            gameStateTracker.storage = storageSO;
            var ev = new UnityEvent();
            ev.AddListener(completedWritingSO.Raise);
            gameStateTracker.completedWritingObjectsToStorage = ev;
        }
    }
}
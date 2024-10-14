using BehaviourScripts;
using ObjectTracking.GameEventScripts;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.Events;

namespace SetUpScripts
{
    public class SetUpObjectTracking : MonoBehaviour
    {
        private ObjectTrackerScript TrackerScript;
        private RequiredScriptableObjectsStorage gameEvents;

        //Finds the scriptable objects necessary for setting up the objectTracker script and then
        //adds a tracker script and game listener to the object
        private void Start()
        {
            gameEvents = FindObjectOfType<RequiredScriptableObjectsStorageScript>().requiredScriptables;
            TrackerScript = gameObject.AddComponent<ObjectTrackerScript>();
            TrackerSetup(gameEvents.storage,gameEvents.objectCreated,gameEvents.addedToList);
        }

        /// <summary>
        /// Sets up the ObjectTrackerScript for current object
        /// </summary>
        /// <param name="storageSO">Scriptable object containing nickname,
        /// session count and a list of the currently played games and the objects in each game</param>
        /// <param name="objectCreatedSo">Game event used to inform the GameStateTracker script of the creation of a tracked object</param>
        /// <param name="addedToListSO">Game event used to inform the GameStateTracker when an object is added to the StorageSO for saving</param>
        private void TrackerSetup(StorageSO storageSO,GameEvent objectCreatedSo,GameEvent addedToListSO)
        {
            TrackerScript.storage = storageSO;
            var createdEv = new UnityEvent();
            var addedEv = new UnityEvent();
            createdEv.AddListener(objectCreatedSo.Raise);
            addedEv.AddListener(addedToListSO.Raise);
            TrackerScript.objectCreatedEvent = createdEv;
            TrackerScript.objectAddedToListEvent = addedEv;
        }
    }
}
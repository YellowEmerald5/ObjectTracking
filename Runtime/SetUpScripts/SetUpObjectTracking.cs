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

        /// <summary>
        /// Sets up a ObjectTracker script for the object it is attached to
        /// </summary>
        private void Start()
        {
            gameEvents = FindObjectOfType<RequiredScriptableObjectsStorageScript>().requiredScriptables;
            TrackerScript = gameObject.AddComponent<ObjectTrackerScript>();
            TrackerSetup(gameEvents.storage,gameEvents.objectCreated,gameEvents.addedToList);
        }

        /// <summary>
        /// Sets up the ObjectTrackerScript for current object
        /// </summary>
        /// <param name="storageSO">Scriptable object containing necessary data</param>
        /// <param name="objectCreatedSo">Game event for informing of the objects creation</param>
        /// <param name="addedToListSO">Game event for starting the save sequence</param>
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
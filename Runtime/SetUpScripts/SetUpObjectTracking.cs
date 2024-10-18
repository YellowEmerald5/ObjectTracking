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
            TrackerScript.storage = gameEvents.storage;
        }
    }
}
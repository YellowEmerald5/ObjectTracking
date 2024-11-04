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
        [SerializeField] public bool TrackChildObjects;

        /// <summary>
        /// Sets up a ObjectTracker script for the object it is attached to
        /// </summary>
        private void Start()
        {
            gameEvents = FindObjectOfType<RequiredScriptableObjectsStorageScript>().requiredScriptables;
            if (TrackChildObjects)
            {
                var childObjects = gameObject.GetComponentsInChildren<Transform>();
                print(childObjects.Length);
            }
            else
            {
                TrackerScript = gameObject.AddComponent<ObjectTrackerScript>();
                TrackerScript.storage = gameEvents.storage;
            }
        }
    }
}
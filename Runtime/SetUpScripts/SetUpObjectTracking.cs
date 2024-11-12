using BehaviourScripts;
using ObjectTracking.GameEventScripts;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SetUpScripts
{
    public class SetUpObjectTracking : MonoBehaviour
    {
        private RequiredScriptableObjectsStorage gameEvents;
        [Tooltip("Tracks child objects with no children")]
        [SerializeField] public bool TrackChildObjects;
        [Tooltip("Tracks all objects under the attached object")]
        [SerializeField] public bool TrackAllObjects;

        /// <summary>
        /// Sets up a ObjectTracker script for the object it is attached to
        /// </summary>
        private void Start()
        {
            gameEvents = FindObjectOfType<RequiredScriptableObjectsStorageScript>().requiredScriptables;
            if (TrackChildObjects || TrackAllObjects)
            {
                if (TrackAllObjects && !TrackChildObjects)
                {
                    TrackChildObjects = true;
                }
                SetUpMultiple(gameObject);
            }
            else
            {
                SetUpObject(gameObject);
            }
        }

        private void SetUpMultiple(GameObject obj)
        {
            var childObjects = obj.transform.GetComponentsInChildren<Transform>();
            if (TrackAllObjects)
            {
                foreach (var childObj in childObjects)
                {
                    SetUpObject(childObj.gameObject);
                }
            }
            else
            {
                for (var i = 1; i < childObjects.Length; i++)
                {
                    var childObj = childObjects[i].gameObject;
                    var children = childObj.transform.GetComponentsInChildren<Transform>();
                    if (children.Length == 1)
                    {
                        SetUpObject(childObj); 
                    }
                } 
            }
        }

        private void SetUpObject(GameObject obj)
        {
            var tracker = obj.gameObject.AddComponent<ObjectTrackerScript>();
            tracker.storage = gameEvents.storage;
        }
    }
}
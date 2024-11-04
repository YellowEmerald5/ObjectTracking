using BehaviourScripts;
using ObjectTracking.GameEventScripts;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.Events;

namespace SetUpScripts
{
    public class SetUpObjectTracking : MonoBehaviour
    {
        private RequiredScriptableObjectsStorage gameEvents;
        [SerializeField] public bool TrackChildObjects;
        [SerializeField] public bool TrackParents;

        /// <summary>
        /// Sets up a ObjectTracker script for the object it is attached to
        /// </summary>
        private void Start()
        {
            gameEvents = FindObjectOfType<RequiredScriptableObjectsStorageScript>().requiredScriptables;
            if (TrackChildObjects)
            {
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
            print(childObjects.Length);
            if (TrackParents)
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
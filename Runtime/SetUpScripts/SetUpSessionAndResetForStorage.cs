using BehaviourScripts;
using ObjectTracking.GameEventScripts;
using ScriptableObjectScripts;
using UnityEngine;

namespace SetUpScripts
{
    public class SetUpSessionAndResetForStorage : MonoBehaviour
    {
        [SerializeField] public RequiredScriptableObjectsStorage requiredScriptableObjects;
        private GameEventListener eventListener;
        private SessionSetup sessionSetup;
        private StorageReset storageReset;
        
        /// <summary>
        /// Sets up the session and script for resetting the storage.
        /// </summary>
        private void OnEnable()
        {
            var obj = gameObject;
            sessionSetup = obj.AddComponent<SessionSetup>();
            sessionSetup.storage = requiredScriptableObjects.storage;
            sessionSetup.SetUpDefaultSession();
            storageReset = obj.AddComponent<StorageReset>();
            storageReset.storage = requiredScriptableObjects.storage;
            
            GameEventListenerSetup.SetUpEventListener(obj,sessionSetup.GetSessionCount,requiredScriptableObjects.nicknameAdded);
        }
    }
}
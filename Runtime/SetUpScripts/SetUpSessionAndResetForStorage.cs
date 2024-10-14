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

        //Adds and sets up the storage control and a nickname game event listener for the application
        //The storage controller gets the session id from the database and resets the StorageSO when exiting play mode
        //The game event listener reacts when a nickname is entered and checks if it exists in the database
        private void OnEnable()
        {
            sessionSetup = gameObject.AddComponent<SessionSetup>();
            sessionSetup.storage = requiredScriptableObjects.storage;
            storageReset = gameObject.AddComponent<StorageReset>();
            storageReset.storage = requiredScriptableObjects.storage;

            if (sessionSetup.storage.User != null) return;

            GameEventListenerSetup.SetUpEventListener(gameObject,sessionSetup.GetSessionCount,requiredScriptableObjects.nicknameAdded);
        }
    }
}
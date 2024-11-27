using System.Collections.Generic;
using Objects;
using ScriptableObjectScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BehaviourScripts
{
    public class StorageReset : MonoBehaviour
    {
        public StorageSO storage;
        
        //Sets up a listener to reset the storageSO when play mode is exited in the editor
        //This will not run in the built application
        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += ResetStorageSO;
        }

        /// <summary>
        /// Resets all values in the StorageSO when play mode is exited.
        /// This is only necessary in the editor due to the behaviour of scriptable objects
        /// </summary>
        /// <param name="state">Represents the current state of the editor</param>
        private void ResetStorageSO(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.EnteredEditMode) return;
            if (storage.ContainsItems)
            {
                DatabaseManager.SaveStorageSOToDatabase(storage);
            }
            storage.User = null;
            storage.GameID = 0;
            storage.sessionID = 0;
            storage.nickname = "";
            storage.ContainsItems = false;
            storage.StartTracking = false;
        }
    }
}
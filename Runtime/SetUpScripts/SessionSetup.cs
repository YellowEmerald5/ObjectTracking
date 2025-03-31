using System;
using System.Collections.Generic;
using System.Linq;
using BehaviourScripts;
using Objects;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SetUpScripts
{
    public class SessionSetup : MonoBehaviour
    {
        public StorageSO storage;
        private bool _saved;

        /// <summary>
        /// Gets the session count associated with the nickname from the database
        /// and adds it to the StorageSO scriptable object
        /// </summary>
        public void GetGameCount()
        {
            storage.User = DatabaseManager.GetUser(storage.nickname, storage);
        }

        /// <summary>
        /// Sets up a scenario without a user
        /// </summary>
        public void SetUpDefaultSession()
        {
            if (storage.User != null) return;
            storage.nickname = "";
            GetGameCount();
        }
    }
}
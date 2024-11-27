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
        public void GetSessionCount()
        {
            var user = DatabaseManager.GetUser(storage.nickname,storage);
            storage.User = user;
            var sessionCount = DatabaseManager.GetSessionCount(storage.User.Id);
            storage.sessionID = sessionCount+1;
            storage.User.Sessions.Add(new Session(storage.sessionID, storage.User.Id));
            storage.User.Sessions.Last().GamesList = new List<Game>();
        }

        /// <summary>
        /// Sets up a scenario without a user
        /// </summary>
        public void SetUpDefaultSession()
        {
            if (storage.User != null) return;
            storage.nickname = "";
            GetSessionCount();
        }
    }
}
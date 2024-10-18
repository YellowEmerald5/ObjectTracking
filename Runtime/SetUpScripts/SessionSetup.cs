using BehaviourScripts;
using Objects;
using ScriptableObjectScripts;
using UnityEngine;

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
            if (!_saved && storage.ContainsItems)
            {
                DatabaseManager.SaveStorageSOToDatabase(storage);
                _saved = true;
            }
            var user = DatabaseManager.GetUser(storage.nickname,storage);
            storage.User = user;
            var sessionCount = DatabaseManager.GetSessionCount(storage.User.Id);
            storage.sessionID = sessionCount+1;
            storage.User.Sessions.Add(new Session(storage.sessionID, storage.User.Id));
        }

        /// <summary>
        /// Sets up a scenario without a user
        /// </summary>
        private void OnDestroy()
        {
            if (!_saved && storage.ContainsItems)
            {
                DatabaseManager.SaveStorageSOToDatabase(storage);
            }
            if (storage.User != null) return;
            storage.nickname = "";
            GetSessionCount();
        }
    }
}
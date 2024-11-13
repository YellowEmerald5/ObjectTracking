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

        private void Awake()
        {
            SceneManager.activeSceneChanged += OnLoad;
        }

        /// <summary>
        /// Gets the session count associated with the nickname from the database
        /// and adds it to the StorageSO scriptable object
        /// </summary>
        public void GetSessionCount()
        {
            if (storage.ContainsItems)
            {
                SetObjectEnd();
                DatabaseManager.SaveStorageSOToDatabase(storage);
                storage.ContainsItems = false;
                _saved = true;
                storage.User.Sessions.Last().GamesList = new List<Game>();
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
        private void OnLoad(Scene current, Scene next)
        {
            if (!_saved && storage.ContainsItems)
            {
                SetObjectEnd();
                DatabaseManager.SaveStorageSOToDatabase(storage);
                storage.ContainsItems = false;
            }
            if (storage.User != null) return;
            storage.nickname = "";
            GetSessionCount();
        }

        private void SetObjectEnd()
        {
            foreach (var obj in storage.User.Sessions[^1].GamesList[^1].Objects)
            {
                obj.TimeDestroyed = obj.Points[^1].Time;
                var point = obj.Points[^1];
                obj.EndPositionX = point.PosX;
                obj.EndPositionY = point.PosY;

                obj.Aoi.TimeDestroy = obj.Aoi.TimeDestroy = obj.TimeDestroyed;
                var origin = obj.Aoi.Origins[^1];
                obj.Aoi.EndPositionX = origin.PosX;
                obj.Aoi.EndPositionY = origin.PosY;
            }
        }
    }
}
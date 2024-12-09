using System;
using System.Collections.Generic;
using System.Linq;
using Objects;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BehaviourScripts
{
    public class GameStateTracker : MonoBehaviour
    {
        [SerializeField] public RequiredScriptableObjectsStorage scriptableObjects;
        private StorageSO storage;

        /// <summary>
        /// Sets up the StorageSO for the current session and game and sets startTracking to true
        /// StartTracking is used to inform gameobjects that the required data is prepared
        /// </summary>
        void Start()
        {
            storage = scriptableObjects.storage;
            var key = SceneManager.GetActiveScene().name;
            if (!storage.currentTimePlaying.ContainsKey(key))
            {
                storage.currentTimePlaying.Add(key, 1);
            }
            else
            {
                storage.currentTimePlaying[key] ++;
            }
            if(storage.User == null) return;
            storage.User.Sessions[^1].GamesList.Add(new Game(storage.currentTimePlaying[key],storage.currentTimePlaying.Count,SceneManager.GetActiveScene().name,storage.User.Id,storage.sessionID));
            storage.GameID = storage.User.Sessions[^1].GamesList[^1].Id;
            storage.StartTracking = true;
            scriptableObjects.gameReady.Raise();
            storage.ContainsItems = true;
        }

        /// <summary>
        /// Ensures the currentObject is zero for the next scene to accurately represent the amount of objects present in the current game
        /// Ensures game objects will not start tracking prior to required data becoming available
        /// </summary>
        private void OnDisable()
        {
            storage.CurrentObject = 0;
            storage.StartTracking = false;
        }
    }
}

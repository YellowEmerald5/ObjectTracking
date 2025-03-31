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
            
            if(storage.User == null) return;
            var userId = storage.User.Id;
            var gameName = SceneManager.GetActiveScene().name;
            
            var id = DatabaseManager.GetAvailableGameID();
            storage.GameID = id;
            storage.availableGameId = id;
            var currentTimePlaying = DatabaseManager.AmountOfTimesPlayingTheSameGame(userId,gameName);
            storage.User.Games.Add(new Game(storage.availableGameId,currentTimePlaying+1,SceneManager.GetActiveScene().name, userId,Screen.height,Screen.width));
            storage.availableGameId++;
            storage.currentTimePlaying = currentTimePlaying;
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

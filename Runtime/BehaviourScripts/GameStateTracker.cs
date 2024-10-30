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

        //Sets up the StorageSO for the current session and game
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
            storage.User.Sessions.Last().GamesList = new List<Game>();
            storage.User.Sessions[^1].GamesList.Add(new Game(storage.currentTimePlaying[key],SceneManager.GetActiveScene().name,storage.User.Id,storage.sessionID));
            storage.GameID = storage.User.Sessions[^1].GamesList[^1].Id;
            scriptableObjects.gameReady.Raise();
            storage.ContainsItems = true;
        }

        private void OnDisable()
        {
            storage.CurrentObject = 0;
        }
    }
}

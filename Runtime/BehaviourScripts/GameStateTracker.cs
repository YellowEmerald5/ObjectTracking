using System;
using System.Collections.Generic;
using System.Linq;
using Objects;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BehaviourScripts
{
    public class GameStateTracker : MonoBehaviour
    {
        [SerializeField] public StorageSO storage;

        //Sets up the StorageSO for the current session and game
        void Start()
        {
            var key = SceneManager.GetActiveScene().name;
            if (!storage.currentTimePlaying.ContainsKey(key))
            {
                storage.currentTimePlaying.Add(key, 1);
            }
            else
            {
                storage.currentTimePlaying[key] ++;
            }
            storage.User.Sessions.Last().GamesList = new List<Game>();
            storage.User.Sessions[^1].GamesList.Add(new Game(storage.currentTimePlaying[key],SceneManager.GetActiveScene().name,storage.User.Id,storage.sessionID));
            storage.ContainsItems = true;
        }

        private void OnDisable()
        {
            storage.CurrentObject = 0;
        }
    }
}

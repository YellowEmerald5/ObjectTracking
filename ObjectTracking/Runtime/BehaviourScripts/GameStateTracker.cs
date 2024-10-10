using System.Collections.Generic;
using System.Linq;
using Objects;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BehaviourScripts
{
    public class GameStateTracker : MonoBehaviour
    {
        [SerializeField] public StorageSO storage;
        [SerializeField] public UnityEvent completedWritingObjectsToStorage;
        private int amountOfObjects = 0;

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
            
        }

        /// <summary>
        /// Runs when the ObjectCreated game event is raised
        /// Increments amountOfObjects by 1
        /// </summary>
        public void ObjectCreated()
        {
            amountOfObjects++;
        }

        /// <summary>
        /// Runs when the ObjectAddedToList game event is raised
        /// Decrements amountOfObjects by 1 until 0
        /// Raises the CompletedWriting game event when amountOfObjects = 0
        /// </summary>
        public void WaitForDataCollection()
        {
            amountOfObjects--;
            if (amountOfObjects > 0) return;
            DatabaseManager.SaveToDatabase(storage);
            print("saved");
        }
    }
}

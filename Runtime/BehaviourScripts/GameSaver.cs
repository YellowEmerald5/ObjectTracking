using System;
using System.Collections.Generic;
using Codice.Client.Common.EventTracking;
using Objects;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BehaviourScripts
{
    public class GameSaver : MonoBehaviour
    {
        [SerializeField] private StorageSO storage;
        
        /// <summary>
        /// Makes the SceneManager inform this script when a new scene is loaded
        /// </summary>
        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        /// <summary>
        /// Saves the data in the storage and empties the game list to lessen program impact
        /// </summary>
        /// <param name="current">The current loaded scene (Unused)</param>
        /// <param name="next">The next scene to be loaded (Unused)</param>
        private void OnSceneChanged(Scene current, Scene next)
        {
            if (!storage.ContainsItems) return;
            storage.ContainsItems = false;
            SetObjectEnd();
            DatabaseManager.SaveStorageSOToDatabase(storage);
        }
        
        /// <summary>
        /// Sets the last values for the object to the end values of the object
        /// </summary>
        private void SetObjectEnd()
        {
            foreach (var obj in storage.User.Games[^1].Objects)
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
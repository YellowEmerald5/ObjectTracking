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
        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnSceneChanged(Scene current, Scene next)
        {
            if (!storage.ContainsItems) return;
            storage.ContainsItems = false;
            SetObjectEnd();
            DatabaseManager.SaveStorageSOToDatabase(storage);
            storage.User.Sessions[^1].GamesList = new List<Game>();
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
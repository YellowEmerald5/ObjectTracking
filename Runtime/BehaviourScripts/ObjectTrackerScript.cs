using System;
using Objects;
using ScriptableObjectScripts;
using UnityEngine;

namespace BehaviourScripts
{
    public class ObjectTrackerScript : MonoBehaviour
    {
        //Place this script on all objects to be tracked
        public StorageSO storage;
        private int _positionOfObject;
        private Aoi _aoi;
        private Camera _cam;
        private bool _cameraDestroyed = false;
        private Renderer _renderer;
        private bool _objectAdded;
        private bool _isTracking;
        private int _objectId;
        
        /// <summary>
        /// Sets up the AOI and object tracking before raising the ObjectCreated game event
        /// </summary>
        public void StartTracker()
        {
            if (storage.User == null) return;
            _cam = FindObjectOfType<Camera>(); 
            _renderer = GetComponent<Renderer>();
            var scale = SizeOnScreen();
            var pos = FindPositionOnScreen();
            var gameId = storage.GameID;
            _objectId = storage.availableObjectId + storage.User.Games[^1].Objects.Count;
            _aoi = new Aoi(_objectId,_objectId,DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),pos);
            _aoi.Sizes.Add(new AoiSize(_aoi.Id,scale.y,scale.x));
            storage.User.Games[^1].Objects.Add(new ObjectInGame(_objectId,name,_aoi,gameId,DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),pos.x,pos.y,pos.z));
            _positionOfObject = storage.User.Games[^1].Objects.Count - 1;
            storage.CurrentObject++;
            _objectAdded = true;
            AddPosition();
        }

        /// <summary>
        /// Tracks the current millisecond utc and position of the object every frame
        /// </summary>
        private void Update()
        {
            if (!_objectAdded) return;
            AddPosition();
            var scale = SizeOnScreen();
            _aoi.Sizes.Add(new AoiSize(_aoi.Id,scale.y,scale.x));
        }

        /// <summary>
        /// Adds the current position of the object and aoi to the storage
        /// </summary>
        private void AddPosition()
        {
            var pos = FindPositionOnScreen();
            storage.User.Games[^1].Objects[_positionOfObject].Points.Add(new Point(_objectId
                , DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), pos.x, pos.y, pos.z));
            storage.User.Games[^1].Objects[_positionOfObject].Aoi.Origins.Add(new AoiOrigin(_aoi.Id,pos));
        }

        /// <summary>
        /// Finds the current position in the screen space
        /// </summary>
        /// <returns>Current position</returns>
        private Vector3 FindPositionOnScreen()
        {
            if (!_cameraDestroyed && _cam == null) _cameraDestroyed = true;
            if (_cameraDestroyed) return new Vector3();
            var pos = transform.position;
            var positionOnScreen = _cam.WorldToScreenPoint(pos);
            return positionOnScreen;

        }

        /// <summary>
        /// Adds this ObjectInGame to the list of game objects in the currently played game in the current session
        /// </summary>
        private void AddObjectToList()
        {
            var pos = FindPositionOnScreen();
            var time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            storage.User.Games[^1].Objects[_positionOfObject].Aoi.TimeDestroy = time;
            storage.User.Games[^1].Objects[_positionOfObject].Aoi.EndPositionX = pos.x;
            storage.User.Games[^1].Objects[_positionOfObject].Aoi.EndPositionY = pos.y;
            storage.User.Games[^1].Objects[_positionOfObject].TimeDestroyed = time;
            storage.User.Games[^1].Objects[_positionOfObject].EndPositionX = pos.x;
            storage.User.Games[^1].Objects[_positionOfObject].EndPositionX = pos.y;
            Destroy(this);
        }

        /// <summary>
        /// Finds the current pixel counts of the object boundary
        /// </summary>
        /// <returns>Object size</returns>
        private Vector2 SizeOnScreen()
        {
            var bounds = _renderer.bounds;
            var min = bounds.min * 1.05f;
            var max = bounds.max * 1.05f;

            var screenMin = _cam.WorldToScreenPoint(min);
            var screenMax = _cam.WorldToScreenPoint(max);

            var screenWidth = screenMax.x - screenMin.x;
            var screenHeight = screenMax.y - screenMin.y;

            var result = new Vector2(screenWidth,screenHeight);
            return result;
        }
    }
}

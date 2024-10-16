using System;
using Objects;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BehaviourScripts
{
    public class ObjectTrackerScript : MonoBehaviour
    {
        //Place this script on all objects to be tracked
        [SerializeField] public StorageSO storage;
        [SerializeField] public UnityEvent objectCreatedEvent;
        [SerializeField] public UnityEvent objectAddedToListEvent;
        private int PositionOfObject;
        private Aoi m_Aoi;
        private bool inList = false;
        private Camera cam;
        private string Name;
        private bool CameraDestroyed = false;
        private Renderer Renderer;
        
        //Sets up the AOI and object tracking before raising the ObjectCreated game event
        private void Start()
        {
            cam = FindObjectOfType<Camera>(); 
            Renderer = GetComponent<Renderer>();
            var scale = SizeOnScreen();
            var pos = FindPositionOnScreen();
            var session = storage.User.Sessions[^1];
            var gameId = session.GamesList[^1].Id;
            m_Aoi = new Aoi(gameId + " " + name,DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),pos);
            m_Aoi.Sizes.Add(new AoiSize(m_Aoi.Id,scale.y,scale.x));
            storage.User.Sessions[^1].GamesList[^1].Objects.Add(new ObjectInGame(name,m_Aoi,gameId,DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),pos.x,pos.y));
            Name = storage.User.Sessions[^1].GamesList[^1].Objects[^1].Name;
            PositionOfObject = storage.User.Sessions[^1].GamesList[^1].Objects.Count - 1;
            AddPosition();
            objectCreatedEvent.Invoke();
        }

        /// <summary>
        /// Runs when the object is disabled before the game changes scene
        /// Writes the tracked object to StorageSO if it is not present in the list
        /// </summary>
        public void OnDisable()
        {
            AddObjectToList();
        }

        //Runs when the game object is destroyed
        //It writes the tracked object to the StorageSO if it is not present in the list
        private void OnDestroy()
        {
            AddObjectToList();
        }

        //Tracks the current millisecond utc and position of the object every frame
        private void Update()
        {
            AddPosition();
            var scale = SizeOnScreen();
            m_Aoi.Sizes.Add(new AoiSize(m_Aoi.Id,scale.y,scale.x));
        }

        /// <summary>
        /// Adds the current position of the object and aoi to the storage
        /// </summary>
        private void AddPosition()
        {
            var pos = FindPositionOnScreen();
            storage.User.Sessions[^1].GamesList[^1].Objects[PositionOfObject].Points.Add(new Point(
                Name, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), pos.x, pos.y));
            storage.User.Sessions[^1].GamesList[^1].Objects[PositionOfObject].Aoi.Origins.Add(new AoiOrigin(m_Aoi.Id,pos));
        }

        /// <summary>
        /// Finds the current position in the screen space
        /// </summary>
        /// <returns>Current position</returns>
        private Vector3 FindPositionOnScreen()
        {
            if (!CameraDestroyed && cam == null) CameraDestroyed = true;
            if (CameraDestroyed) return new Vector3();
            var pos = transform.position;
            var windowPosition = Screen.mainWindowPosition;
            var positionOnScreen = cam.WorldToScreenPoint(pos);
            var screenHeight = Screen.mainWindowDisplayInfo.height;
            positionOnScreen.x += windowPosition.x;
            positionOnScreen.y += screenHeight - (windowPosition.y + Screen.height);
            return positionOnScreen;

        }

        /// <summary>
        /// Adds this ObjectInGame to the list of game objects in the currently played game in the current session
        /// </summary>
        private void AddObjectToList()
        {
            if(inList) return;
            inList = true;
            var pos = FindPositionOnScreen();
            m_Aoi.TimeDestroy = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            m_Aoi.EndPositionX = pos.x;
            m_Aoi.EndPositionY = pos.y;
            storage.User.Sessions[^1].GamesList[^1].Objects[PositionOfObject].TimeDestroyed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            storage.User.Sessions[^1].GamesList[^1].Objects[PositionOfObject].EndPositionX = pos.x;
            storage.User.Sessions[^1].GamesList[^1].Objects[PositionOfObject].EndPositionX = pos.y;
            objectAddedToListEvent.Invoke();
            Destroy(this);
        }

        /// <summary>
        /// Finds the current pixel counts of the object boundary
        /// </summary>
        /// <returns>Object size</returns>
        private Vector2 SizeOnScreen()
        {
            var scale = Renderer.bounds.size * 1.05f;
            var point = transform.position;
            var p00 = new Vector3(point.x - scale.x/2,point.y - scale.y/2,0);
            var p01 = new Vector3(point.x + scale.x / 2, point.y - scale.y / 2,0);
            var p10 = new Vector3(point.x - scale.x / 2, point.y + scale.y / 2,0);
            var p00S = cam.WorldToScreenPoint(p00);
            var p01S = cam.WorldToScreenPoint(p01);
            var p10S = cam.WorldToScreenPoint(p10);
            var result = new Vector2(p01S.x - p00S.x,p10S.y-p00S.y);
            return result;
        }
    }
}

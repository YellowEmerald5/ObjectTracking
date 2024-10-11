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
        private ObjectInGame trackedObject;
        private Aoi m_Aoi;
        private bool inList = false;
        private Camera cam;
        
        //Sets up the AOI and object tracking before raising the ObjectCreated game event
        private void Start()
        {
            cam = FindObjectOfType<Camera>();
            var scale = GetComponent<Renderer>().bounds.size*1.05f;
            var gameId = storage.User.Sessions[^1].GamesList[^1].Id;
            m_Aoi = new Aoi(gameId + " " + name,scale.y, scale.x);
            var a = new AreaOfInterest(m_Aoi.Height,m_Aoi.Width);
            trackedObject = new ObjectInGame(name,m_Aoi,gameId);
            AddPoint();
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
            AddPoint();
        }

        private void AddPoint()
        {
            var pos = transform.position;
            var windowPosition = Screen.mainWindowPosition;
            var positionOnScreen = cam.WorldToScreenPoint(pos);
            var screenHeight = Screen.mainWindowDisplayInfo.height;
            positionOnScreen.x += windowPosition.x;
            positionOnScreen.y += screenHeight - (windowPosition.y + Screen.height);
            trackedObject.Points.Add(new Point(trackedObject.Name,DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                positionOnScreen.x,positionOnScreen.y));
        }

        /// <summary>
        /// Adds this ObjectInGame to the list of game objects in the currently played game in the current session
        /// </summary>
        private void AddObjectToList()
        {
            if(inList) return;
            var sceneName = SceneManager.GetActiveScene().name;
            var game = storage.User.Sessions[^1]
                .GetGame(storage.currentTimePlaying[sceneName], sceneName);
            if (game == null) return;
            game.Objects.Add(trackedObject);
            inList = true;
            objectAddedToListEvent.Invoke();
        }
    }
}

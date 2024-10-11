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
        private AreaOfInterest Aoi;
        private bool inList = false;
        private Camera cam;
        
        //Sets up the AOI and object tracking before raising the ObjectCreated game event
        private void Start()
        {
            cam = FindObjectOfType<Camera>();
            var scale = SizeOnScreen();
            var pos = FindPositionOnScreen(); 
            var gameId = storage.User.Sessions[^1].GamesList[^1].Id;
            Aoi = new AreaOfInterest(gameId + " " + name,scale.x,scale.y);
            var origo = new AoiOrigin(Aoi.Id,pos);
            Aoi.Origins.Add(origo);
            m_Aoi = new Aoi(gameId + " " + name,scale.y, scale.x);
            storage.User.Sessions[^1].GamesList[^1].Objects.Add(new ObjectInGame(name,m_Aoi,gameId));
            PositionOfObject = storage.User.Sessions[^1].GamesList[^1].Objects.Count - 1;
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
            Aoi.Origins.Add(new AoiOrigin(Aoi.Id,FindPositionOnScreen()));
        }

        private void AddPoint()
        {
            var pos = FindPositionOnScreen(); 
            storage.User.Sessions[^1].GamesList[^1].Objects[PositionOfObject].Points.Add(new Point(storage.User.Sessions[^1].GamesList[^1].Objects[PositionOfObject].Name,DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                pos.x,pos.y));
        }

        private Vector3 FindPositionOnScreen()
        {
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
            DatabaseManager.SaveAoiToDatabase(Aoi);
            inList = true;
            objectAddedToListEvent.Invoke();
        }

        private Vector2 SizeOnScreen()
        {
            var scale = GetComponent<Renderer>().bounds.size*1.05f;
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

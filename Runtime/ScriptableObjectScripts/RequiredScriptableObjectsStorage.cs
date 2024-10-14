using ObjectTracking.GameEventScripts;
using UnityEngine;

namespace ScriptableObjectScripts
{
    [CreateAssetMenu(fileName = "RequiredScriptableObjectsStorage", menuName = "ScriptableObjects/RequiredScriptableObjectsStorage", order = 0)]
    public class RequiredScriptableObjectsStorage : ScriptableObject
    {
        //This object is created to contain scriptable objects needed to reliably set up tracking of objects
        public StorageSO storage;
        public GameEvent objectCreated;
        public GameEvent addedToList;
        public GameEvent gameFinished;
        public GameEvent completedWriting;
        public GameEvent nicknameAdded;
    }
}
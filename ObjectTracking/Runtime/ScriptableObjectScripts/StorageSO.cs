using System.Collections.Generic;
using Objects;
using UnityEngine;

namespace ScriptableObjectScripts
{
    [CreateAssetMenu(fileName = "StorageSO", menuName = "ScriptableObjects/StorageSO", order = 0)]
    public class StorageSO : ScriptableObject
    {
        public User User;
        public string nickname = "";
        public int sessionID;
        public Dictionary<string, int> currentTimePlaying = new Dictionary<string, int>();
        public int timesPlayed;
        public Dictionary<string, List<List<ObjectInGame>>> TrackedObjectsPerTimeGamePlayedInSession = new Dictionary<string, List<List<ObjectInGame>>>();

        public void SetNickname(string n)
        {
            nickname = n;
        }
    }
}
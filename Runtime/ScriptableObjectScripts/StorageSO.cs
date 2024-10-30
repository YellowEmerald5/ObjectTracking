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
        public Dictionary<string, int> currentTimePlaying = new ();
        public bool ContainsItems = false;
        public int GameID = 0;
        public int CurrentObject = 0;

        public void SetNickname(string n)
        {
            nickname = n;
        }
    }
}
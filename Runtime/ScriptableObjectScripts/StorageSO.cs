﻿using System.Collections.Generic;
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

        public void SetNickname(string n)
        {
            nickname = n;
        }
    }
}
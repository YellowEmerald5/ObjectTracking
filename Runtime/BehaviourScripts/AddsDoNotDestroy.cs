using System;
using UnityEngine;

namespace BehaviourScripts
{
    public class AddsDoNotDestroy : MonoBehaviour
    {
        /// <summary>
        /// Makes the game not destroy the object while loading
        /// </summary>
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
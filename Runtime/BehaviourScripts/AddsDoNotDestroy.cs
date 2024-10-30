using System;
using UnityEngine;

namespace BehaviourScripts
{
    public class AddsDoNotDestroy : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
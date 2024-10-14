using ScriptableObjectScripts;
using UnityEngine;

namespace SetUpScripts
{
    public class RequiredScriptableObjectsStorageScript : MonoBehaviour
    {
        //This script is used to ensure all scripts have access to the necessary scriptable objects in a reliable way
        [SerializeField] public RequiredScriptableObjectsStorage requiredScriptables;
    }
}
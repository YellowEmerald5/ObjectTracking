using ScriptableObjectScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BehaviourScripts
{
    public class KeepNickname : MonoBehaviour
    {
        [SerializeField] public TMP_InputField nicknameInput;
        [SerializeField] public StorageSO storage;

        /// <summary>
        /// Sets the text of the name input field to the given name to improve clarity
        /// </summary>
        private void Awake()
        {
            nicknameInput.text = storage.nickname;
        }
    }
}
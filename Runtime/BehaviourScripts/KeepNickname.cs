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

        private void Awake()
        {
            nicknameInput.text = storage.nickname;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.InputManager
{
    public class CustomInputManager : MonoBehaviour
    {
        //or use a normal static classs
        #region singleton 
        public static CustomInputManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); //to keep it in multiple scenes
            }

            InitializeDictionary();
        }
        #endregion

        public Dictionary<string, KeyCode> KeyMapping { get; private set; }
        public readonly Dictionary<string, KeyCode> DefaultkeyMapping = new Dictionary<string, KeyCode>()
        {
            //ingame inputs
            { "Attack", KeyCode.Mouse0 },
            { "SecondaryAttack", KeyCode.Mouse1 },
            { "Forward", KeyCode.W },
            { "Backward", KeyCode.S },
            { "Left", KeyCode.A },
            { "Right", KeyCode.D },
            { "Back/CancelAction", KeyCode.Escape},
            { "ToggleInventory", KeyCode.I},
            { "ToggleEquipment", KeyCode.E}
        };

        private void InitializeDictionary()
        {
            LoadKeyMappingFromPlayerPrefs();
        }

        public void ResetKeyMapping()
        {
            LoadKeyMapping(DefaultkeyMapping);
        }

        public void LoadKeyMappingFromPlayerPrefs()
        {
            LoadKeyMapping(SettingsPlayerPrefs.LoadControls(DefaultkeyMapping));
        }

        private void LoadKeyMapping(Dictionary<string, KeyCode> keyMapping)
        {
            InitializeOrClearKeyMapping();
            foreach (KeyValuePair<string, KeyCode> keyValuePairs in keyMapping)
            {
                KeyMapping.Add(keyValuePairs.Key, keyValuePairs.Value);
            }
        }

        private void InitializeOrClearKeyMapping()
        {
            if (KeyMapping != null)
            {
                KeyMapping.Clear();
            }
            else
            {
                KeyMapping = new Dictionary<string, KeyCode>();
            }
        }

        public bool GetButtonDown(string buttonName)
        {
            if (KeyMapping.ContainsKey(buttonName) == false)
            {
                Debug.LogError("CustomInputManager::GetButtonDown -- No button named: " + buttonName);
                return false;
            }

            return Input.GetKeyDown(KeyMapping[buttonName]);
        }

        public bool GetButton(string buttonName)
        {
            if (KeyMapping.ContainsKey(buttonName) == false)
            {
                Debug.LogError("CustomInputManager::GetButtonDown -- No button named: " + buttonName);
                return false;
            }
            return Input.GetKey(KeyMapping[buttonName]);
        }
        public string[] GetButtonNames()
        {
            return KeyMapping.Keys.ToArray();
        }

        public string GetKeyNameForButton(string buttonName)
        {
            if (KeyMapping.ContainsKey(buttonName) == false)
            {
                Debug.LogError("InputManager::GetKeyNameForButton -- No button named: " + buttonName);
                return "N/A";
            }

            return KeyMapping[buttonName].ToString();
        }

        public void SetKeyForButtonName(string buttonName, KeyCode keyCode)
        {
            KeyMapping[buttonName] = keyCode;
        }

        public int GetAxisRaw(string axisName) //Horizontal or Vertical
        {
            if (axisName.Equals("Horizontal"))
            {
                if (Input.GetKey(KeyMapping["Left"]) && !Input.GetKey(KeyMapping["Right"]))
                {
                    return -1;
                }
                else if (!Input.GetKey(KeyMapping["Left"]) && Input.GetKey(KeyMapping["Right"]))
                {
                    return 1;
                }
                return 0;
            }
            else if (axisName.Equals("Vertical"))
            {
                if (Input.GetKey(KeyMapping["Backward"]) && !Input.GetKey(KeyMapping["Forward"]))
                {
                    return -1;
                }
                else if (!Input.GetKey(KeyMapping["Backward"]) && Input.GetKey(KeyMapping["Forward"]))
                {
                    return 1;
                }
                return 0;
            }
            else
            {
                throw new ArgumentException("olnly Horizontal or Vertical values are accepted");
            }
        }
    }
}


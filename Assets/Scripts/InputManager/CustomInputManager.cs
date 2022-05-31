using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.InputManager
{
    public class CustomInputManager : MonoBehaviour //should rewrite it to static class
    {
        public Dictionary<string, KeyCode> KeyMapping { get; private set; }
        private readonly Dictionary<string, KeyCode> DefaultkeyMapping = new Dictionary<string, KeyCode>() //should be from file aka default player pre
        {
            { "Attack", KeyCode.Q },
            { "Forward", KeyCode.W },
            { "Backward", KeyCode.S },
            { "Left", KeyCode.A },
            { "Right", KeyCode.D }
        };

        private void OnEnable()
        {
            InitializeDictionary();
        }

        private void OnDisable()
        {
            
        }

        private void InitializeDictionary()
        {
            //if player preferences null/corrupted then init default controls: (TODO: if)
            KeyMapping = new Dictionary<string, KeyCode>();
            foreach (var keymap in DefaultkeyMapping)
            {
                KeyMapping.Add(keymap.Key, keymap.Value);
            }
        }

        public bool GetKeyDown(string buttonName)
        {
            if (KeyMapping.ContainsKey(buttonName) == false)
            {
                Debug.LogError("CustomInputManager::GetButtonDown -- No button named: " + buttonName);
                return false;
            }

            return Input.GetKeyDown(KeyMapping[buttonName]);
        }

        public bool GetKey(string buttonName)
        {
            if (KeyMapping.ContainsKey(buttonName) == false)
            {
                Debug.LogError("CustomInputManager::GetButtonDown -- No button named: " + buttonName);
                return false;
            }
            return Input.GetKey(KeyMapping[buttonName]);
        }

        //not needed currently
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


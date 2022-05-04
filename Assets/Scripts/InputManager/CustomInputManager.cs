using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.InputManager
{
    public class CustomInputManager
    {
        static Dictionary<string, KeyCode> keyMapping;
        readonly static Dictionary<string, KeyCode> defaultkeyMapping = new Dictionary<string, KeyCode>()
        {
            { "Attack", KeyCode.Q },
            { "Forward", KeyCode.W },
            { "Backward", KeyCode.S },
            { "Left", KeyCode.A },
            { "Right", KeyCode.D }
        };

        static CustomInputManager()
        {
            InitializeDictionary();
        }

        private static void InitializeDictionary()
        {
            //if player preferences null/corrupted then init default controls: (TODO: if)
            keyMapping = new Dictionary<string, KeyCode>();
            foreach (var keymap in defaultkeyMapping)
            {
                keyMapping.Add(keymap.Key, keymap.Value);
            }
        }

        public static void SetKeyMap(string keyMap, KeyCode key)
        {
            if (!keyMapping.ContainsKey(keyMap))
                throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap); //should show error on UI too
            keyMapping[keyMap] = key;
        }

        public static bool GetKeyDown(string keyMap)
        {
            return Input.GetKeyDown(keyMapping[keyMap]);
        }

        public static int GetAxisRaw(string axisName) //Horizontal or Vertical
        {
            if (axisName.Equals("Horizontal"))
            {
                if (Input.GetKey(keyMapping["Left"]) && !Input.GetKey(keyMapping["Right"]))
                {
                    return -1;
                }
                else if (!Input.GetKey(keyMapping["Left"]) && Input.GetKey(keyMapping["Right"]))
                {
                    return 1;
                }
                return 0;
            }
            else if (axisName.Equals("Vertical"))
            {
                if (Input.GetKey(keyMapping["Backward"]) && !Input.GetKey(keyMapping["Forward"]))
                {
                    return -1;
                }
                else if (!Input.GetKey(keyMapping["Backward"]) && Input.GetKey(keyMapping["Forward"]))
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


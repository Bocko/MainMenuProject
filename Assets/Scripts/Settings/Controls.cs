using Assets.Scripts.InputManager;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Controls : MonoBehaviour
    {
        private bool IsListeningKey = false;
        private string CurrnetlyPressedButtonName = null;
        private Button CurrnetlyPressedButton = null;

        private bool keysSpawned = false;

        [SerializeField] private GameObject KeyItemPrefab; //scriptable object?
        [SerializeField] private GameObject KeyItemList;

        [SerializeField] private CustomInputManager InputManager;

        [SerializeField] private Button applyButton;
        [SerializeField] private Button resetButton;

        private UnityAction applyAction;
        private UnityAction resetAction;

        private void Awake()
        {
            applyAction = new UnityAction(ApplySettings);
            resetAction = new UnityAction(ResetSettings);
        }

        private void OnEnable()
        {
            applyButton.onClick.AddListener(applyAction);
            resetButton.onClick.AddListener(resetAction);

            if(keysSpawned) { return; }
            keysSpawned = true;

            foreach (var keyValuePair in InputManager.KeyMapping)
            {
                //instantiate the keys
                GameObject currentKeyItem = (GameObject)Instantiate(KeyItemPrefab);
                currentKeyItem.transform.SetParent(KeyItemList.transform);
                currentKeyItem.transform.localScale = Vector3.one; //kell?

                TextMeshProUGUI buttonNameText = currentKeyItem.transform.Find("Button Name").GetComponent<TextMeshProUGUI>();
                buttonNameText.text = keyValuePair.Key;

                TextMeshProUGUI keyNameText = currentKeyItem.transform.Find("Button/Key Name").GetComponent<TextMeshProUGUI>();
                keyNameText.text = keyValuePair.Value.ToString();

                //add listener to button
                Button SetKeybutton = currentKeyItem.transform.Find("Button").GetComponent<Button>();
                SetKeybutton.onClick.AddListener(() => { StartKeyRebindFor(keyValuePair.Key, SetKeybutton); });
                //SetKeybutton.onClick.AddListener(() => { DoNothing(keyValuePair.Key); });

                //set tag for disabling listeners later or write a script for the perfab and disable the in onDestroy
                SetKeybutton.tag = "DisableMeLater"; //must create this tag first in editor
            }
        }

        private void OnDisable()
        {
            applyButton.onClick.RemoveListener(applyAction);
            resetButton.onClick.RemoveListener(resetAction);

            RemoveAllListenersFromButtons();
        }

        private void RemoveAllListenersFromButtons()
        //kell ez vagy megoldja Unity?
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("DisableMeLater"))
            {
                Button btn = go.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
            }
        }

        private void Update()
        {
            if (IsListeningKey) //&& CurrnetlyPressedButtonName != null) //ezt majd neide?
            {
                if (Input.anyKeyDown)
                {
                    KeyCode currentKey = DetectPressedKey();
                    InputManager.SetKeyForButtonName(CurrnetlyPressedButtonName, currentKey);
                    CurrnetlyPressedButton.GetComponentInChildren<TextMeshProUGUI>().text = currentKey.ToString();
                    OnChangeKeyPressedEnd();
                }
            }
        }

        private KeyCode DetectPressedKey()
        {
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode))) //could be a global variable for efficiency if Enum.GetValues(typeof(KeyCode)) not watching current keyboard
            {
                if (Input.GetKeyDown(kc))
                    return kc;
            }

            throw new KeyNotFoundException();
        }

        private void StartKeyRebindFor(string buttonName, Button btn)
        {
            //nezdd meg mi van ha esc mert lehet le kell tiltani
            IsListeningKey = true;
            CurrnetlyPressedButtonName = buttonName;
            CurrnetlyPressedButton = btn;
        }

        private void OnChangeKeyPressedEnd()
        {
            IsListeningKey = false;
            CurrnetlyPressedButtonName = null;
            CurrnetlyPressedButton = null;
        }

        private void ApplySettings()
        {
            print("saving controls");
            //set playerperfs to values shown on UI
        }

        private void ResetSettings()
        {
            print("reseting controls");
            //Reset settings back to default
        }

        private void CancelChanges()
        {
            //set the settings & the values on UI back to playerPrefs
            //shoud use this or ApplySettings() on menuchange
        }
    }
}

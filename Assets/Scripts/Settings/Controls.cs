using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Controls : MonoBehaviour
    {
        //Control mapping:

        //With old input sys:
        //only with self made Input manager, like:
        //https://forum.unity.com/threads/can-i-modify-the-input-manager-via-script.458800/

        //With new input sys: has several methods

        private void ApplySettings()
        {
            //set playerperfs to values shown on UI
        }

        private void ResetSettings()
        {
            //Reset settings back to default
        }

        private void CancelChanges()
        {
            //set the settings & the values on UI back to playerPrefs
            //shoud use this or ApplySettings() on menuchange
        }
    }
}

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
        // private float MouseSensitivity; akár lehet ezt is de ez játékfüggö, h hogy kell
        
        //Control mapping:

        //With old input sys:
        //only with self made Input manager, like:
        //https://forum.unity.com/threads/can-i-modify-the-input-manager-via-script.458800/

        //With new input sys: has several methods

        public void ApplySettings()
        {
            //set playerperfs to values shown on UI
        }

        public void ResetSettings()
        {
            //Reset settings back to default
        }

        public void CancelChanges()
        {
            //set the settings & the values on UI back to playerPrefs
            //shoud use this or ApplySettings() on menuchange
        }
    }
}

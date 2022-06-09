using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    //Apply this class to each Secene's Post processing gameobject, so on scnene change the original (the one that is on the 1st shown scene) post porcessing effects will be used
    public class PostProcessingSingletonHelper : MonoBehaviour
    {
        public static PostProcessingSingletonHelper Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject); //to keep it in multiple scenes
            }
        }
    }
}

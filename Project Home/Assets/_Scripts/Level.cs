using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    public class Level : MonoBehaviour
    {
        private static Level _instance = null;
        public static Level Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}


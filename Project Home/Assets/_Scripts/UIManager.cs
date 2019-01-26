using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace HOME
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        public static UIManager Instance { get { return _instance; } }

        [SerializeField] private Text currentFundsText;

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
            HOME.Player.Instance.FundsChanged += Instance_FundsChanged;
        }

        private void Instance_FundsChanged(object sender, Player.FundsChangedArgs e)
        {
            currentFundsText.text = "$" + e.newFunds.ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            if(_instance == this)
            {
                _instance = null;
            }
        }
    }
}


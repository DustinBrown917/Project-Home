using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

namespace HOME
{
    public class Player : MonoBehaviour
    {
        private static Player _instance = null;
        public static Player Instance { get { return _instance; } }

        [SerializeField] float _runStartFunds = 10000.0f;

        [SerializeField] private float _currentFunds = 0.0f;
        public float CurrentFunds { get { return _currentFunds; } }

        private Rewired.Player rwPlayer;
        private PlayerController pc;

        [SerializeField] private float costOfMove = 2.0f;
        [SerializeField] private Vector2 moveForce;
        [SerializeField] private Vector2 initialForce;

        private void Awake()
        {
            if(_instance == null) {
                _instance = this;
            } else {
                Destroy(gameObject);
            }

            rwPlayer = ReInput.players.GetPlayer(0);
            pc = GetComponent<PlayerController>();
        }

        // Start is called before the first frame update
        void Start()
        {
            SetCurrentFunds(_runStartFunds);
            BeginPlay();
        }

        // Update is called once per frame
        void Update()
        {
            GetRuns();
        }

        private void OnDestroy()
        {
            if(_instance == this)
            {
                _instance = null;
            }
        }

        public void GetRuns()
        {
            if ((rwPlayer.GetButtonDown("RunL") || rwPlayer.GetButtonDown("RunR")) && _currentFunds >= costOfMove && pc.IsGrounded()) {
                SetCurrentFunds(_currentFunds - costOfMove);
                pc.Propel(moveForce);
            }
            if (rwPlayer.GetButtonDown("Jump")) {
                pc.Jump();
            }
        }

        public void SetCurrentFunds(float funds)
        {
            FundsChangedArgs args = new FundsChangedArgs(_currentFunds, funds);
            _currentFunds = funds;
            OnFundsChanged(args);
        }

        public void BeginPlay()
        {
            pc.Propel(initialForce);
        }


        public event EventHandler<FundsChangedArgs> FundsChanged;

        public class FundsChangedArgs : EventArgs {
            public float oldFunds;
            public float newFunds;
            public float difference { get { return newFunds - oldFunds; } }

            public FundsChangedArgs(float oldFunds, float newFunds) : base()
            {
                this.oldFunds = oldFunds;
                this.newFunds = newFunds;
            }
        }

        public void OnFundsChanged(FundsChangedArgs e)
        {
            EventHandler<FundsChangedArgs> handler = FundsChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

    }
}



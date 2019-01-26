using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace HOME
{
    public class Player : MonoBehaviour
    {

        private Rewired.Player rwPlayer;

        private PlayerController pc;

        private Coroutine cr_JumpTimer = null;
        [SerializeField] private float jumpTimerDuration = 0.2f;

        private void Awake()
        {
            rwPlayer = ReInput.players.GetPlayer(0);
            pc = GetComponent<PlayerController>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            GetRuns();
        }

        public void GetRuns()
        {
            if (rwPlayer.GetButtonDown("RunL") || rwPlayer.GetButtonDown("RunR"))
            {
                pc.CancelDrag();
            }
            if (rwPlayer.GetButtonDown("Jump"))
            {
                pc.Jump();
            }
        }
    }
}



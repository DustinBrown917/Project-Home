using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance = null;
        public static GameManager Instance { get { return _instance; } }

        private GameStates _currentState = GameStates.PRE_PLAY;
        public GameStates CurrenState { get { return _currentState; } }

        private Coroutine cr_BeginPlayTimer = null;

        private void Awake()
        {
            if(_instance == null) {
                _instance = this;
            }
            else {
                Destroy(gameObject);
            }
            
        }

        // Start is called before the first frame update
        void Start()
        {
            CoroutineManager.BeginCoroutine(BeginPlayTimer(), ref cr_BeginPlayTimer, this);
            
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

        public void ResetRun()
        {
            OnRunReset();
            CoroutineManager.BeginCoroutine(BeginPlayTimer(), ref cr_BeginPlayTimer, this);
            _currentState = GameStates.PRE_PLAY;
        }

        public void BeginPlay()
        {
            OnBeginPlay();
            UIManager.Instance.BroadCastHighImpact("Wow! Such Independence!", true);
            _currentState = GameStates.PLAYING;
        }


        private IEnumerator BeginPlayTimer()
        {
            yield return new WaitForSeconds(2.0f);
            BeginPlay();
        }


        public event EventHandler RunReset;

        private void OnRunReset()
        {
            RunReset?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler PlayBegins;

        private void OnBeginPlay()
        {
            PlayBegins?.Invoke(this, EventArgs.Empty);
        }
    }

    
}


public enum GameStates
{
    PRE_PLAY,
    PLAYING
}


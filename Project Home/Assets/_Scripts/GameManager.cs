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

        [SerializeField] Player[] players;

        private Coroutine[] cr_BeginPlayTimers;

        [SerializeField] CameraFollow cameraFollower;
        [SerializeField] CanvasGroup endScreenCanvasGroup;
        [SerializeField] private float endScreenFadeDuration;
        [SerializeField] private float endScreenWaitTime;
        private Coroutine cr_EndScreen;
        [SerializeField] private string startScreenName;

        private void Awake()
        {
            if(_instance == null) {
                _instance = this;

                for(int i = 0; i < players.Length; i++)
                {
                    players[i].RegisterPlayer();
                }
            }
            else {
                Destroy(gameObject);
            }
            
        }

        // Start is called before the first frame update
        void Start()
        {
            cr_BeginPlayTimers = new Coroutine[PlayerManager.PlayerCount];
            for(int i = 0; i < PlayerManager.PlayerCount; i++)
            {
                
                CoroutineManager.BeginCoroutine(BeginPlayTimer(i), ref cr_BeginPlayTimers[i], this);
            }
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

        public void ResetRun(int playerIndex)
        {
            OnRunReset(playerIndex);
            CoroutineManager.BeginCoroutine(BeginPlayTimer(playerIndex), ref cr_BeginPlayTimers[playerIndex], this);
            _currentState = GameStates.PRE_PLAY;
        }

        public void BeginPlay(int playerIndex)
        {
            OnBeginPlay(playerIndex);
            UIManager.Instance.BroadCastHighImpact(playerIndex, "Wow! Such Independence!", true);
            _currentState = GameStates.PLAYING;
        }


        private IEnumerator BeginPlayTimer(int playerIndex)
        {
            yield return new WaitForSeconds(2.0f);
            BeginPlay(playerIndex);
        }


        public event EventHandler<RunResetArgs> RunReset;

        public class RunResetArgs : EventArgs
        {
            public int playerIndex;

            public RunResetArgs(int playerIndex) {
                this.playerIndex = playerIndex;
            }
        }

        private void OnRunReset(int playerIndex)
        {
            RunReset?.Invoke(this, new RunResetArgs(playerIndex));
        }

        public event EventHandler<PlayBeginsArgs> PlayBegins;

        public class PlayBeginsArgs : EventArgs
        {
            public int playerIndex;

            public PlayBeginsArgs(int playerIndex) {
                this.playerIndex = playerIndex;
            }
        }

        private void OnBeginPlay(int playerIndex)
        {
            PlayBegins?.Invoke(this, new PlayBeginsArgs(playerIndex));
        }

        public void Victory()
        {
            CoroutineManager.BeginCoroutine(VictoryScreen(), ref cr_EndScreen, this);
        }

        private IEnumerator VictoryScreen()
        {
            cameraFollower.SetFollow(false);
            _currentState = GameStates.POST_PLAY;
            yield return new WaitForSeconds(3);
            float t = 0;
            endScreenCanvasGroup.gameObject.SetActive(true);
            while (t < endScreenFadeDuration)
            {
                endScreenCanvasGroup.alpha = t / endScreenFadeDuration;
                t += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(endScreenWaitTime);

            SceneTransitioner.Instance.LoadScene(startScreenName);
        }
    }  
}


public enum GameStates
{
    PRE_PLAY,
    PLAYING,
    POST_PLAY
}


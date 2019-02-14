using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

namespace HOME
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private int preferredIndex;
        private int myIndex = -1;
        public int Index { get { return myIndex; } }

        private PlayerStates myCurrentState;
        public PlayerStates CurrentState { get { return myCurrentState; } }

        public float CurrentVelocity { get { return pc.Rb2d.velocity.x; } }

        [SerializeField] private MileStone[] achievements;
        public MileStone[] MileStones { get { return achievements; } } //Gamejam bad code!

        [SerializeField] float _runStartFunds = 1000.0f;

        [SerializeField] private float _currentFunds = 0.0f;
        public float CurrentFunds { get { return _currentFunds; } }

        private Rewired.Player rwPlayer;
        private PlayerController pc;

        [SerializeField] private float costOfMove = 2.0f;
        [SerializeField] private float moveForceDegenerationFactor = 1.5f;
        [SerializeField] private Vector2 moveForce;
        [SerializeField] private Vector2 dp_moveForce;
        [SerializeField] private Vector2 initialForce;
        [SerializeField] private GameObject startingPoint;
        [SerializeField] private RiseAndFlashText toastText;
        [SerializeField] private Transform textTarget;
        [SerializeField] private Canvas playerCanvas;

        private AudioSource audioSource;
        [SerializeField] private AudioClip run1;
        [SerializeField] private AudioClip run2;
        [SerializeField] private AudioClip launch;
        [SerializeField] private AudioClip slide;

        private bool sliding;
        private bool handleInput = false;
        private bool hadFootDownLastFrame = false;

        private Animator animator;

        Coroutine cr_AchievementChecker;

        private void Awake()
        {
            RegisterPlayer();

            for(int i = 0; i < achievements.Length; i++)
            {
                achievements[i].Unachieve();
            }

            rwPlayer = ReInput.players.GetPlayer(myIndex);
            pc = GetComponent<PlayerController>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            GameManager.Instance.RunReset += GameManager_RunReset;
            GameManager.Instance.PlayBegins += GameManager_PlayBegins;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        private void GameManager_RunReset(object sender, GameManager.RunResetArgs e)
        {
            if (e.playerIndex == myIndex) {
                ResetRun();
            }
        }

        private void GameManager_PlayBegins(object sender, GameManager.PlayBeginsArgs e)
        {
            if(e.playerIndex == myIndex) {
                BeginPlay();
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (!handleInput || myCurrentState != PlayerStates.PLAYING) { return; }
            GetRuns();
        }

        private void OnDestroy()
        {
            DeregisterPlayer();
        }

        public void RegisterPlayer()
        {
            if (PlayerManager.IsPlayerRegistered(this)) { return; }

            PlayerManager.AddPlayer(this, preferredIndex);
            myIndex = preferredIndex;
        }

        public void DeregisterPlayer()
        {
            PlayerManager.RemovePlayer(this);
        }

        public int GetLevel()
        {
            for(int i = 0; i < achievements.Length; i++)
            {
                if (!achievements[i].Achieved) { return i + 1; }
            }

            return achievements.Length;
        }

        public void GetRuns()
        {
            if (GameOptions.GetDancepadMode())
            {
                if ((rwPlayer.GetButtonDown("RunL") || rwPlayer.GetButtonDown("RunR")) && _currentFunds >= costOfMove && pc.IsGrounded())
                {
                    SetCurrentFunds(_currentFunds - costOfMove);
                    Vector2 f = dp_moveForce * Mathf.Clamp((moveForceDegenerationFactor - (CurrentVelocity / pc.CurrentMaxSpeed)), 0.0f, moveForceDegenerationFactor);
                    pc.Propel(f);
                    Debug.Log(f);
                }

                if ((!rwPlayer.GetButton("RunL") && !rwPlayer.GetButton("RunR")) && hadFootDownLastFrame)
                {
                    pc.Jump();
                }

                if (rwPlayer.GetButton("RunL") || rwPlayer.GetButton("RunR")) {
                    hadFootDownLastFrame = true;
                }
                else {
                    hadFootDownLastFrame = false;
                }

                if (rwPlayer.GetButtonDown("Slide") && hadFootDownLastFrame) {
                    sliding = true;
                    animator.SetBool("sliding", sliding);
                }
                else if (rwPlayer.GetButtonUp("Slide"))
                {
                    sliding = false;
                    animator.SetBool("sliding", sliding);
                }
            }
            else
            {
                if ((rwPlayer.GetButtonDown("RunL") || rwPlayer.GetButtonDown("RunR")) && _currentFunds >= costOfMove && pc.IsGrounded())
                {
                    SetCurrentFunds(_currentFunds - costOfMove);
                    Vector2 f = dp_moveForce * Mathf.Clamp((moveForceDegenerationFactor - (CurrentVelocity / pc.CurrentMaxSpeed)), 0.0f, moveForceDegenerationFactor);
                    pc.Propel(f);
                    Debug.Log(f);
                }
                if (rwPlayer.GetButtonDown("Jump"))
                {
                    pc.Jump();
                }

                if (rwPlayer.GetButtonDown("Slide"))
                {
                    sliding = true;
                    animator.SetBool("sliding", sliding);
                }
                else if (rwPlayer.GetButtonUp("Slide"))
                {
                    sliding = false;
                    animator.SetBool("sliding", sliding);
                }
            }

        }

        public void PlaySlideSound()
        {
            audioSource.clip = slide;
            audioSource.Play();
        }

        public void SetCurrentFunds(float funds)
        {
            if(funds < 0) { funds = 0; }
            FundsChangedArgs args = new FundsChangedArgs(_currentFunds, funds, myIndex);
            _currentFunds = funds;
            OnFundsChanged(args);
            if(Mathf.Abs(args.difference) > 50.0f)
            {
                SpawnText((args.difference > 0), args.difference.ToString());
            }
            
        }

        private void SpawnText(bool good, string text)
        {
            RiseAndFlashText raft = Instantiate(toastText.gameObject, playerCanvas.transform).GetComponent<RiseAndFlashText>();
            raft.SetGoodBad(good);
            raft.SetText(text);
            raft.SetTargetPosition(textTarget.transform);
            raft.SetDestroyWhenDone(true);
            raft.BeginAnimate();
        }

        public void BeginPlay()
        {
            myCurrentState = PlayerStates.PLAYING;
            SetCurrentFunds(_runStartFunds * GetLevel());
            pc.Propel(initialForce);
            audioSource.clip = launch;
            audioSource.Play();
            handleInput = true;
            CoroutineManager.BeginCoroutine(CheckAchievementsRoutine(), ref cr_AchievementChecker, this);
        }

        public void ResetRun()
        {
            myCurrentState = PlayerStates.PRE_PLAY;
            transform.position = new Vector3(startingPoint.transform.position.x, startingPoint.transform.position.y, transform.position.z);
            handleInput = false;
            if (sliding)
            {
                sliding = false;
                animator.SetBool("sliding", false);
            }
        }

        public void PlayRunSound(int sound)
        {
            if(sound == 1) { audioSource.clip = run1; }
            else { audioSource.clip = run2; }

            audioSource.Play();
        }

        private IEnumerator CheckAchievementsRoutine()
        {
            WaitForSeconds wfs = new WaitForSeconds(0.1f);
            while (true)
            {
                for (int i = 0; i < achievements.Length; i++)
                {
                    if (transform.position.x > achievements[i].GoalDistance && !achievements[i].Achieved)
                    {
                        achievements[i].Achieve(myIndex);
                        if(i == achievements.Length - 1)
                        {
                            myCurrentState = PlayerStates.POST_PLAY;
                            GameManager.Instance.Victory();
                        }
                        break;
                    }
                }
                yield return wfs;
            }
        }

        public event EventHandler<FundsChangedArgs> FundsChanged;

        public class FundsChangedArgs : EventArgs {
            public int playerIndex;
            public float oldFunds;
            public float newFunds;
            public float difference { get { return newFunds - oldFunds; } }

            public FundsChangedArgs(float oldFunds, float newFunds, int playerIndex) : base()
            {
                this.oldFunds = oldFunds;
                this.newFunds = newFunds;
                this.playerIndex = playerIndex;
            }
        }

        public void OnFundsChanged(FundsChangedArgs e)
        {
            FundsChanged?.Invoke(this, e);
        }

        public enum PlayerStates
        {
            PRE_PLAY,
            PLAYING,
            POST_PLAY
        }
    }
}



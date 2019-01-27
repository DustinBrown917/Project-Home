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

        public float CurrentVelocity { get { return pc.Rb2d.velocity.x; } }

        [SerializeField] private MileStone[] achievements;
        public MileStone[] MileStones { get { return achievements; } } //Gamejam bad code!

        [SerializeField] float _runStartFunds = 10000.0f;

        [SerializeField] private float _currentFunds = 0.0f;
        public float CurrentFunds { get { return _currentFunds; } }

        private Rewired.Player rwPlayer;
        private PlayerController pc;

        [SerializeField] private float costOfMove = 2.0f;
        [SerializeField] private Vector2 moveForce;
        [SerializeField] private Vector2 initialForce;
        [SerializeField] private GameObject startingPoint;
        [SerializeField] private RiseAndFlashText toastText;
        [SerializeField] private Transform textTarget;
        [SerializeField] private Canvas playerCanvas;

        private AudioSource audioSource;
        [SerializeField] private AudioClip run1;
        [SerializeField] private AudioClip run2;
        [SerializeField] private AudioClip launch;

        private bool sliding;
        private bool handleInput = false;

        private Animator animator;

        Coroutine cr_AchievementChecker;

        private void Awake()
        {
            if(_instance == null) {
                _instance = this;
            } else {
                Destroy(gameObject);
            }

            for(int i = 0; i < achievements.Length; i++)
            {
                achievements[i].Unachieve();
            }

            rwPlayer = ReInput.players.GetPlayer(0);
            pc = GetComponent<PlayerController>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.RunReset += GameManager_RunReset;
            GameManager.Instance.PlayBegins += GameManager_PlayBegins;
        }

        private void GameManager_PlayBegins(object sender, EventArgs e)
        {
            BeginPlay();
        }

        private void GameManager_RunReset(object sender, EventArgs e)
        {
            ResetRun();
        }

        // Update is called once per frame
        void Update()
        {
            if (!handleInput) { return; }
            GetRuns();
        }

        private void OnDestroy()
        {
            if(_instance == this)
            {
                _instance = null;
            }
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
            if ((rwPlayer.GetButtonDown("RunL") || rwPlayer.GetButtonDown("RunR")) && _currentFunds >= costOfMove && pc.IsGrounded()) {
                SetCurrentFunds(_currentFunds - costOfMove);
                pc.Propel(moveForce);
            }
            if (rwPlayer.GetButtonDown("Jump")) {
                pc.Jump();
            }

            if (rwPlayer.GetButtonDown("Slide"))
            {
                sliding = true;
                animator.SetBool("sliding", sliding);
            } else if (rwPlayer.GetButtonUp("Slide"))
            {
                sliding = false;
                animator.SetBool("sliding", sliding);
            }
        }

        public void SetCurrentFunds(float funds)
        {
            if(funds < 0) { funds = 0; }
            FundsChangedArgs args = new FundsChangedArgs(_currentFunds, funds);
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
            SetCurrentFunds(_runStartFunds * GetLevel());
            pc.Propel(initialForce);
            audioSource.clip = launch;
            audioSource.Play();
            handleInput = true;
            CoroutineManager.BeginCoroutine(CheckAchievementsRoutine(), ref cr_AchievementChecker, this);
        }

        public void ResetRun()
        {
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
                        achievements[i].Achieve();
                        break;
                    }
                }
                yield return wfs;
            }
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
            FundsChanged?.Invoke(this, e);
        }

    }
}



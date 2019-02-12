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

        [SerializeField] private Text[] fundsTexts;
        [SerializeField] private Text[] distanceTexts;
        [SerializeField] private RiseAndFlashText[] highImpactTexts;
        [SerializeField] private RiseAndFlashText[] lowImpactTexts;
        [SerializeField] private Slider[] progressSliders;
        private Coroutine cr_DistanceTimer = null;
        private Coroutine[] cr_ResetTimers;

        private AudioSource audioSource;
        [SerializeField] private AudioClip countdownSound;
        [SerializeField] private AudioClip loseSound;
        [SerializeField] private AudioClip achievementSound;
        [SerializeField] private AudioClip warningSound;

        private void Awake()
        {
            if (_instance == null) {
                _instance = this;
                audioSource = GetComponent<AudioSource>();
            }
            else {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            for(int i = 0; i < PlayerManager.PlayerCount; i++)
            {
                PlayerManager.GetPlayer(i).FundsChanged += Player_FundsChanged;
            }
            CoroutineManager.BeginCoroutine(UpdateDistanceText(), ref cr_DistanceTimer, this);

            for(int i = 0; i < PlayerManager.PlayerCount; i++)
            {
                highImpactTexts[i].ResetText();
                highImpactTexts[i].gameObject.SetActive(false);
                lowImpactTexts[i].ResetText();
                lowImpactTexts[i].gameObject.SetActive(false);
            }


            if (cr_ResetTimers == null || cr_ResetTimers.Length != PlayerManager.PlayerCount) {
                cr_ResetTimers = new Coroutine[PlayerManager.PlayerCount];
            }
        }

        private void Player_FundsChanged(object sender, Player.FundsChangedArgs e)
        {
            fundsTexts[e.playerIndex].text = "$" + e.newFunds.ToString();
        }

        private void OnDestroy()
        {
            if(_instance == this)
            {
                _instance = null;
            }
        }

        public void BroadCastHighImpact(int playerIndex, string str, bool good)
        {
            highImpactTexts[playerIndex].ResetText();
            highImpactTexts[playerIndex].SetText(str);
            highImpactTexts[playerIndex].SetGoodBad(good);
            highImpactTexts[playerIndex].gameObject.SetActive(true);
            highImpactTexts[playerIndex].BeginAnimate();
        }

        public void BroadCastLowImpact(int playerIndex, string str, bool good)
        {
            lowImpactTexts[playerIndex].ResetText();
            lowImpactTexts[playerIndex].SetText(str);
            lowImpactTexts[playerIndex].SetGoodBad(good);
            lowImpactTexts[playerIndex].gameObject.SetActive(true);
            lowImpactTexts[playerIndex].BeginAnimate();
        }

        private IEnumerator UpdateDistanceText()
        {
            WaitForSeconds wfs = new WaitForSeconds(0.2f);
            while (true)
            {
                yield return wfs;

                for(int i = 0; i < PlayerManager.PlayerCount; i++)
                {
                    Player p = PlayerManager.GetPlayer(i);
                    float distance = p.transform.position.x;

                    distanceTexts[i].text = distance.ToString("F2") + "m";

                    progressSliders[i].value = distance;

                    if (p.CurrentVelocity == 0.0f && cr_ResetTimers[i] == null)
                    {
                        if(p.CurrentState == Player.PlayerStates.PLAYING)
                        {
                            CoroutineManager.BeginCoroutine(ResetTimer(i), ref cr_ResetTimers[i], this);
                        }
                    } else if(p.CurrentVelocity > 0.0f && cr_ResetTimers[i] != null)
                    {
                        CoroutineManager.HaltCoroutine(ref cr_ResetTimers[i], this);
                    }
                }
            }
        }

        private IEnumerator ResetTimer(int playerIndex)
        {          
            if(PlayerManager.GetPlayer(playerIndex).CurrentFunds > 0) {
                UIManager.Instance.BroadCastHighImpact(playerIndex, "Keep Moving!", false);
            }
            else {
                UIManager.Instance.BroadCastHighImpact(playerIndex, "Broke as the Berlin Wall!", false);              
            }

            audioSource.clip = warningSound;
            audioSource.Play();

            yield return new WaitForSeconds(2.0f);

            float seconds = 3.0f;
            int intSeconds = 0;

            while(seconds > 0.0f)
            {
                if((int)seconds != intSeconds)
                {
                    intSeconds = (int)seconds;
                    UIManager.Instance.BroadCastHighImpact(playerIndex, (intSeconds + 1).ToString(), false);
                    audioSource.clip = countdownSound;
                    audioSource.Play();
                }

                seconds -= Time.deltaTime;
                yield return null;
            }
            UIManager.Instance.BroadCastHighImpact(playerIndex, "Parents! More Money Please!", false);
            audioSource.clip = loseSound;
            audioSource.Play();
            GameManager.Instance.ResetRun(playerIndex);
        }

        public void PlayAchievementSound()
        {
            audioSource.clip = achievementSound;
            audioSource.Play();
        }
    }


}


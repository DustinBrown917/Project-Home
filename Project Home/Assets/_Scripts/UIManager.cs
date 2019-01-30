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
        [SerializeField] private Text distanceText;
        [SerializeField] private RiseAndFlashText highImpactText;
        [SerializeField] private RiseAndFlashText lowImpactText;
        [SerializeField] private Slider progressSlider;
        private Coroutine cr_DistanceTimer = null;
        private Coroutine cr_ResetTimer = null;

        private AudioSource audioSource;
        [SerializeField] private AudioClip countdownSound;
        [SerializeField] private AudioClip loseSound;
        [SerializeField] private AudioClip achievementSound;
        [SerializeField] private AudioClip warningSound;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                audioSource = GetComponent<AudioSource>();
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
            CoroutineManager.BeginCoroutine(UpdateDistanceText(), ref cr_DistanceTimer, this);
            highImpactText.ResetText();
            highImpactText.gameObject.SetActive(false);
            lowImpactText.ResetText();
            lowImpactText.gameObject.SetActive(false);
        }

        private void Instance_FundsChanged(object sender, Player.FundsChangedArgs e)
        {
            currentFundsText.text = "$" + e.newFunds.ToString();
        }

        private void OnDestroy()
        {
            if(_instance == this)
            {
                _instance = null;
            }
        }

        public void BroadCastHighImpact(string str, bool good)
        {
            highImpactText.ResetText();
            highImpactText.SetText(str);
            highImpactText.SetGoodBad(good);
            highImpactText.gameObject.SetActive(true);
            highImpactText.BeginAnimate();
        }

        public void BroadCastLowImpact(string str, bool good)
        {
            lowImpactText.ResetText();
            lowImpactText.SetText(str);
            lowImpactText.SetGoodBad(good);
            lowImpactText.gameObject.SetActive(true);
            lowImpactText.BeginAnimate();
        }

        private IEnumerator UpdateDistanceText()
        {
            WaitForSeconds wfs = new WaitForSeconds(0.2f);
            while (true)
            {
                yield return wfs;
                distanceText.text = Player.Instance.transform.position.x.ToString("F2") + "m";

                progressSlider.value = Player.Instance.transform.position.x;

                if (Player.Instance.CurrentVelocity == 0.0f && cr_ResetTimer == null) 
                {
                    if (GameManager.Instance.CurrenState == GameStates.PLAYING) {
                        CoroutineManager.BeginCoroutine(ResetTimer(), ref cr_ResetTimer, this);
                    }
                } else if(Player.Instance.CurrentVelocity > 0.0f && cr_ResetTimer != null)
                {
                    CoroutineManager.HaltCoroutine(ref cr_ResetTimer, this);
                }
            }
        }

        private IEnumerator ResetTimer()
        {
            
            if(Player.Instance.CurrentFunds > 0) {
                UIManager.Instance.BroadCastHighImpact("Keep Moving!", false);
            }
            else {
                UIManager.Instance.BroadCastHighImpact("Broke as the Berlin Wall!", false);              
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
                    UIManager.Instance.BroadCastHighImpact((intSeconds + 1).ToString(), false);
                    audioSource.clip = countdownSound;
                    audioSource.Play();
                }

                seconds -= Time.deltaTime;
                yield return null;
            }
            UIManager.Instance.BroadCastHighImpact("Parents! More Money Please!", false);
            audioSource.clip = loseSound;
            audioSource.Play();
            GameManager.Instance.ResetRun();
        }

        public void PlayAchievementSound()
        {
            audioSource.clip = achievementSound;
            audioSource.Play();
        }
    }


}


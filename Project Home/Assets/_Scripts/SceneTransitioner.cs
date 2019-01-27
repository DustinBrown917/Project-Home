using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HOME
{
    public class SceneTransitioner : MonoBehaviour
    {
        private static SceneTransitioner _instance;
        public static SceneTransitioner Instance { get { return _instance; } }

        [SerializeField] private float transitionFadeDuration;
        private CanvasGroup cg;
        private Coroutine cr_Transitoning;

        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
                cg = GetComponent<CanvasGroup>();
                DontDestroyOnLoad(this.gameObject);
            } else
            {
                Destroy(this.gameObject);
            }


        }

        private void OnDestroy()
        {
            if(Instance == this)
            {
                _instance = null;
            }
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void LoadScene(string str)
        {
            CoroutineManager.BeginCoroutine(SceneTransition(str), ref cr_Transitoning, this);
        }

        private IEnumerator SceneTransition(string str)
        {
            float t = 0;

            while (t < transitionFadeDuration)
            {
                t += Time.deltaTime;
                cg.alpha = Mathf.Lerp(0, 1, t / transitionFadeDuration);
                yield return null;
            }

            SceneManager.LoadScene(str);
            yield return  new WaitForSeconds(0.1f);
            t = 0;

            while (t < transitionFadeDuration)
            {
                t += Time.deltaTime;
                cg.alpha = Mathf.Lerp(1, 0, t / transitionFadeDuration);
                yield return null;
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HOME
{
    public class RiseAndFlashText : MonoBehaviour
    {
        private Text text;

        [SerializeField] private Color goodColour;
        [SerializeField] private Color badColour;

        private Color flashColour1;
        [SerializeField] private Color flashColour2;

        [SerializeField] private Transform targetPosition;
        [SerializeField] private Transform startPosition;
        [SerializeField] private float animationTime = 2.0f;

        private Coroutine cr_Animation = null;

        private bool destroyWhenDone;

        private void Awake()
        {
            text = GetComponent<Text>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            
        }



        public void BeginAnimate()
        {
            CoroutineManager.BeginCoroutine(LifeTime(), ref cr_Animation, this);
        }

        public void SetTargetPosition(Transform t) {
            targetPosition = t;
        }

        public void SetText(string text)
        {
            this.text.text = text;
        }

        public void SetGoodBad(bool good)
        {
            if (good) { flashColour1 = goodColour; }
            else { flashColour1 = badColour; }
        }

        public void SetDestroyWhenDone(bool destroy)
        {
            destroyWhenDone = destroy;
        }

        public void ResetText()
        {
            CoroutineManager.HaltCoroutine(ref cr_Animation, this);
            if(startPosition != null) { transform.position = startPosition.position; }
            else { transform.position = new Vector3(0, 0, transform.position.z); }
        }

        private IEnumerator LifeTime()
        {
            int framesAtColour = 0;
            Color col1 = flashColour1;
            Color col2 = flashColour2;
            text.color = col1;

            float t = 0;

            Vector2 vel = new Vector2();

            while (t < animationTime)
            {

                if (framesAtColour == 5) {
                    framesAtColour = 0;
                    if (text.color == col1) {
                        col2.a = Mathf.Lerp(0, 1.0f, 1.0f - (t / animationTime));
                        text.color = col2;
                    }
                    else {
                        col1.a = Mathf.Lerp(0, 1.0f, 1.0f - (t / animationTime));
                        text.color = col1;
                    }
                }
                else { framesAtColour += 1; }

                transform.position = Vector2.SmoothDamp(transform.position, targetPosition.position, ref vel, 0.1f);

                t += Time.deltaTime;

                yield return null;
            }

            if (destroyWhenDone) {
                Destroy(gameObject);
            } else {
                gameObject.SetActive(false);
            }
            
        }
    }
}


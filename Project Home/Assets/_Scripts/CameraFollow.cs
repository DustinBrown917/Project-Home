using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] GameObject followTarget;
        [SerializeField] float smoothTime = 1.0f;
        [SerializeField] float xOffset;
        private float newX;
        private float vel;
        private Vector3 newPos;

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.RunReset += GameManager_RunReset;
        }

        private void GameManager_RunReset(object sender, System.EventArgs e)
        {
            ResetRun();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ResetRun()
        {
            transform.position = new Vector3(followTarget.transform.position.x, transform.position.y, transform.position.z);
        }

        private void FixedUpdate()
        {
            newX = Mathf.SmoothDamp(transform.position.x, followTarget.transform.position.x + xOffset, ref vel, smoothTime);
            newPos.x = newX;
            newPos.y = transform.position.y;
            newPos.z = transform.position.z;

            transform.position = newPos;
        }
    }
}


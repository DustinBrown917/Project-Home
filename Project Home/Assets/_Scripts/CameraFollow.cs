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

        private bool follow = true;
        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.RunReset += GameManager_RunReset;
        }

        private void GameManager_RunReset(object sender, GameManager.RunResetArgs e)
        {
            ResetRun(e.playerIndex);
        }

        public void ResetRun(int index)
        {
            Player p = followTarget.GetComponent<Player>();
            if(p!= null && index == p.Index) {
                transform.position = new Vector3(followTarget.transform.position.x, transform.position.y, transform.position.z);
            } 
        }

        public void SetFollow(bool follow)
        {
            this.follow = follow;
        }

        private void FixedUpdate()
        {
            if (!follow) { return; }
            newX = Mathf.SmoothDamp(transform.position.x, followTarget.transform.position.x + xOffset, ref vel, smoothTime);
            newPos.x = newX;
            newPos.y = transform.position.y;
            newPos.z = transform.position.z;

            transform.position = newPos;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME {
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Vector2 initialImpulse;
        private Rigidbody2D rb2d;

        [SerializeField] private Transform groundCheck;
        private float linearDrag;
        private bool jump;

        private Coroutine cr_DragTimer = null;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            linearDrag = rb2d.drag;
        }

        // Start is called before the first frame update
        void Start()
        {
            BeginPlay();
        }

        // Update is called once per frame
        void Update()
        {
            if(rb2d.velocity.magnitude < 0.1f && rb2d.velocity.magnitude > 0.0f )
            {
                rb2d.velocity = new Vector2();
            }
        }

        public void BeginPlay()
        {
            rb2d.AddForce(initialImpulse);
        }

        public void Jump()
        {
            if (!IsGrounded()) { return; }
            rb2d.AddForce(new Vector2(0.0f, 300.0f));
        }

        public bool IsGrounded()
        {
            return (Physics2D.OverlapCircle(groundCheck.position, 0.15f, 9) != null);
        }

        public void CancelDrag()
        {
            rb2d.drag = 0;

            if(cr_DragTimer != null)
            {
                StopCoroutine(cr_DragTimer);
                cr_DragTimer = null;
            }

            cr_DragTimer = StartCoroutine(DragTimer());
        }

        private IEnumerator DragTimer()
        {
            yield return new WaitForSeconds(0.04f);
            rb2d.drag = linearDrag;
            cr_DragTimer = null;
        }
    }
}



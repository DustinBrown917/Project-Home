using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME {
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D rb2d;

        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private float jumpForce;
        private bool jump;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(rb2d.velocity.x < 0.1f && rb2d.velocity.x > 0.0f )
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
        }

        public void Jump()
        {
            if (!IsGrounded()) { return; }
            rb2d.AddForce(new Vector2(0.0f, jumpForce));
        }

        public bool IsGrounded()
        {
            return (Physics2D.OverlapCircle(groundCheck.position, 0.15f, whatIsGround) != null);
        }

        public void Propel(Vector2 propulsion)
        {
            rb2d.AddForce(propulsion);
        }
    }
}



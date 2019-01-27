﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME {
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D rb2d;
        public Rigidbody2D Rb2d { get { return rb2d; } }

        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private float jumpForce;
        [SerializeField] private float maxSpeed;

        private Animator animator;
        private bool airbornLastFrame = false;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if (rb2d.velocity.x < 0.1f)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                animator.SetBool("idle", true);
            } else
            {
                animator.SetBool("idle", false);
            }

            if (rb2d.velocity.x > maxSpeed)
            {
                rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
            }

            animator.SetBool("airborne", !IsGrounded());
        }

        public void Jump()
        {
            if (!IsGrounded()) { return; }
            rb2d.AddForce(new Vector2(0.0f, jumpForce));
            airbornLastFrame = true;
            animator.SetBool("airborne", true);
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



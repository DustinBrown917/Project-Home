using System.Collections;
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
        [SerializeField] private float normalMaxSpeed;
        [SerializeField] private float dancePadMaxSpeed;
        private float currentMaxSpeed;
        [SerializeField] private float maxSpeedFactor;
        [SerializeField] private float minSpeedFactor;
        private float currentSpeedFactor;

        private Animator animator;
        private bool airbornLastFrame = false;

        private AudioSource audioSource;
        [SerializeField] private AudioClip jumpSound;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (GameOptions.DancePadMode) { currentMaxSpeed = dancePadMaxSpeed; }
            else { currentMaxSpeed = normalMaxSpeed; }
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

            if (rb2d.velocity.x > currentMaxSpeed)
            {
                rb2d.velocity = new Vector2(currentMaxSpeed, rb2d.velocity.y);
            }

            animator.SetBool("airborne", !IsGrounded());

            currentSpeedFactor = Mathf.Lerp(minSpeedFactor, maxSpeedFactor, rb2d.velocity.x / currentMaxSpeed);
            animator.speed = currentSpeedFactor;
        }

        public void Jump()
        {
            if (!IsGrounded()) { return; }
            rb2d.AddForce(new Vector2(0.0f, jumpForce));
            airbornLastFrame = true;
            animator.SetBool("airborne", true);
            audioSource.clip = jumpSound;
            audioSource.Play();
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



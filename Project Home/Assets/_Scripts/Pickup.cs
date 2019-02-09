using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    public class Pickup : MonoBehaviour
    {

        [SerializeField] private float pickupValue;
        [SerializeField] private SpriteRenderer outlineRenderer;
        [SerializeField] private string itemDescription;
        [SerializeField] private AudioClip clip;
        private Rigidbody2D rb2d;

        private Vector3 cachedPosition;

        private bool pickedUp = false;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            pickedUp = false;
            cachedPosition = transform.position;
            cachedPosition.x = 0;
            transform.position = cachedPosition;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (pickedUp) { return; }
            pickedUp = true;
            Player p = collision.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.SetCurrentFunds(p.CurrentFunds + pickupValue);
                UIManager.Instance.BroadCastHighImpact(itemDescription, (pickupValue > 0));
                PickupManager.Instance.PlaySound(pickupValue > 0);
            }

            OnPickedUp();
            PickupManager.PoolPickup(this);
        }

        public event EventHandler PickedUp;

        private void OnPickedUp()
        {
            PickedUp?.Invoke(this, EventArgs.Empty);
        }

    }
}


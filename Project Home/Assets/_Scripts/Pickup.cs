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

        public void SetLayer(int layer)
        {
            gameObject.layer = layer;
            for(int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.layer = layer;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (pickedUp) { return; }
            pickedUp = true;
            Player p = collision.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.SetCurrentFunds(p.CurrentFunds + pickupValue);
                UIManager.Instance.BroadCastHighImpact(p.Index, itemDescription, (pickupValue > 0));
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


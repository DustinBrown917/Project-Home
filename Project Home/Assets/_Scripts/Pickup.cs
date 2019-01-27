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

        private bool pickedUp = false;

        private void Awake()
        {
        }

        private void OnEnable()
        {
            pickedUp = false;
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


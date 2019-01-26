using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    public class Pickup : MonoBehaviour
    {

        [SerializeField] float pickupValue;
        [SerializeField] SpriteRenderer outlineRenderer;

        private void Awake()
        {
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
            Player p = collision.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.SetCurrentFunds(p.CurrentFunds + pickupValue);
            }
        }

    }
}


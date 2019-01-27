using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    public class PickupManager : MonoBehaviour
    {
        private static PickupManager _instance = null;
        public static PickupManager Instance { get { return _instance; } }

        private static List<Pickup> pooledPickups = new List<Pickup>();
        private static List<Pickup> activePickups = new List<Pickup>();


        [SerializeField] private PickupPool pickups;
        private AudioSource audioSource;
        [SerializeField] private AudioClip goodSound;
        [SerializeField] private AudioClip badSound;

        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;

                for(int i = 0; i < 20; i++)
                {
                    PoolPickup(pickups.GetRandomPickup());
                    pooledPickups[i].transform.SetParent(transform);
                }

                audioSource = GetComponent<AudioSource>();

            } else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if(_instance == this)
            {
                _instance = null;
            }
        }

        public static Pickup ActivatePickup()
        {
            if(pooledPickups.Count == 0) { return null; }
            Pickup pu = pooledPickups[0];
            pooledPickups.Remove(pu);
            activePickups.Add(pu);
            pu.gameObject.SetActive(true);

            return pu;
        }

        public static void PoolPickup(Pickup p)
        {
            if(p == null) { return; }
            activePickups.Remove(p);
            pooledPickups.Add(p);
            p.gameObject.SetActive(false);
        }

        public void PlaySound(bool good)
        {
            if (good)
            {
                audioSource.clip = goodSound;
            } else
            {
                audioSource.clip = badSound;
            }

            audioSource.Play();
        }

      
    }


}



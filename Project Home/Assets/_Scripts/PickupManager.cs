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

        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;

                for(int i = 0; i < 10; i++)
                {
                    PoolPickup(pickups.GetRandomPickup());
                    pooledPickups[i].transform.SetParent(transform);
                }

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

        public static Pickup GetPooledPickup()
        {
            Pickup pu = pooledPickups[0];
            pooledPickups.Remove(pu);

            return pu;
        }

        public static void PoolPickup(Pickup p)
        {
            activePickups.Remove(p);
            pooledPickups.Add(p);
            p.gameObject.SetActive(false);
        }

      
    }


}



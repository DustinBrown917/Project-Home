using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    public class SpawnRange : MonoBehaviour
    {
        [SerializeField] private Transform topSpawnRange;
        [SerializeField] private Transform botSpawnRange;
        private Pickup spawnedPickup;

        public void SpawnPickup( int forIndex)
        {
            DeSpawnPickup();
            spawnedPickup = PickupManager.ActivatePickup();
            switch (forIndex)
            {
                case 0:
                    spawnedPickup.SetLayer(LayerMask.NameToLayer("P1Pickups"));
                    break;
                case 1:
                    spawnedPickup.SetLayer(LayerMask.NameToLayer("P2Pickups"));
                    break;
                default:
                    break;
            }
            Vector3 newPos = new Vector3(botSpawnRange.position.x, UnityEngine.Random.Range(botSpawnRange.position.y, topSpawnRange.position.y), botSpawnRange.position.z);
            spawnedPickup.transform.position = newPos;
            spawnedPickup.PickedUp += SpawnedPickup_PickedUp;
        }

        private void SpawnedPickup_PickedUp(object sender, System.EventArgs e)
        {
            spawnedPickup.PickedUp -= SpawnedPickup_PickedUp;
            spawnedPickup = null;
        }

        public void DeSpawnPickup()
        {
            if(spawnedPickup == null) { return; }
            spawnedPickup.PickedUp -= SpawnedPickup_PickedUp;
            PickupManager.PoolPickup(spawnedPickup);
            spawnedPickup = null;
        }
    }
}


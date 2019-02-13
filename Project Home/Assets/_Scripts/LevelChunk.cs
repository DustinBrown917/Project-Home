using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    public class LevelChunk : MonoBehaviour
    { 
        [SerializeField] private Transform chunkGround;
        [SerializeField] private SpawnRange[] spawnRanges;

        [SerializeField] private int playerIndex;

        private bool encountered = false;

        private void Awake()
        {

        }

        private void OnEnable()
        {
            encountered = false;

            for (int i = 0; i < spawnRanges.Length; i++)
            {
                switch (playerIndex)
                {
                    case 0:
                        spawnRanges[i].gameObject.layer = LayerMask.NameToLayer("P1Pickups");
                        break;
                    case 1:
                        spawnRanges[i].gameObject.layer = LayerMask.NameToLayer("P2Pickups");
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!encountered) { encountered = true; }
            else { return; }
            
            Player p = other.gameObject.GetComponent<Player>();

            if(p == null) { return; }
            
            if (p.Index != playerIndex) { return; }
            Level.HandleNewChunkEntered(this, playerIndex);
            
        }

        public void SetPlayerIndex(int i) {
            playerIndex = i;
        }

        public void SpawnPickups(int playerIndex)
        {
            for (int i = 0; i < spawnRanges.Length; i++)
            {
                if(UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f)
                {
                    spawnRanges[i].SpawnPickup(playerIndex);
                }
            }
        }

        public void PoolPickups()
        {
            for (int i = 0; i < spawnRanges.Length; i++)
            {
                spawnRanges[i].DeSpawnPickup();
            }
        }


    }
}


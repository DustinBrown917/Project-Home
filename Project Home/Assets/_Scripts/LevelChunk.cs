using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    public class LevelChunk : MonoBehaviour
    { 
        [SerializeField] private Transform chunkGround;
        [SerializeField] private SpawnRange[] spawnRanges;

        private bool encountered = false;

        private void Awake()
        {

        }

        private void OnEnable()
        {
            encountered = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!encountered) { encountered = true; }
            else { return; }
            Level.HandleNewChunkEntered(this);
        }

        public void SpawnPickups()
        {
            for (int i = 0; i < spawnRanges.Length; i++)
            {
                if(UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f)
                {
                    spawnRanges[i].SpawnPickup();
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    public class Level : MonoBehaviour
    {
        private static Level _instance = null;
        public static Level Instance { get { return _instance; } }


        private static List<LevelChunk> levelChunks = new List<LevelChunk>();
        private static List<LevelChunk> inactiveLevelChunks = new List<LevelChunk>();
        private static float chunkWidth = 16.0f;

        [SerializeField] private GameObject levelChunkPrefab;
        [SerializeField] private LevelChunk initialChunk;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            for (int i = 0; i < 10; i++)
            {
                LevelChunk lc = Instantiate(levelChunkPrefab, Level.Instance.transform).GetComponent<LevelChunk>();
                lc.gameObject.SetActive(false);
                inactiveLevelChunks.Add(lc);
            }
            levelChunks.Add(initialChunk);
        }

        private void Start()
        {
            GameManager.Instance.RunReset += GameManager_RunReset;
        }

        private void GameManager_RunReset(object sender, System.EventArgs e)
        {
            ResetRun();
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        public static void HandleNewChunkEntered(LevelChunk chunk)
        {
            if (levelChunks.Count >= 3)
            {
                if (levelChunks[2] == chunk)
                {
                    levelChunks[0].PoolPickups();
                    levelChunks[0].gameObject.SetActive(false);
                    inactiveLevelChunks.Add(levelChunks[0]);
                    levelChunks.RemoveAt(0);
                }
            }


            if (levelChunks.Count < 4)
            {
                for (int i = 0; i < 4 - levelChunks.Count; i++)
                {
                    PlacePooledChunk();
                }
            }
        }

        public static void PlacePooledChunk()
        {
            LevelChunk lc = inactiveLevelChunks[0];
            inactiveLevelChunks.Remove(lc);
            lc.gameObject.SetActive(true);
            Vector3 newPos = levelChunks[levelChunks.Count - 1].gameObject.transform.position;
            levelChunks.Add(lc);
            newPos.x += chunkWidth;
            lc.transform.position = newPos;
            lc.SpawnPickups();
        }

        public void ResetRun()
        {
            for(int i = 0; i < levelChunks.Count; i++)
            {
                levelChunks[i].transform.position = new Vector3(i * chunkWidth, levelChunks[i].transform.position.y, levelChunks[i].transform.position.z);
            }
        }
    }
}


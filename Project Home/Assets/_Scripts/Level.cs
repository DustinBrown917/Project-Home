using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    public class Level : MonoBehaviour
    {
        private static Level _instance = null;
        public static Level Instance { get { return _instance; } }

        private static List<LevelChunk>[] levelChunksLists;
        private static List<LevelChunk> inactiveLevelChunks = new List<LevelChunk>();
        private static float chunkWidth = 16.0f;

        [SerializeField] private GameObject levelChunkPrefab;
        [SerializeField] private LevelChunk[] initialChunks;

        private void Awake()
        {
            if (_instance == null) {  _instance = this; }
            else { Destroy(gameObject); }
        }

        private void Start()
        {
            levelChunksLists = new List<LevelChunk>[PlayerManager.PlayerCount];
            for (int i = 0; i < levelChunksLists.Length; i++) {
                levelChunksLists[i] = new List<LevelChunk>();
            }

            GameManager.Instance.RunReset += GameManager_RunReset;

            for (int i = 0; i < 10; i++) {
                LevelChunk lc = Instantiate(levelChunkPrefab, Level.Instance.transform).GetComponent<LevelChunk>();
                lc.gameObject.SetActive(false);
                inactiveLevelChunks.Add(lc);
            }
            for (int i = 0; i < levelChunksLists.Length; i++)
            {
                
                levelChunksLists[i].Add(initialChunks[i]);
                levelChunksLists[i][0].SetPlayerIndex(i);
            }
        }

        private void GameManager_RunReset(object sender, GameManager.RunResetArgs e)
        {
            ResetRun(e.playerIndex);
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        public static void HandleNewChunkEntered(LevelChunk chunk, int playerIndex)
        {
            
            if (levelChunksLists[playerIndex].Count >= 3)
            {
                if (levelChunksLists[playerIndex][2] == chunk)
                {
                    levelChunksLists[playerIndex][0].PoolPickups();
                    levelChunksLists[playerIndex][0].gameObject.SetActive(false);
                    inactiveLevelChunks.Add(levelChunksLists[playerIndex][0]);
                    levelChunksLists[playerIndex].RemoveAt(0);
                }
            }


            if (levelChunksLists[playerIndex].Count < 4)
            {
                for (int i = 0; i < 4 - levelChunksLists[playerIndex].Count; i++)
                {
                    PlacePooledChunk(playerIndex);
                }
            }
        }

        public static void PlacePooledChunk(int playerIndex)
        {
            LevelChunk lc = inactiveLevelChunks[0];
            inactiveLevelChunks.Remove(lc);
            lc.SetPlayerIndex(playerIndex);
            switch (playerIndex) {
                case 0:
                    lc.gameObject.layer = LayerMask.NameToLayer("P1LevelChunk");
                    lc.gameObject.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("P1LevelChunk");
                    break;
                case 1:
                    lc.gameObject.layer = LayerMask.NameToLayer("P2LevelChunk");
                    lc.gameObject.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("P2LevelChunk");
                    break;
                default:
                    break;
            }

            lc.gameObject.SetActive(true);
            Vector3 newPos = levelChunksLists[playerIndex][levelChunksLists[playerIndex].Count - 1].gameObject.transform.position;
            levelChunksLists[playerIndex].Add(lc);
            newPos.x += chunkWidth;
            lc.transform.position = newPos;
            lc.SpawnPickups(playerIndex);
        }

        public void ResetRun(int playerIndex)
        {
            for(int i = 0; i < levelChunksLists[playerIndex].Count; i++)
            {
                levelChunksLists[playerIndex][i].transform.position = new Vector3(i * chunkWidth, levelChunksLists[playerIndex][i].transform.position.y, levelChunksLists[playerIndex][i].transform.position.z);
            }
        }
    }
}


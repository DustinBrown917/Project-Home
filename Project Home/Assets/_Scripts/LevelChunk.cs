using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    public class LevelChunk : MonoBehaviour
    {
        private static List<LevelChunk> levelChunks = new List<LevelChunk>();
        private static List<LevelChunk> inactiveLevelChunks = new List<LevelChunk>();
        private static float chunkWidth = 20.0f;

        [SerializeField] private GameObject levelChunkPrefab;
        [SerializeField] private Transform chunkGround;
        private void Awake()
        {
            if (inactiveLevelChunks.Count == 0)
            {
                inactiveLevelChunks.Add(this);
                for (int i = 0; i < 10; i++)
                {
                    LevelChunk lc = Instantiate(levelChunkPrefab, Level.Instance.transform).GetComponent<LevelChunk>();
                    lc.gameObject.SetActive(false);
                    inactiveLevelChunks.Add(lc);
                }
                inactiveLevelChunks.Remove(this);
                levelChunks.Add(this);
            }

        }

        private void OnEnable()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            chunkGround.localScale = new Vector3(chunkWidth, 1.0f, 1.0f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (levelChunks.Count >= 3)
            {
                if (levelChunks[2] == this)
                {
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
                    Debug.Log("Chunk placed");
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
        }
    }
}


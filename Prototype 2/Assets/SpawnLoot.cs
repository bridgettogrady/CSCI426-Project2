using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLoot : MonoBehaviour
{
    public GameObject loot;
    public int spawnCount;
    public float minDistance = 10f;
    public Vector2 spawnAreaMin = new Vector2(-50f, -50f);
    public Vector2 spawnAreaMax = new Vector2(50f, 50f);
    private bool noRespawn = false;
    public Movement Player1;
    public Movement Player2;


    private List<Vector2> spawnedPositions = new List<Vector2>();
    private List<GameObject> allLoot = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnCount; i++) {
            Spawn();
        }
    }

    void Update() {
        // spawn every time either player finds an object

        if (Input.GetKey(KeyCode.Space))
        {
            Respawn();
        }
    }

    private void Spawn() {
        bool validPos = false;
        Vector2 spawnPosition = Vector2.zero;

        while (!validPos) {
            spawnPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            validPos = true;
            foreach (Vector2 pos in spawnedPositions) {
                if (Vector2.Distance(pos, spawnPosition) < minDistance) {
                    validPos = false;
                    break;
                }
            }

            if (validPos) {
                GameObject lootInstance = Instantiate(loot, spawnPosition, Quaternion.identity);
                spawnedPositions.Add(spawnPosition);
                allLoot.Add(lootInstance);
                break;
            }
        }
    }

    public void Respawn() {
        if (!noRespawn && Player1.score < 3 && Player2.score < 3) {
            // start coroutine
            StartCoroutine(NoRespawn());

            // delete everything in spawned positions
            foreach (GameObject thisLoot in allLoot) {
                Destroy(thisLoot);
            }
            allLoot.Clear();

            // clear list
            spawnedPositions.Clear();

            // respawn
            for (int i = 0; i < spawnCount; i++) {

                Player1.ScoreUpdate();
                Player2.ScoreUpdate();

                Spawn();
            }
            
        }
    }

    public List<Vector2> GetLootPos() {
        return spawnedPositions;
    }

    public IEnumerator NoRespawn() {
        noRespawn = true;
        yield return new WaitForSeconds(1f);
        noRespawn = false;
    }
}

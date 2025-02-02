using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public bool leftPlayer = true;
    public Vector2 startingPos;
    public SpawnLoot spawnLoot;
    public float epsilon = 0.2f;
    public float waitingTime = 2f;
    private List<KeyCode> WASD = new List<KeyCode>{KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D};
    private List<KeyCode> arrows = new List<KeyCode>{KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow};
    private List<KeyCode> activeKeys;

    //FIXME
    private int i = 0;

    void Start() {
        if (leftPlayer) {
            activeKeys = WASD;
        }
        else {
            activeKeys = arrows;
        }

        transform.position = startingPos;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;

        // get input based on left or right player
        if (Input.GetKey(activeKeys[0])) {
            move += Vector3.up;
        }
        if (Input.GetKey(activeKeys[1])) {
            move += Vector3.left;
        }
        if (Input.GetKey(activeKeys[2])) {
            move += Vector3.down;
        }
        if (Input.GetKey(activeKeys[3])) {
            move += Vector3.right;
        }
        
        move = move.normalized * speed * Time.deltaTime;
        transform.position += move;

        // update close to loot or not
        List<Vector2> loot = spawnLoot.GetLootPos();
        foreach (Vector2 pos in loot) {
            if (Vector2.Distance(transform.position, pos) < epsilon) {
                StartCoroutine(Wait(pos));
            }
        }
    }

    private IEnumerator Wait(Vector2 lootPos) {
        if (Vector2.Distance(transform.position, lootPos) >= epsilon) {
            yield break;
        }

        yield return new WaitForSeconds(waitingTime);

        spawnLoot.Respawn();
        Respawn();
    }

    private void Respawn() {
        transform.position = startingPos;
    }
}


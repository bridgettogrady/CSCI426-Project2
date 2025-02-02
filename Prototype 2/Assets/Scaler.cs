using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    public SpawnLoot spawnLoot;
    public Movement movement;
    private float distanceAway;
    public float epsilon = 0.2f;
    public float waitingTime = 2f;
    public Vector2 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startingPos;
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector2> loot = spawnLoot.GetLootPos();
        foreach (Vector2 pos in loot)
        {
            // transform.localScale = new Vector3(1.5f-(distanceAway/18), 1.5f-(distanceAway/18), 1f);
            distanceAway = Vector3.Distance(transform.position, pos);
            transform.localScale = new Vector3(2.2f - (distanceAway / 18), 2.2f - (distanceAway / 18), 1f);
            if (Vector2.Distance(transform.position, pos) < epsilon)
            {
                StartCoroutine(Wait(pos));
            }
        }
    }

    private IEnumerator Wait(Vector2 lootPos)
    {
        if (Vector2.Distance(transform.position, lootPos) >= epsilon)
        {
            yield break;
        }

        yield return new WaitForSeconds(waitingTime);

        spawnLoot.Respawn();
        Respawn();
    }
    public void Respawn()
    {
        transform.position = startingPos;
    }

}

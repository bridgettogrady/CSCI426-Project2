using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public bool leftPlayer = true;
    public Vector2 startingPos;
    public SpawnLoot spawnLoot;
    public Movement OtherPlayer;
    public float epsilon = 0.2f;
    public float waitingTime = 1f;
    public float minRadius = 0.5f;
    private List<KeyCode> WASD = new List<KeyCode>{KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D};
    private List<KeyCode> arrows = new List<KeyCode>{KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow};
    private List<KeyCode> activeKeys;
    private Vector2 scaleChange;
    private float distanceAway;
    public Text winText;
    public Text scoreText;
    public int score = 0;
    private bool justScored = false;
    public bool won = false;
    public float flashDuration = 0.1f;
    AudioSource audio;

    //FIXME
    private int i = 0;

    void Start() {

        audio = GetComponent<AudioSource>();

        winText.text = "";

        if (leftPlayer) {
            activeKeys = WASD;
            scoreText.text = "P1 " + score + "/3";
        }
        else {
            activeKeys = arrows;
            scoreText.text = "P2 " + score + "/3";
        }

        transform.position = startingPos;
        //Transform indicatorSize = GetComponentInChildren<Transform>(transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (score >= 3)
        {
            if (leftPlayer)
            {
                won = true;
                winText.text = "P1 Wins!";
            }
            else
            {
                won = true;
                winText.text = "P2 Wins!";
            }
        }

        
        
        justScored = false;
        
        Vector3 move = Vector3.zero;

        // get input based on left or right player

        if (score < 3 || OtherPlayer.won == false)
        {
            if (Input.GetKey(activeKeys[0]))
            {
                move += Vector3.up;
            }
            if (Input.GetKey(activeKeys[1]))
            {
                move += Vector3.left;
            }
            if (Input.GetKey(activeKeys[2]))
            {
                move += Vector3.down;
            }
            if (Input.GetKey(activeKeys[3]))
            {
                move += Vector3.right;
            }
        }
            
        
        move = move.normalized * speed * Time.deltaTime;
        transform.position += move;

        // update close to loot or not
        List<Vector2> loot = spawnLoot.GetLootPos();
        foreach (Vector2 pos in loot) {
            
            distanceAway = Vector2.Distance(transform.position, pos);
            float newRadius = 1.9f - (1.9f *(distanceAway / 18));
            float clampedRadius = Mathf.Clamp(newRadius, minRadius, newRadius);
            transform.localScale = new Vector3(clampedRadius, clampedRadius, 1.0f);

            if (Vector2.Distance(transform.position, pos) < epsilon) {
                justScored = true;
                StartCoroutine(Wait(pos));
            }
            
        }
    }

    private IEnumerator Wait(Vector2 lootPos) {

        if (Vector2.Distance(transform.position, lootPos) >= epsilon) {
            yield break;
        }

        if (score < 3) {
            audio.Play();
        }
        yield return new WaitForSeconds(waitingTime);

        if (score < 3)
        {
            spawnLoot.Respawn();
            Respawn();
        }
    }

    private void RespawnPosition() {

        transform.position = startingPos;
    }

    public static void Respawn() {
        Movement[] allMovement = FindObjectsOfType<Movement>();
        foreach(Movement m in allMovement) {
            m.RespawnPosition();
        }
    }

    public void ScoreUpdate()
    {
        if (justScored == true) {
            if (leftPlayer)
            {
                score = score + 1;
                scoreText.text = "P1 " + score + "/3";
                justScored = false;
            }
            else
            {
                score = score + 1;
                scoreText.text = "P2 " + score + "/3";
                justScored = false;
            }
        }
        else
        {
            justScored = false;
        }
    }

}


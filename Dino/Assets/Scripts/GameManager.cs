using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    private GameObject player;
    private Player playerScript;
    public GameObject[] roads;
    public GameObject dummyObject;
    public GameObject[] cacti;
    public Text gameOverText;
    public Text scoreText;
    public float gameOverDelay = 2.0f;
    private float gameOverTime = 0;
    private LinkedList<GameObject> spawnedRoads;
    private LinkedList<GameObject> spawnedCacti;
    private static float roadLength = 4.78f;
    private static float minCactiDistance = 0.5f * roadLength;
    private static float maxCactiDistance = 4 * roadLength;
    

    // Use this for initialization
    void Start () {
        if (instance == null) {
            instance = this;
        } else if (instance != this){
            throw new Exception("wtf?!");
        }
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        gameOverText.enabled = false;
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        player = GameObject.Find("Kangaroo");
        playerScript = player.GetComponent<Player>();
        int randomIndex = Random.Range(0, roads.Length);
        spawnedRoads = new LinkedList<GameObject>();
        spawnedRoads.AddLast(GameObject.Instantiate(roads[randomIndex], new Vector3(-2*roadLength, -0.9f), Quaternion.identity) as GameObject);
        
        spawnRoads();
        spawnedCacti = new LinkedList<GameObject>();
        randomIndex = Random.Range(0, cacti.Length);
        spawnedCacti.AddLast(GameObject.Instantiate(cacti[randomIndex], new Vector3(15, 0), Quaternion.identity) as GameObject);
        spawnedCacti.Last.Value.transform.parent = dummyObject.transform;
        spawnCacti();
	}
	
    public void updatePlayerScore() {
        scoreText.text = "score: " + player.transform.position.x;
    }

    public void gameOver() {
        gameOverTime = Time.realtimeSinceStartup;
        gameOverText.text = "Game Over!\n Total score: " + player.transform.position.x;
        gameOverText.enabled = true;
    }

    void spawnRoads() {
        int size = roads.Length;

        float lastRoadPosition = spawnedRoads.Last.Value.transform.position.x;

        int randomIndex;
        for (int i = 0; i < size; i++) {
            randomIndex = Random.Range(0, size);
            if (spawnedRoads.Count > 12) {
                Destroy(spawnedRoads.First.Value);
                spawnedRoads.RemoveFirst();
            }
            spawnedRoads.AddLast(GameObject.Instantiate(roads[randomIndex], new Vector3(lastRoadPosition + (i + 1) * roadLength, -0.9f),Quaternion.identity) as GameObject) ;
            
        }
    }

    void spawnCacti() {
        int size = cacti.Length;

        float lastCactusPosition = spawnedCacti.Last.Value.transform.position.x;

        int randomIndex;
        float randomDistance;
        for (int i = 0; i < size; i++) {
            randomIndex = Random.Range(0, size);
            if (spawnedCacti.Count > 12) {
                Destroy(spawnedCacti.First.Value);
                spawnedCacti.RemoveFirst();
            }
            randomDistance = Random.Range(minCactiDistance, maxCactiDistance);
            spawnedCacti.AddLast(GameObject.Instantiate(cacti[randomIndex], new Vector3(lastCactusPosition + randomDistance, 0), Quaternion.identity) as GameObject);
            spawnedCacti.Last.Value.transform.parent = dummyObject.transform;
            
            lastCactusPosition += randomDistance;
        }

    }

	// Update is called once per frame
	void Update () {
        minCactiDistance = playerScript.getJumpDistance();
        maxCactiDistance = 3 * minCactiDistance;
        if (gameOverTime > 0 && Time.realtimeSinceStartup - gameOverTime > gameOverDelay && Input.anyKey) {
            Application.LoadLevel(Application.loadedLevel);
        }
        if (spawnedRoads.Last.Value.transform.position.x - player.transform.position.x < 4 * roadLength) {
            spawnRoads();
        }
        if (spawnedCacti.Last.Value.transform.position.x - player.transform.position.x < maxCactiDistance) {
            spawnCacti();
        }
        updatePlayerScore();
	}
}

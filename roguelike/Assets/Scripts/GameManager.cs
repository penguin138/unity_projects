using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;
	private int currentLevel;
	private BoardManager boardManager;
	private int playerFood;
	private bool playersTurn;
	private bool enemiesMoving;
	private List<Enemy> enemies;
	private float turnDelay = 0.1f;
	private Text levelText;
	private GameObject levelImage;
	private Text foodText;
	private bool doingSetup;
	private float setupDelay = 2f;


	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			throw new Exception("wtf?!!");
		}
		DontDestroyOnLoad(gameObject);
		currentLevel = 1;
		playerFood = 35;
		boardManager = GetComponent<BoardManager>();
		init ();
	}

	void Update() {
		if (playersTurn || enemiesMoving || doingSetup) {
			return;
		}
		StartCoroutine(moveEnemies());

	}

	public bool isDoingSetup() {
		return doingSetup;
	}
	private void OnLevelWasLoaded(int index) {
		init();
	}
	private void init() {
		doingSetup = true;
		playersTurn = true;
		enemiesMoving = false;
		enemies =  new List<Enemy>();
		levelImage = GameObject.Find("LevelImage");
		levelText = GameObject.Find("LevelText").GetComponent<Text>();
		foodText = GameObject.Find("FoodText").GetComponent<Text>();
		levelText.text = "Day " + currentLevel;
		levelImage.SetActive(true);
		boardManager.createBoard(currentLevel);
		Invoke ("hideLevelImage", setupDelay);
	}
	void hideLevelImage() {
		levelImage.SetActive(false);
		doingSetup = false;
	}

	public int getPlayerFood() {
		return playerFood;
	}
	public void updatePlayerFood(int food) {
		playerFood+=food;
		foodText.text = "Food: " + playerFood + (food < 0 ? " -" : " +") + Math.Abs(food);
		checkIfGameOver();
	}

	public void loadNewLevel() {
		currentLevel++;
		Application.LoadLevel(Application.loadedLevel);
	}
	public void changeTurn() {
		playersTurn = !playersTurn;
	}
	void checkIfGameOver() {
		if (playerFood <= 0) {
			gameOver();
		}
	}
	void gameOver() {
		levelText.text = "After " + currentLevel + " days you died.";
		levelImage.SetActive(true);
		currentLevel = 0;
		playerFood = 35;
		Invoke("loadNewLevel", setupDelay);
	}
	public void addToEnemies(Enemy enemy) {
		enemies.Add(enemy);
	}
	public bool isPlayersTurn() {
		return playersTurn;
	}
	IEnumerator moveEnemies() {
		enemiesMoving = true;
		yield return new WaitForSeconds(turnDelay);
		if (enemies.Count == 0) {
			yield return new WaitForSeconds(turnDelay);
		}
		for (int i = 0; i < enemies.Count; i++) {
			enemies[i].moveEnemy();
			yield return new WaitForSeconds(enemies[i].moveTime);
		}
		playersTurn = true;
		enemiesMoving = false;
	}
}

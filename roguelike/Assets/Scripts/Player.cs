using UnityEngine;
using System.Collections;

public class Player : MovingObject {
	private Animator animator;
	public int wallDamage = 1;
	public int pointsPerFood = 20;
	public int pointsPerSoda = 10;
	private Vector2 touchOrigin = new Vector2(0,-1);

	// Use this for initialization
	public override void Start () {
		base.Start();
		animator = GetComponent<Animator>();
	}
	

	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.isPlayersTurn() || GameManager.instance.isDoingSetup()) {
			return;
		}
		Vector2 coordinateChange = getCoordinatesChange();
		int xChange = (int) coordinateChange.x;
		int yChange = (int) coordinateChange.y;

		if (xChange != 0 && yChange != 0) {
			yChange = 0;
		}
		if (xChange != 0 || yChange != 0) {
			attemptMove<Wall>(xChange, yChange);
		}
	}
	private Vector2 getCoordinatesChange() {
		int xChange = 0;
		int yChange = 0;
#if 	UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
		xChange = (int) Input.GetAxisRaw("Horizontal");
		yChange = (int) Input.GetAxisRaw("Vertical");

#else		
		if (Input.touchCount > 0 ) {
			Touch newTouch = Input.touches[0];
			if (newTouch.phase == TouchPhase.Began) {
				touchOrigin = newTouch.position;
			} else if (newTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {
				Vector2 touchEnd = newTouch.position;
				float x = touchEnd.x - touchOrigin.x;
				float y = touchEnd.y - touchOrigin.y;
				touchOrigin.x = -1;
				if (Mathf.Abs(x) > Mathf.Abs(y)) {
					xChange = x > 0 ? 1 : -1;
				} else {
					yChange = y > 0 ? 1 : -1;
				}
			}
		}
#endif
		return new Vector2(xChange, yChange);
	}
	public override void attemptMove<T>(int x, int y) { 
		GameManager.instance.updatePlayerFood(-1);
		base.attemptMove<T>(x,y);
		GameManager.instance.changeTurn();
	}

	private void OnTriggerEnter2D(Collider2D item) {
		if (item.tag == "Exit") {
			gameObject.SetActive(false);
			GameManager.instance.loadNewLevel();
		} else if (item.tag == "Food") {
			GameManager.instance.updatePlayerFood(pointsPerFood);
			item.gameObject.SetActive(false);
		} else if (item.tag == "Soda") {
			GameManager.instance.updatePlayerFood(pointsPerSoda);
			item.gameObject.SetActive(false);
		}
	}

	protected override void onCantMove<T> (T hitObject) {
		Wall hitWall = hitObject as Wall;
		if (hitWall == null) {
			return;
		}
		animator.SetTrigger("Player_Chop");
		hitWall.damageWall(wallDamage); 
	}



	public void damagePlayer(int loss) {
		animator.SetTrigger("Player_Hit");
		GameManager.instance.updatePlayerFood(-loss);
	}
}

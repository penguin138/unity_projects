using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public Sprite damagedSprite;
	private SpriteRenderer spriteRenderer;
	public int hp = 4;

	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void damageWall(int damage) {
		spriteRenderer.sprite = damagedSprite;
		hp-=damage;
		if (hp <= 0) {
			gameObject.SetActive(false);
		}
	}
}

using UnityEngine;
using System.Collections;

public class Enemy : MovingObject {

	private Player target;
	private Animator animator;
	public int enemyDamage;
	public float moveTime = 0.1f;

	public override void Start ()
	{
		base.Start ();
		GameManager.instance.addToEnemies(this);
		animator = GetComponent<Animator>();
		target = GameObject.FindWithTag("Player").GetComponent<Player>();
	}

	// Use this for initialization
	protected override void onCantMove<T> (T hitObject)
	{	Player player = hitObject as Player;
		if (player == null) {
			return;
		}
		animator.SetTrigger("Enemy_Hit");
		player.damagePlayer(enemyDamage);
	}

	public void moveEnemy() {
		Vector2 enemyPosition = transform.position;
		Vector2 targetPosition = target.transform.position;
		int x = 0;
		int y = 0;
		if (Mathf.Abs(enemyPosition.x - targetPosition.x) > float.Epsilon) {
			x = enemyPosition.x > targetPosition.x ? -1 : 1;
		} else {
			y = enemyPosition.y > targetPosition.y ? -1 : 1;
		}
		attemptMove<Player>(x, y);
	}
}

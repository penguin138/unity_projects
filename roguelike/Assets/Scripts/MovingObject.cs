using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {
	public float velocity = 10;
	public LayerMask blockingLayer;
	private static int count = 0;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rigidBody;

	public virtual void Start() {
		boxCollider = GetComponent<BoxCollider2D>();
		rigidBody = GetComponent<Rigidbody2D>();
	}
	private IEnumerator moveAnimation(int x, int y) {
		Vector3 currentPosition = rigidBody.position;
		Vector3 destination = new Vector3(currentPosition.x + x, currentPosition.y + y, currentPosition.z);
		count++;
		float remainingDistance  = (destination - currentPosition).sqrMagnitude;
		while(remainingDistance > float.Epsilon) {
			currentPosition = Vector3.MoveTowards(rigidBody.position, destination, velocity*Time.deltaTime);
			rigidBody.position = currentPosition;
			remainingDistance = (destination - currentPosition).sqrMagnitude;
			yield return null;
		}
	}

	public virtual void attemptMove<T>(int x, int y) where T : Component {
		Vector2 currentPosition = transform.position;
		Vector2 destination = new Vector2(currentPosition.x + x, currentPosition.y + y);
		boxCollider.enabled = false;
		RaycastHit2D hitObject = Physics2D.Linecast(currentPosition, destination, blockingLayer);
		boxCollider.enabled = true;
		if (hitObject.transform == null) {
			StartCoroutine(moveAnimation(x,y));
		} else {
		onCantMove<T>(hitObject.transform.GetComponent<T>());
		}
	}
	protected abstract void onCantMove<T>(T hitObject) where T:Component;
}


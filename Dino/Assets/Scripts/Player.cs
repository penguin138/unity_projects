using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private Animator animator;
	private Rigidbody2D rigidBody;
	bool inAir = false;
    bool gameOver = false;
	GameObject mainCamera;

    public float baseVelocity = 10;
    private float velocity;
	public Vector2 baseJumpVelocity;
    private Vector2 jumpVelocity;
	public Vector2 g = new Vector2(0,-9.8f);
    public PolygonCollider2D colliderIdle;
    public PolygonCollider2D colliderJump;
    private float startTime;

	// Use this for initialization
	void Start () {
        startTime = Time.realtimeSinceStartup;
        velocity = baseVelocity;
        jumpVelocity = baseJumpVelocity;
        colliderJump.enabled = false;
        colliderIdle.enabled = true;
		animator = gameObject.GetComponent<Animator>();
		rigidBody = gameObject.GetComponent<Rigidbody2D>();
		mainCamera = GameObject.Find("Main Camera");
	}

	void Move() {
		//Vector2 position = transform.position;
		//rigidBody.MovePosition(position + new Vector2(velocity*Time.deltaTime,0));
		rigidBody.velocity = new Vector2(velocity,0);
	}

    IEnumerator Jump() {
        animator.SetTrigger("inAir");
        inAir = true;
        colliderIdle.enabled = false;
        colliderJump.enabled = true;
        float start = (float)Time.realtimeSinceStartup;
        rigidBody.velocity = jumpVelocity;
        while (transform.position.y > 0 && !gameOver) {
            float currentTime = (float)Time.realtimeSinceStartup;
            float deltaTime = currentTime - start;
            //rigidBody.MovePosition((Vector2)transform.position + jumpVelocity*deltaTime + g*deltaTime*deltaTime/2);
            rigidBody.velocity = jumpVelocity + g * deltaTime;
            yield return null;
        }
        if (!gameOver) {
            transform.position = new Vector2(transform.position.x, 0);
            animator.ResetTrigger("inAir");
            inAir = false;
            colliderJump.enabled = false;
            colliderIdle.enabled = true;
        } else {
            rigidBody.velocity = Vector2.zero;
        }
	}

	private void OnTriggerEnter2D(Collider2D item) {
        if (item.tag == "Cactus") {
            gameOver = true;
            rigidBody.isKinematic = true;
            rigidBody.velocity = new Vector2(0,0);
            GameManager.instance.gameOver();
        }
            
	}

    void FixedUpdate() {
        float dt = Time.realtimeSinceStartup - startTime;
        velocity = baseVelocity * Mathf.Log(dt / 10 + Mathf.Exp(1));
        jumpVelocity.x = baseJumpVelocity.x * Mathf.Log(dt / 10 + Mathf.Exp(1));
    }

	// Update is called once per frame
	void Update () {
        if (!gameOver) {
            Move();
            int jump = 0;
            if (Input.anyKey || Input.touchCount > 0) {
                jump = 1;
            }
            if (jump > 0 && !inAir) {
                StartCoroutine("Jump");
            }
        }
	}
	void LateUpdate() {
		mainCamera.transform.position = new Vector3(transform.position.x + 4, mainCamera.transform.position.y,-1);
	}

    public float getJumpDistance() {
        return 2 * jumpVelocity.x * jumpVelocity.y / g.magnitude;
    }
}

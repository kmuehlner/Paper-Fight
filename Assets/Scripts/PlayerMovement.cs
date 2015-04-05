using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public static int FOREGROUND_LAYER = LayerMask.NameToLayer("Foreground");
	public static int BACKGROUND_LAYER = LayerMask.NameToLayer("Background");

	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.

	private float initialY;

	// Use this for initialization
	void Awake ()
	{
		initialY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float v = Input.GetAxisRaw ("Vertical");
		int layer = gameObject.layer;

		if (v == 1 && layer == FOREGROUND_LAYER) {
			gameObject.layer = BACKGROUND_LAYER;
			Vector3 newPosition = new Vector3(transform.position.x, initialY, 0);
			transform.position = newPosition;
			GetComponent<SpriteRenderer>().sortingLayerName = "Background";
		} else if (v == -1 && layer == BACKGROUND_LAYER) {
			gameObject.layer = FOREGROUND_LAYER;
			GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
		}
	}

	void FixedUpdate()
	{
		float h = Input.GetAxis ("Horizontal");

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
			// ... add a force to the player.
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
	}
}

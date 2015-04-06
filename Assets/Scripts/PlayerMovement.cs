using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public static int FOREGROUND_LAYER = LayerMask.NameToLayer("Foreground");
	public static int BACKGROUND_LAYER = LayerMask.NameToLayer("Background");

	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public float enterStageSpeed = 1f;

	//private float initialY;
	private Vector2 destinationPoint;
	private int destinationLayer;
	private bool movingOffstage;
	private bool movingOnstage;
	private bool offstage;

	// Use this for initialization
	void Awake ()
	{
		//initialY = transform.position.y;
	}

	public bool Offstage()
	{
		return offstage;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (offstage) {
			MoveOnstage ();
		}
	}


	// TODO: lots of cleanup
	void MoveOnstage()
	{
		if (!movingOnstage) {
			movingOnstage = true;
			Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
			RaycastHit2D hit = Physics2D.Raycast(curPos, Vector2.up, 100f, LayerMask.GetMask(LayerMask.LayerToName(destinationLayer)));
			//Debug.Log (hit.collider.name);
			destinationPoint = hit.point;
			gameObject.layer = destinationLayer;
			if (gameObject.layer == FOREGROUND_LAYER) {
				GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
			} else if (gameObject.layer == BACKGROUND_LAYER) {
				GetComponent<SpriteRenderer>().sortingLayerName = "Background";
			}
		}
		Vector3 target = new Vector3(destinationPoint.x, destinationPoint.y + 2.5f, 0f);
		if (Mathf.Abs (transform.position.y - target.y) > Mathf.Epsilon) {
			transform.position = Vector3.MoveTowards (transform.position, target, enterStageSpeed * Time.deltaTime); 
		} else {
			offstage = false;
			movingOnstage = false;
			CircleCollider2D[] feetColliders = GetComponents<CircleCollider2D>();
			for (int i = 0; i < feetColliders.Length; i++) {
				feetColliders[i].enabled = true;
			}
			GetComponent<BoxCollider2D>().enabled = true;
			GetComponent<Rigidbody2D>().isKinematic = false;
			GetComponent<Rigidbody2D>().fixedAngle = false;
		}
	}

	bool LayerAvailable(float v) {
		if (gameObject.layer == FOREGROUND_LAYER && v == 1.0f)
			return true;
		else if (gameObject.layer == BACKGROUND_LAYER && v == -1.0f)
			return true;
		return false;
	}

	void SetDestinationLayer(float v) {
		if (gameObject.layer == FOREGROUND_LAYER && v == 1.0f)
			destinationLayer = BACKGROUND_LAYER;
		else if (gameObject.layer == BACKGROUND_LAYER && v == -1.0f)
			destinationLayer = FOREGROUND_LAYER;
	}

	void FixedUpdate()
	{
		float v = Input.GetAxisRaw ("Vertical");

		// input requesting moving forward one layer, // and we're not offstage or in the process of moving offstage between layers
		if (v == -1 || v == 1 && !offstage && !movingOffstage && !movingOnstage) { 

			if (LayerAvailable(v)) {
				SetDestinationLayer(v);
				movingOffstage = true;
				CircleCollider2D[] feetColliders = GetComponents<CircleCollider2D> ();
				for (int i = 0; i < feetColliders.Length; i++) {
					feetColliders [i].enabled = false;
				}
				GetComponent<BoxCollider2D>().enabled = false;
				GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
				GetComponent<Rigidbody2D>().rotation = 0f;
			}
		}
		// moving offstage current in process
		if (movingOffstage) {
			if (!GetComponent<SpriteRenderer>().isVisible) {
				movingOffstage = false;
				offstage = true;
				GetComponent<Rigidbody2D>().isKinematic = true;
				transform.rotation = Quaternion.Euler(Vector3.up);
				GetComponent<Rigidbody2D>().fixedAngle = true;
			}
		}
		else {
			float h = Input.GetAxis ("Horizontal");

			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if(h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
				// ... add a force to the player.
				GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);				
		}
	}
}

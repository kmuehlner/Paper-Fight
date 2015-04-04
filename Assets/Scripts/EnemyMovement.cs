using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	public static int FOREGROUND_LAYER = LayerMask.NameToLayer("Foreground");
	public static int BACKGROUND_LAYER = LayerMask.NameToLayer("Background");

	public GameObject player;
	public float waitTimeBeforeLayerChange = 2f;

	private float initialY;
	private float timeOffPlayerLayer;
	
	// Use this for initialization
	void Awake ()
	{
		initialY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!OnPlayerLayer ()) {
			timeOffPlayerLayer+= Time.deltaTime;
			if (timeOffPlayerLayer >= waitTimeBeforeLayerChange) {
				gameObject.layer = player.layer;
				if (gameObject.layer == BACKGROUND_LAYER) {
					Vector3 newPosition = new Vector3(transform.position.x, initialY, 0);
					transform.position = newPosition;
				}
				timeOffPlayerLayer = 0f;
			}
		}
	}

	bool OnPlayerLayer() {
		return gameObject.layer == player.layer;
	}
}

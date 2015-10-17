using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour {

	
	protected Player player;
	protected Movement2D movement2D;
	protected float speed;
	
	protected void Start() {
		player = gameObject.GetComponent<Player> ();
		movement2D = gameObject.GetComponent<Movement2D> ();
		speed = movement2D.maxSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 screenPoint = Input.mousePosition;
		screenPoint.z = 10.0f;
		movement2D.MoveTo (Camera.main.ScreenToWorldPoint(screenPoint));
	}
}

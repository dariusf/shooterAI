using UnityEngine;
using System.Collections;

public class Movement2D : MonoBehaviour {

	// Movement bounds
	public float xMax = 9f;
	public float xMin = -9f;
	public float yMax = 5f;
	public float yMin = -5f;
	
	// Max speed
	public float maxSpeed = 3f;

	public void Move(Vector3 v) {
		MoveTo(v + gameObject.transform.position);
	}

	/**
	 * Set position, with boundary and speed restrictions.
	 */
	public void MoveTo(Vector3 pos) {
		Vector3 moveVector = pos - gameObject.transform.position;
		moveVector = Vector3.ClampMagnitude (moveVector, maxSpeed * Time.deltaTime);
		Vector3 finalPos = ClampToBorders(gameObject.transform.position + moveVector);
		gameObject.transform.position = finalPos;
	}

	public void MoveToUnrestricted(Vector3 pos) {
		gameObject.transform.position = pos;
	}

	public Vector3 ClampToBorders(Vector3 pos) {
		return new Vector3(Mathf.Clamp(pos.x, xMin, xMax),
		                   Mathf.Clamp(pos.y, yMin, yMax), 0f);
	}
}

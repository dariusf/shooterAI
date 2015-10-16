using UnityEngine;
using System.Collections;

public class ShooterMovement : MonoBehaviour {

	float xMax = 3f;
	float xMin = -3f;
	float yMax = 4f;
	float yMin = 3f;
	bool left = false;
	bool up = false;
	float xSpeedPerSec = 1f;
	float ySpeedPerSec = 1.9f;

	void Start () {
		
	}
	
	void Update () {

		float vx = xSpeedPerSec * Time.deltaTime;
		float vy = ySpeedPerSec * Time.deltaTime;
	
		if (left) {
			vx *= -1f;
		}
		if (up) {
			vy *= -1f;
		}

		Vector3 movement = new Vector3(vx, vy, 0f);
		gameObject.transform.position = gameObject.transform.position + movement;

		float x = gameObject.transform.position.x;
		float y = gameObject.transform.position.y;

		if (left) {
			if (x < xMin) {
				left = !left;
			}
		} else { // right
			if (x > xMax) {
				left = !left;
			}
		}

		if (up) {
			if (y < yMin) {
				up = !up;
			}
		} else { // right
			if (y > yMax) {
				up = !up;
			}
		}

	}
}

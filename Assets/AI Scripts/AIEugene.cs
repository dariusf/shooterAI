using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIEugene : AIPlayer {
	
	float EPS = 0.0001f;
//	float xMax = 3f;
//	float xMin = -3f;
//	float yMax = -4f;
//	float yMin = -3f;
	float radius = 2f;
	bool left = false;
	bool up = false;
	float xMaxSpeedPerSec = 2f;
	float yMaxSpeedPerSec = 2f;
	
	void Update () {
		Collider2D [] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, radius, bulletLayer);
		
		// sort bullets by distance
		System.Array.Sort(colliders, delegate(Collider2D a, Collider2D b) {
			Vector3 distanceA = a.attachedRigidbody.position - ((Vector2)transform.position);
			Vector3 distanceB = b.attachedRigidbody.position - ((Vector2)transform.position);
			if(distanceA.sqrMagnitude > distanceB.sqrMagnitude) {
				return 1;
			} else if(distanceA.sqrMagnitude < distanceB.sqrMagnitude) {
				return -1;
			}
			return 0;
		});

//		Vector3 netMovement = Vector3.zero;

		foreach (Collider2D c in colliders) {
			c.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
			Vector2 bulletVelocity = c.attachedRigidbody.velocity;
			Vector2 bulletPosition = c.attachedRigidbody.position;
			
			// set movement to be in positive x direction
			float dx = xMaxSpeedPerSec * Time.deltaTime;
			
			// go in negative x direction if the following occurs:
			//  1. enemies bullets has positive x-velocity
			//  2. enemies bullets has 0 x-velocity but and
			//     own ship is left of bullet
			if (bulletVelocity.x > 0f ||
			    (bulletVelocity.x == 0f &&
			 bulletPosition.x - gameObject.transform.position.x > 0f)) {
				dx = -dx;
			}
			Vector3 movement = new Vector3(dx, 0f, 0f);
//			netMovement += movement;
			gameObject.transform.position += movement;
			break;
		}		
//		movement2D.MoveTo(gameObject.transform.position + netMovement);
	}
}

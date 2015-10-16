using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIDarius : AIPlayer {

	void Update () {	
		Collider2D closest = findClosestBullet();
		
		Vector3 playerPosition = gameObject.transform.position;
				
		if (closest == null) {
			Vector3 dest1 = playerPosition.normalized * -speed * Time.deltaTime;
			Move(dest1);
		} else {
			closest.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
			
			Vector3 closestVelocity = closest.GetComponent<Rigidbody2D>().velocity.normalized;
			Vector3 p = Perpendicular(closest.gameObject.transform.position,
									  closest.gameObject.transform.position + closestVelocity);
							

			float dist1 = Vector3.Distance(playerPosition + p, closest.gameObject.transform.position);
			float dist2 = Vector3.Distance(playerPosition - p, closest.gameObject.transform.position);
			bool whichIsCloser = dist2 > dist1;

			// TODO not equivalent?
			//  bool whichIsCloser = Vector3.Dot(p, closest.gameObject.transform.position) > 0;
		
			Vector3 awayFromBullet = whichIsCloser ? -p : p;
			Debug.DrawLine(playerPosition, playerPosition + awayFromBullet, Color.white);		
		
			Move(awayFromBullet);
		}
	}
	
	void Move(Vector3 v) {
		Vector3 dest = v + gameObject.transform.position;
		movement2D.MoveTo(dest);
	}
	
	Vector2 Perpendicular(Vector2 start, Vector2 end) {
		Vector2 v = end - start;
		Vector2 start1 = new Vector2(-v.y, v.x) / Mathf.Sqrt(v.x * v.x + v.y * v.y) * v.magnitude;
		Vector2 end1 = new Vector2(-v.y, v.x) / Mathf.Sqrt(v.x * v.x + v.y * v.y) * -v.magnitude;
		return end1 - start1;
	}
}

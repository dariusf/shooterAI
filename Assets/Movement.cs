using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour {

	public LayerMask bulletLayer;
	public bool slowMotion = false;
	
	// Player movement bounds
	float xMax = 6f;
	float xMin = -6f;
	float yMax = 3f;
	float yMin = -4f;

	// Player speed
	float speed = 3f;
	
	void Update () {
		
		Time.timeScale = slowMotion ? 0.3f : 1.0f;
				
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
	
	Collider2D findClosestBullet() {
		Collider2D [] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 1f, bulletLayer);
		float minDist = float.MaxValue;
		Collider2D closest = null;
		foreach (Collider2D c in colliders) {
			float dist = (c.gameObject.transform.position - gameObject.transform.position).magnitude;
			if (dist < minDist) {
				minDist = dist;
				closest = c;
			}
		}
		return closest;
	}
	
	void Move(Vector3 v) {
		v = Vector3.ClampMagnitude(v, speed * Time.deltaTime);
		Vector3 dest = v + gameObject.transform.position;
		dest = new Vector3(Mathf.Clamp(dest.x, xMin, xMax), Mathf.Clamp(dest.y, yMin, yMax), 0f);
		gameObject.transform.position = dest;
	}
	
	Vector2 Perpendicular(Vector2 start, Vector2 end) {
		Vector2 v = end - start;
		Vector2 start1 = new Vector2(-v.y, v.x) / Mathf.Sqrt(v.x * v.x + v.y * v.y) * v.magnitude;
		Vector2 end1 = new Vector2(-v.y, v.x) / Mathf.Sqrt(v.x * v.x + v.y * v.y) * -v.magnitude;
		return end1 - start1;
	}
}

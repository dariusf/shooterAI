using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIDarius : AIPlayer {

	static readonly List<Vector3> neutralPositions = new List<Vector3> {
		new Vector2(0, 0),
		new Vector2(3, -3),
		new Vector2(-3, -3)
	};

	int cnp = 0;

	new void Start () {
		base.Start ();
	}

	void Update () {

		foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet")) {
			bullet.GetComponent<SpriteRenderer>().color = Color.white;
		}

		Collider2D closest = findClosestBullet();
		
		Vector3 playerPosition = gameObject.transform.position;

		if ((playerPosition - neutralPositions[cnp]).magnitude < 0.01f) {
			cnp = (cnp + 1) % neutralPositions.Count;
		}
				
		if (closest == null) {
			movement2D.MoveTo(neutralPositions[cnp]);
		} else {
			closest.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
			
			Vector3 closestVelocity = closest.GetComponent<Rigidbody2D>().velocity.normalized;
			Vector3 p = Perpendicular(closest.gameObject.transform.position,
									  closest.gameObject.transform.position + closestVelocity);
			p = p.normalized * speed * Time.deltaTime;

			float dist1 = Vector3.Distance(playerPosition + p, closest.gameObject.transform.position);
			float dist2 = Vector3.Distance(playerPosition - p, closest.gameObject.transform.position);
			bool whichIsCloser = dist2 > dist1;

			// TODO not equivalent?
			//  bool whichIsCloser = Vector3.Dot(p, closest.gameObject.transform.position) > 0;
		
			Vector3 awayFromBullet = whichIsCloser ? -p : p;
			Debug.DrawLine(playerPosition, playerPosition + awayFromBullet, Color.white);		
		
			movement2D.Move(awayFromBullet);
		}
	}
	
	Vector2 Perpendicular(Vector2 start, Vector2 end) {
		Vector2 v = end - start;
		Vector2 start1 = new Vector2(-v.y, v.x) / Mathf.Sqrt(v.x * v.x + v.y * v.y) * v.magnitude;
		Vector2 end1 = new Vector2(-v.y, v.x) / Mathf.Sqrt(v.x * v.x + v.y * v.y) * -v.magnitude;
		return end1 - start1;
	}
}

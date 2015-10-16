using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPlayer : MonoBehaviour {

	public LayerMask bulletLayer;

	protected Movement2D movement2D;
	protected float speed;

	protected void Start() {
		movement2D = gameObject.GetComponent<Movement2D> ();
		speed = movement2D.maxSpeed;
	}

	protected Collider2D findClosestBullet(float range=1f) {
		Collider2D [] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, range, bulletLayer);
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

	protected Collider2D[] findBullets(float range=1f) {
		return Physics2D.OverlapCircleAll(gameObject.transform.position, range, bulletLayer);
	}


}

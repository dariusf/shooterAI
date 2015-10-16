using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPlayer : MonoBehaviour {

	public LayerMask bulletLayer;

	protected Movement2D movement2D;
	protected float speed;

	void Start() {
		movement2D = gameObject.GetComponent<Movement2D> ();
		speed = movement2D.maxSpeed;
	}

	protected Collider2D findClosestBullet() {
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
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPlayer : MonoBehaviour {

	public LayerMask bulletLayer;
	
	protected Player player;
	protected Movement2D movement2D;
	protected float speed;

	protected void Start() {
		player = gameObject.GetComponent<Player> ();
		movement2D = gameObject.GetComponent<Movement2D> ();
		speed = movement2D.maxSpeed;
	}

	protected Collider2D findClosestBullet(float range=1f, bool filterFF=true) {
		Collider2D [] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, range, bulletLayer);
		float minDist = float.MaxValue;
		Collider2D closest = null;
		foreach (Collider2D c in colliders) {
			float dist = (c.gameObject.transform.position - gameObject.transform.position).magnitude;
			if (dist < minDist && !bulletIsOwnTeam(c)) {
				minDist = dist;
				closest = c;
			}
		}
		return closest;
	}

	protected Collider2D[] findBullets(float range=1f, bool filterFF=true) {
		Collider2D[] colliders = Physics2D.OverlapCircleAll (gameObject.transform.position, range, bulletLayer);
		List<Collider2D> bullets = new List<Collider2D>();
		foreach(Collider2D c in colliders)
		{
			if(!bulletIsOwnTeam(c))
				bullets.Add(c);
		}
		return bullets.ToArray();
	}

	bool bulletIsOwnTeam(Collider2D b) {
		Bullet bullet = b.GetComponent<Bullet> ();
		if (player != null && bullet != null && player.team == bullet.getTeam() && player.team >= 0) {
			return true;
		}
		return false;
	}

}

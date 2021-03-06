﻿using UnityEngine;
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

	public Collider2D findClosestBullet(float range=1f, bool filterFF=true) {
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

	public Collider2D[] findBullets(float range=1f, bool filterFF=true) {
		Collider2D[] colliders = Physics2D.OverlapCircleAll (gameObject.transform.position, range, bulletLayer);
		List<Collider2D> bullets = new List<Collider2D>();
		foreach(Collider2D c in colliders)
		{
			if(!bulletIsOwnTeam(c))
				bullets.Add(c);
		}
		return bullets.ToArray();
	}

	public Collider2D findClosestEnemy(float range=1f) {
		Collider2D[] colliders = findEnemies (range);
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

	public Collider2D[] findEnemies(float range=1f) {
		Collider2D[] colliders = Physics2D.OverlapCircleAll (gameObject.transform.position, range);
		List<Collider2D> bullets = new List<Collider2D>();
		foreach(Collider2D c in colliders)
		{
			if(isPlayer(c) && !playerIsOwnTeam (c))
				bullets.Add(c);
		}
		return bullets.ToArray();
	}

	public bool bulletIsOwnTeam(Collider2D b) {
		Bullet bullet = b.GetComponent<Bullet> ();
		if (player != null && bullet != null && player.team == bullet.getTeam() && player.team >= 0) {
			return true;
		}
		return false;
	}

	public bool isPlayer(Collider2D c) {
		Player p = c.GetComponent<Player> ();
		if (p != null) {
			return true;
		}
		return false;
	}

	public bool playerIsOwnTeam(Collider2D c) {
		Player p = c.GetComponent<Player> ();
		if (p != null && player!= null && p.team == player.team) {
			return true;
		}
		return false;
	}
}

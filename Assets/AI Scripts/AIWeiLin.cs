﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIWeiLin : AIPlayer {
	
	public GameObject BulletPrefab;
	
	float hitRadius;

	private Collider2D[] enemiesAll;

	new void Start() {
		base.Start ();
		hitRadius = GetComponent<CircleCollider2D>().radius + BulletPrefab.GetComponent<CircleCollider2D>().radius + 0.005f;
		enemiesAll = findEnemies (10);
	}
	
	void Update () {
		//enemiesAll = findEnemies (10);

		Collider2D [] collidersAll = findBullets (10);
		foreach (Collider2D c in collidersAll) {
			c.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		}

		Collider2D [] colliders = findBullets (4);

		List<Collider2D> bullets = new List<Collider2D> (colliders);
		
		// Compute movement
		Vector3 moveDir = Vector3.zero;
		
		// Compute score
		float currScore = score (bullets, gameObject.transform.position, true);

		if (currScore != 0f) {
			float minScore = 100;
			Vector2 bestDir = Vector2.zero;
			
			int searchPoints = 32;
			int searchLayers = 60;
			float searchRangeK = 0.05f;
			float searhLineThreshold = 1.2f;
			
			for (float i=0 ; i<360 ; i+=360f/searchPoints) {				
				for (int j=1 ; j<=searchLayers ; j++) {
					float rad = Mathf.Deg2Rad * i;
					Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * j * searchRangeK;

					float s = score(bullets, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)
					                + dir);

					float sInv = 1f/s;
					Debug.DrawLine(gameObject.transform.position, gameObject.transform.position+new Vector3(dir.x, dir.y, 0), new Color(s,s,s));

					if (s < minScore ) {
						minScore = s;
						bestDir = dir;
					} else {
						if (s > searhLineThreshold) {
							Debug.Log(s);
							break;
						}
					}
				}
			}
			moveDir = bestDir;
			
			Debug.DrawLine(gameObject.transform.position, gameObject.transform.position+moveDir, Color.magenta);
		}

		Vector3 dest = gameObject.transform.position + moveDir;
		movement2D.MoveTo (dest);
	}

	float score(List<Collider2D> bullets, Vector2 pos, bool draw=false) {
		Vector3 pos3D = new Vector3 (pos.x, pos.y, 0);
		float s = 0;
		for (int i=0; i<bullets.Count; i++) {
			Vector3 bulletDir = bullets[i].gameObject.GetComponent<Rigidbody2D>().velocity;
			Vector3 bulletPos = bullets[i].gameObject.transform.position;
			Vector3 bulletToPlayer = (pos3D - bulletPos);
			
			float projectionLength = Vector3.Dot(bulletToPlayer, bulletDir.normalized);
			
			float pDistance = Mathf.Sqrt(bulletToPlayer.magnitude*bulletToPlayer.magnitude - projectionLength*projectionLength);
			float distance = bulletToPlayer.magnitude;

			// Where bullet will be next frame
			//Debug.DrawLine(bullets[i].gameObject.transform.position + bulletDir*Time.deltaTime,
			//               bullets[i].gameObject.transform.position, Color.red);

			// In line of fire
			if (pDistance <= hitRadius && projectionLength >= -0.1){// -hitRadius*2) {
				Debug.DrawLine(bulletPos, bullets[i].gameObject.transform.position, Color.green);

				// Bullet max projected path
				Debug.DrawLine(bullets[i].gameObject.transform.position + bulletDir.normalized*projectionLength,
				               bullets[i].gameObject.transform.position, Color.red);

				// Compute score
				//float ss = 0.1f*1/(pDistance+0.001f) + (1f/(distance+0.001f)) - 0.01f*gameObject.transform.position.magnitude;


				// 
				float bulletSpeed = bulletDir.magnitude;
				Vector3 futureBulletPos = bulletPos + bulletDir.normalized * bulletSpeed * (pDistance / movement2D.maxSpeed);
				futureBulletPos -= 2f*hitRadius*bulletDir.normalized;
				float futureBulletDist = (pos3D - futureBulletPos).magnitude;
				Vector3 futureBulletToPos = pos3D - futureBulletPos;

				
				Debug.DrawLine(futureBulletPos, futureBulletPos + 1f*hitRadius*bulletDir.normalized, Color.yellow);

				float futureBulletProjection = Vector3.Dot(futureBulletToPos, bulletDir.normalized);
				float futurePDistance = Mathf.Sqrt(futureBulletToPos.magnitude*futureBulletToPos.magnitude 
				                                   - futureBulletProjection*futureBulletProjection);

				if (futureBulletProjection < -0.1) {
					futureBulletDist = 1000000;
				}

				Debug.DrawLine(futureBulletPos, bullets[i].gameObject.transform.position, Color.cyan);
				Debug.DrawLine(pos, gameObject.transform.position, Color.magenta);


				//float ss = futureBulletDist <= hitRadius ? futureBulletDist/hitRadius:0f;
				float ss = Mathf.Clamp(1-futureBulletDist, 0, 1);///hitRadius;
				if (pDistance <= hitRadius ) {
					ss += (Mathf.Min(1f/futurePDistance, 1f));
				}

				//float ss = Mathf.Min(bulletDir.magnitude/distance, 1f) + Mathf.Min(projectionLength/pDistance, 1f);
				s = Mathf.Max(s, ss);


				//float ss = 1f;
				//s += ss;

				if (draw)
					bullets[i].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
			} else {
				if (draw)
					bullets[i].gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
			}
		}

		// Move to middle
		if ((pos+new Vector2(0,2)).magnitude > 1f)
			s += (pos+new Vector2(0,2)).magnitude*0.00001f;

		/*
		foreach (Collider2D enemy in enemiesAll) {
			float dist = (enemy.transform.position - pos3D).magnitude;
			if (dist < 6) {
				s += (1f/(dist+0.000001f));
			}
		}
		*/

		return s;
	}
}

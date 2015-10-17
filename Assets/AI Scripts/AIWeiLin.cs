using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIWeiLin : AIPlayer {
	
	public GameObject BulletPrefab;
	
	float hitRadius;

	//private Collider2D[] enemiesAll;

	private Vector2 target;
	private float targetThreshold = 0.5f;

	new void Start() {
		base.Start ();
		hitRadius = GetComponent<CircleCollider2D>().radius + BulletPrefab.GetComponent<CircleCollider2D>().radius + 0.02f;
		//enemiesAll = findEnemies (10);
		target = transform.position;
	}
	
	void Update () {

		// Change target location randomly
		if (Random.Range(0,100)==0) {
			target = new Vector2(Random.Range(movement2D.xMin*0.75f, movement2D.xMax*0.75f),
			                     Random.Range(movement2D.yMin*0.75f, movement2D.yMax*0.75f));
			//target = new Vector2(transform.position.x, transform.position.y) + new Vector2(Random.Range(-3f,3f), Random.Range(-3f,3f));
			//target = movement2D.ClampToBorders(target);
		}


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
			float searhLineThreshold = 100f;
			
			for (float i=0 ; i<360 ; i+=360f/searchPoints) {
				float ssum = 0;
				for (int j=1 ; j<=searchLayers ; j++) {
					float rad = Mathf.Deg2Rad * i;
					Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * j * searchRangeK;
					
					Vector3 dir3D = new Vector3 (dir.x, dir.y, 0);
					Vector3 dst = gameObject.transform.position + dir3D;
					if (movement2D.ClampToBorders(dst) != dst) {
						continue;
					}

					float s = score(bullets, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)
					                + dir);

					if (ssum < searhLineThreshold)
						ssum *= 0.90f;
					ssum += s;
					s = ssum;

					Debug.DrawLine(gameObject.transform.position, gameObject.transform.position+new Vector3(dir.x, dir.y, 0), new Color(s,s,s));

					if (s < minScore ) {
						minScore = s;
						bestDir = dir;
					} else {
						if (s > searhLineThreshold && minScore < searhLineThreshold) {
							//Debug.Log(s);
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
			//float distance = bulletToPlayer.magnitude;

			// Where bullet will be next frame
			//Debug.DrawLine(bullets[i].gameObject.transform.position + bulletDir*Time.deltaTime,
			//               bullets[i].gameObject.transform.position, Color.red);

			// In line of fire
			if (pDistance <= hitRadius && projectionLength >= -0.3){// -hitRadius*2) {
				Debug.DrawLine(bulletPos, bullets[i].gameObject.transform.position, Color.green);

				// Bullet max projected path
				Debug.DrawLine(bullets[i].gameObject.transform.position + bulletDir.normalized*projectionLength,
				               bullets[i].gameObject.transform.position, Color.red);

				// Compute score
				//float ss = 0.1f*1/(pDistance+0.001f) + (1f/(distance+0.001f)) - 0.01f*gameObject.transform.position.magnitude;


				// 
				float bulletSpeed = bulletDir.magnitude;
				Vector3 futureBulletPos = bulletPos + bulletDir.normalized * bulletSpeed * (pDistance / movement2D.maxSpeed);
				Debug.DrawLine(futureBulletPos, futureBulletPos - 1f*hitRadius*bulletDir.normalized, Color.yellow);

				futureBulletPos -= 1f*hitRadius*bulletDir.normalized;
				float futureBulletDist = (pos3D - futureBulletPos).magnitude;
				Vector3 futureBulletToPos = pos3D - futureBulletPos;

				

				float futureBulletProjection = Vector3.Dot(futureBulletToPos, bulletDir.normalized);
				float futurePDistance = Mathf.Sqrt(futureBulletToPos.magnitude*futureBulletToPos.magnitude 
				                                   - futureBulletProjection*futureBulletProjection);

				if (futureBulletProjection < -0.1) {
					futureBulletDist = 1000000;
				}

				//Debug.DrawLine(futureBulletPos, bullets[i].gameObject.transform.position, Color.cyan);
				//Debug.DrawLine(pos, gameObject.transform.position, Color.magenta);


				//float ss = futureBulletDist <= hitRadius ? futureBulletDist/hitRadius:0f;
				float ss = Mathf.Clamp(5f/(futureBulletDist), 0, 10);///hitRadius;
				if (futurePDistance <= hitRadius*1.5f ) {
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
		Debug.DrawLine (transform.position, target, Color.yellow);
		if ((target-pos).magnitude > targetThreshold)
			s += (target-pos).magnitude*0.00001f;

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

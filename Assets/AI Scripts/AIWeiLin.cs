using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIWeiLin : AIPlayer {
	
	public GameObject BulletPrefab;
	
	float hitRadius;
	
	new void Start() {
		base.Start ();
		hitRadius = GetComponent<CircleCollider2D>().radius + BulletPrefab.GetComponent<CircleCollider2D>().radius + 0.005f;
	}
	
	void Update () {

		Collider2D [] colliders = findBullets (3);

		List<Collider2D> bullets = new List<Collider2D> (colliders);
		
		// Compute movement
		Vector3 moveDir = Vector3.zero;
		
		// Compute score
		float currScore = score (bullets, gameObject.transform.position);

		if (currScore != 0f) {
			float minScore = 100;
			Vector2 bestDir = Vector2.zero;
			
			int searchPoints = 32;
			int searchLayers = 5;
			float searchRangeK = 10;
			float searhLineThreshold = 0.8f;
			
			for (float i=0 ; i<360 ; i+=360f/searchPoints) {				
				for (int j=1 ; j<=searchLayers ; j++) {
					float rad = Mathf.Deg2Rad * i;
					Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * j * searchRangeK * 3f * Time.deltaTime;

					float s = score(bullets, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)
					                + dir);

					float sInv = 1f/s;
					Debug.DrawLine(gameObject.transform.position, gameObject.transform.position+new Vector3(dir.x, dir.y, 0), new Color(sInv,sInv,sInv));

					if (s < minScore ) {
						minScore = s;
						bestDir = dir;
					} else {
						if (s > searhLineThreshold) {
							break;
						}
					}
				}
			}
			moveDir = bestDir;
		}

		Vector3 dest = gameObject.transform.position + moveDir;
		movement2D.MoveTo (dest);
	}
	
	
	float score(List<Collider2D> bullets, Vector2 pos) {
		float s = 0;
		for (int i=0; i<bullets.Count; i++) {
			Vector3 bulletDir = bullets[i].gameObject.GetComponent<Rigidbody2D>().velocity;
			Vector3 bulletToPlayer = (new Vector3(pos.x, pos.y, 0) - bullets[i].gameObject.transform.position);
			
			float projectionLength = Vector3.Dot(bulletToPlayer, bulletDir.normalized);
			
			float pDistance = Mathf.Sqrt(bulletToPlayer.magnitude*bulletToPlayer.magnitude - projectionLength*projectionLength);
			float distance = bulletToPlayer.magnitude;
			
			if (pDistance <= hitRadius) {
				// Compute score
				float ss = 0.1f*1/(pDistance+0.001f) * (1f/(distance+0.001f)) - 0.01f*gameObject.transform.position.magnitude;
				s = Mathf.Max(s, ss);
				bullets[i].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
			} else {
				bullets[i].gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
			}
		}

		// Move to middle
		s += pos.magnitude*0.001f;
		
		return s;
	}
}

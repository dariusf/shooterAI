using UnityEngine;
using System.Collections;

public class Shooter2 : ShooterBase {

	// Use this for initialization
	new void Start () {
		base.Start ();
		cooldown = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		if (aiPlayer != null) {
			Collider2D enemy = aiPlayer.findClosestEnemy (10);
			if (enemy != null) {
				Vector3 vec = (enemy.transform.position -
				               gameObject.transform.position);
				float angle = Mathf.Rad2Deg * Mathf.Atan2(vec.y, vec.x);
				angle += Random.Range(-50f, 50f);
				FireBullet (angle, 5f);
			}
		}
	}
}

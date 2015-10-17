using UnityEngine;
using System.Collections;

public class Shooter2 : ShooterBase {

	// Use this for initialization
	new void Start () {
		base.Start ();
		cooldown = 0.3f;
		Debug.Log (aiPlayer);
	}
	
	// Update is called once per frame
	void Update () {
		if (aiPlayer != null) {
			Collider2D enemy = aiPlayer.findClosestEnemy(10);
			if (enemy != null)
				FireBullet((enemy.transform.position - gameObject.transform.position).normalized*4f, Vector3.zero);
		}
	}
}

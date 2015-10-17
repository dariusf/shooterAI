using UnityEngine;
using System.Collections;

public class Shooter1 : ShooterBase {

	// Use this for initialization
	new void Start () {
		base.Start ();
		cooldown = 0.3f;
	}
	
	// Update is called once per frame
	void Update () {
		FireBullet (Random.Range(0f,360f), 4f);
	}
}

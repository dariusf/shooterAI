using UnityEngine;
using System.Collections;

public class Shooter1 : ShooterBase {

	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		FireBullet (new Vector3 (0, 4f + Random.Range(0,1.5f), 0));
	}
}

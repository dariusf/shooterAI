using UnityEngine;
using System.Collections;

public class FireProjectiles : MonoBehaviour {

	public GameObject bullet;
	float speed = 2f;

	void Start () {
		StartCoroutine(Fire());
	}

	IEnumerator Fire() {
		GameObject shot;

		while (true) {
			
			//  float speed2 = speed + Random.Range(0, 5) * 0.05f;
			// up
			//  for (int i=0; i<3; i++) {
			//  	float rad = (float) Mathf.Deg2Rad * (Random.Range(-45, -45 + 90) + (transform.localScale.y * 90));
			//  	Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * speed;
				
				shot = Instantiate(bullet, gameObject.transform.position, Quaternion.identity) as GameObject;
				shot.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -speed);
				//  shot.GetComponent<Rigidbody2D>().velocity = direction;
			//  }

			//  // southwest
			shot = Instantiate(bullet, gameObject.transform.position, Quaternion.identity) as GameObject;
			shot.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, -speed);

			//  // southeast
			shot = Instantiate(bullet, gameObject.transform.position, Quaternion.identity) as GameObject;
			shot.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, -speed);

			yield return new WaitForSeconds(0.7f);
		}
	}
}

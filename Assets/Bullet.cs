using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private int damage = 1;

	void OnCollisionEnter2D(Collision2D collision) {

		// If what is hit has health, damage it.
		if (collision.gameObject.GetComponent<Health> () != null) {
			collision.gameObject.GetComponent<Health> ().TakeDamage (damage);
			Destroy (gameObject);
		}
	}

	void Start() {
		StartCoroutine(Timer());
	}

	IEnumerator Timer() {
		yield return new WaitForSeconds(14f);
		Destroy(gameObject);
	}
}

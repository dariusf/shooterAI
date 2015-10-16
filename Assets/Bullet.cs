using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			Destroy(gameObject);
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

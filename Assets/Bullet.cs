using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private int damage = 1;
	private int team = -2;

	void OnTriggerEnter2D(Collider2D collider) {

		// If has health and not on same team, apply damage.
		Health health = collider.gameObject.GetComponent<Health> ();
		Player player = collider.gameObject.GetComponent<Player> ();
		if ( health != null && (player == null || player.team != team) ) {
			collider.gameObject.GetComponent<Health> ().TakeDamage (damage);
			Destroy (gameObject);
		}
	}

	void Start() {
		StartCoroutine(Timer());
	}

	public int getTeam() {
		return team;
	}

	public void setTeam(int _team) {
		if (team == -2) {
			team = _team;
		} else {
			Debug.Log("Unable to set team for bullet!");
		}
	}

	IEnumerator Timer() {
		yield return new WaitForSeconds(14f);
		Destroy(gameObject);
	}
}

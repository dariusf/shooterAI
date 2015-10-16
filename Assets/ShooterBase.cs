using UnityEngine;
using System.Collections;

public class ShooterBase : MonoBehaviour {
	
	[SerializeField]
	private GameObject prefab_bullet;

	private Player player;
	private int team = -1;

	protected void Start() {
		player = gameObject.GetComponent<Player> ();
		if (player != null)
			team = player.team;
	}

	public GameObject FireBullet(float directionDeg, float speed)
	{
		return FireBullet(directionDeg, speed, Vector3.zero);
	}
	
	public GameObject FireBullet(float direction, float speed, Vector3 offset)
	{
		direction *= Mathf.PI/180f;
		var velocity = new Vector3(Mathf.Cos(direction), Mathf.Sin(direction), 0)*speed;
		return FireBullet(velocity, offset);
	}
	
	public GameObject FireBullet(Vector3 velocity)
	{
		return FireBullet(velocity, Vector3.zero);
	}
	
	public GameObject FireBullet(Vector3 velocity, Vector3 offset)
	{
		var bullet = Instantiate(prefab_bullet, transform.position + offset, prefab_bullet.transform.rotation) as GameObject;
		bullet.GetComponent<Rigidbody2D> ().velocity = velocity;
		bullet.GetComponent<Bullet> ().setTeam (team);
		return bullet;
	}
}

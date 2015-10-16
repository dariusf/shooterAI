using UnityEngine;
using System.Collections;

public class ShooterBase : MonoBehaviour {
	
	[SerializeField]
	private GameObject prefab_bullet;

	private Player player;
	private int team = -1;

	private float cooldown = 0.5f;
	private float nextBulletTime = 0f;


	protected void Start() {
		player = gameObject.GetComponent<Player> ();
		if (player != null)
			team = player.team;
	}

	protected GameObject FireBullet(float direction, float speed)
	{
		return FireBullet(direction, speed, Vector3.zero);
	}
	
	protected GameObject FireBullet(float direction, float speed, Vector3 offset)
	{
		direction %= 360;
		direction *= Mathf.PI/180f;
		var velocity = new Vector3(Mathf.Cos(direction), Mathf.Sin(direction), 0)*speed;
		return FireBullet(velocity, offset);
	}
	
	protected GameObject FireBullet(Vector3 velocity)
	{
		return FireBullet(velocity, Vector3.zero);
	}
	
	protected GameObject FireBullet(Vector3 velocity, Vector3 offset)
	{
		if (Time.time > nextBulletTime)
		{
			var bullet = Instantiate(prefab_bullet, transform.position + offset, prefab_bullet.transform.rotation) as GameObject;
			bullet.GetComponent<Rigidbody2D> ().velocity = velocity;
			bullet.GetComponent<Bullet> ().setTeam (team);

			// Cooldown start
			nextBulletTime += cooldown;
			
			return bullet;
		}
		return null;
	}
}

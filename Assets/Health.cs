using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public int maxHp = 10;
	public int hp = 10;

	// Use this for initialization
	void Start () {
		hp = maxHp;
	}

	public void TakeDamage(int damage)
	{
		hp -= damage;
		hp = Mathf.Max (hp, 0);
		if (hp <= 0)
		{
			Debug.Log("Died:" + gameObject);
			Destroy(this.gameObject);
		}
	}

	public void Heal(int amount)
	{
		hp += amount;
		hp = Mathf.Min (hp, maxHp);
	}

}

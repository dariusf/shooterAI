using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour {

    //  [SerializeField]
    private BulletPattern bulletPattern;

    [SerializeField]
    private GameObject prefab_bullet;

    [SerializeField]
    private RotateDirection rotateDirection;

    private bool clockwise;

    private BulletController bulletController;

    public enum BulletPattern
    {
        RANDOM_SPRAY,
        RINGS,
        LOOPS,
        WAVES,
        SPIRAL,
        BURST,
    }

    public enum RotateDirection
    {
        RANDOM,
        CLOCKWISE,
        ANTICLOCKWISE,
    }

    // Must be called before start.
    public void SetBulletPattern(BulletPattern pattern)
    {
        this.bulletPattern = pattern;
    }
    
    private EnemyShooting.BulletPattern GetRandomPattern()
    {
        int choice = Random.Range(0, 6);
        switch (choice)
        {
            case 0: return EnemyShooting.BulletPattern.RANDOM_SPRAY;
            case 1: return EnemyShooting.BulletPattern.BURST;
            case 2: return EnemyShooting.BulletPattern.WAVES;
            case 3: return EnemyShooting.BulletPattern.SPIRAL;
            case 4: return EnemyShooting.BulletPattern.LOOPS;
            case 5: return EnemyShooting.BulletPattern.RINGS;
        }
        return EnemyShooting.BulletPattern.RANDOM_SPRAY;
    }

    // Use this for initialization
	void Start () {
        
        SetBulletPattern(GetRandomPattern());
        
	    switch (rotateDirection)
        {
            case RotateDirection.RANDOM:
                clockwise = Random.seed % 2 == 1;
                break;
	        case RotateDirection.CLOCKWISE:
	            clockwise = true;
	            break;
	        case RotateDirection.ANTICLOCKWISE:
	            clockwise = false;
	            break;
	    }

	    switch (bulletPattern)
        {
            case BulletPattern.RANDOM_SPRAY:
                bulletController = new BCRandomSpray(this);
                break;
            case BulletPattern.RINGS:
                bulletController = new BCRings(this);
                break;
            case BulletPattern.LOOPS:
                bulletController = new BCLoops(this);
                break;
            case BulletPattern.WAVES:
                bulletController = new BCWaves(this);
                break;
            case BulletPattern.SPIRAL:
                bulletController = new BCSpiral(this);
                break;
            case BulletPattern.BURST:
                bulletController = new BCBurst(this);
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        bulletController.Update();
	}

    public GameObject FireBullet(float direction, float speed)
    {
        return FireBullet(direction, speed, Vector3.zero);
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
        if (clockwise) velocity.x = -velocity.x;
        var bullet = Instantiate(prefab_bullet, transform.position + offset, prefab_bullet.transform.rotation) as GameObject;
		bullet.GetComponent<Rigidbody2D>().velocity = velocity;
		bullet.GetComponent<Bullet> ().setTeam (0);
        //  bullet.GetComponent<BulletHit>().isEnemyBullet = true;
        return bullet;
    }
}


interface BulletController
{
    void Update();
}

class BCRandomSpray : BulletController
{
    private float currentDirection;
    private float cooldown = 0.2f;
    private float nextBulletTime = 0f;
    private EnemyShooting shooting;

    public BCRandomSpray(EnemyShooting shooting)
    {
        this.shooting = shooting;
        currentDirection = Random.seed % 4 * 90 + 10f;
        nextBulletTime = Time.time;
    }

    public void Update()
    {
        if (Time.time > nextBulletTime)
        {
            currentDirection += 90f;
            if (currentDirection > 360) currentDirection -= 360f;
            shooting.FireBullet(currentDirection + Random.Range(0, 70), 5f);

            nextBulletTime += cooldown;

        }
    }
}

class BCRings : BulletController
{
    private float cooldown = 0.9f;
    private float nextBulletTime = 0f;
    private EnemyShooting shooting;
    private int count = 11;
    private float angleGap;

    public BCRings(EnemyShooting shooting)
    {
        this.shooting = shooting;
        angleGap = 360f / count;
        nextBulletTime = Time.time;
    }

    public void Update()
    {
        if (Time.time > nextBulletTime)
        {
            var baseDirection = Random.Range(0, angleGap);
            for (int i = 0; i < count; ++i)
            {
                shooting.FireBullet(baseDirection + i * angleGap, 5f);
            }

            nextBulletTime += cooldown;
        }
    }
}

class BCLoops : BulletController
{
    private float currentDirection;
    private float cooldown = 0.6f;
    private float nextBulletTime = 0f;
    private EnemyShooting shooting;

    private Vector3 gun1 = new Vector3(-0.6f, 0, 0);
    private Vector3 gun2 = new Vector3(-0.3f, -0.52f, 0);
    private Vector3 gun3 = new Vector3(0.3f, -0.52f, 0);
    private Vector3 gun4 = new Vector3(0.6f, 0, 0);
    private Vector3 gun5 = new Vector3(0.3f, 0.52f, 0);
    private Vector3 gun6 = new Vector3(-0.3f, 0.52f, 0);

    public BCLoops(EnemyShooting shooting)
    {
        this.shooting = shooting;
        currentDirection = Random.Range(0, 360f);
        nextBulletTime = Time.time;
    }

    public void Update()
    {
        if (Time.time > nextBulletTime)
        {
            currentDirection += 47f;
            if (currentDirection > 360) currentDirection -= 360f;
            shooting.FireBullet(currentDirection, 5f, gun1);
            shooting.FireBullet(currentDirection, 5f, gun2);
            shooting.FireBullet(currentDirection, 5f, gun3);
            shooting.FireBullet(currentDirection, 5f, gun4);
            shooting.FireBullet(currentDirection, 5f, gun5);
            shooting.FireBullet(currentDirection, 5f, gun6);

            shooting.FireBullet(currentDirection, -5f, gun1);
            shooting.FireBullet(currentDirection, -5f, gun2);
            shooting.FireBullet(currentDirection, -5f, gun3);
            shooting.FireBullet(currentDirection, -5f, gun4);
            shooting.FireBullet(currentDirection, -5f, gun5);
            shooting.FireBullet(currentDirection, -5f, gun6);

            nextBulletTime += cooldown;

        }
    }
}

class BCWaves : BulletController
{
    private bool movingRight;
    private float currentDirection;
    private float cooldown = 0.14f;
    private float nextBulletTime = 0f;
    private EnemyShooting shooting;

    private float leftStart = 170;
    private float leftEnd = 280f;
    private float rightStart = 10f;
    private float rightEnd = 260f;

    private float angularSpeed = 13f;

    public BCWaves(EnemyShooting shooting)
    {
        this.shooting = shooting;
        nextBulletTime = Time.time;
    }

    public void Update()
    {
        if (Time.time > nextBulletTime)
        {
            if (movingRight)
            {
                currentDirection += angularSpeed;
                if (currentDirection > 360) currentDirection -= 360f;

                if (currentDirection > leftEnd)
                {
                    movingRight = false;
                    currentDirection = rightStart;
                }
            }
            else
            {
                currentDirection -= angularSpeed;
                if (currentDirection < 0) currentDirection += 360f;

                if (currentDirection < rightEnd && currentDirection > 180)
                {
                    movingRight = true;
                    currentDirection = leftStart;
                }
            }
            shooting.FireBullet(currentDirection, 5f);

            nextBulletTime += cooldown;
        }
    }
}

class BCSpiral : BulletController
{
    private float currentDirection1;
    private float currentDirection2;
    private float cooldown = 0.2f;
    private float nextBulletTime = 0f;
    private EnemyShooting shooting;

    public BCSpiral(EnemyShooting shooting)
    {
        this.shooting = shooting;
        currentDirection1 = Random.Range(0, 360f);
        currentDirection2 = Random.Range(0, 360f);
        nextBulletTime = Time.time;
    }

    public void Update()
    {
        if (Time.time > nextBulletTime)
        {
            currentDirection1 += 29;
            if (currentDirection1 > 360) currentDirection1 -= 360f;
            currentDirection2 -= 37;
            if (currentDirection2 < 0) currentDirection2 += 360f;

            shooting.FireBullet(currentDirection1, 5f);
            shooting.FireBullet(currentDirection2, 5f);

            nextBulletTime += cooldown;
        }
    }
}

class BCBurst : BulletController
{
    private int burstShotsLeft = 0;
    private float currentDirection;
    private float cooldown = 0.3f;
    private float burstCooldown = 1f;
    private float nextBulletTime = 0f;
    private EnemyShooting shooting;

    public BCBurst(EnemyShooting shooting)
    {
        this.shooting = shooting;
        nextBulletTime = Time.time;
    }

    public void Update()
    {
        if (burstShotsLeft <= 0)
        {
            if (Time.time > nextBulletTime)
            {
                burstShotsLeft = 3;
                currentDirection = Random.Range(0, 360);
            }
        }
        if (Time.time > nextBulletTime)
        {
            currentDirection += 120;

            for (int i = 0; i < 10; ++i)
            {
                shooting.FireBullet(currentDirection + Random.Range(0,50), 4f + Random.Range(0,1.5f));
            }
            burstShotsLeft--;
            if (burstShotsLeft <= 0)
            {
                nextBulletTime = Time.time + burstCooldown;
            }
            else
            {
                nextBulletTime = Time.time + cooldown;
            }
        }
    }
}
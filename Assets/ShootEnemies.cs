using UnityEngine;
using System.Collections.Generic;

public class ShootEnemies : MonoBehaviour
{
	public List<GameObject> enemiesInRange;

	private float lastShotTime;
	private MonsterData monsterData;


	public void Start( )
	{
		enemiesInRange = new List<GameObject>();
		lastShotTime = Time.time;
		monsterData = gameObject.GetComponentInChildren<MonsterData>();
	}

	void Shoot(Collider2D target)
	{
		GameObject bulletPrefab = monsterData.CurrentLevel.bullet;
		//1
		Vector3 startPosition = gameObject.transform.position;
		Vector3 targetPosition = target.transform.position;
		startPosition.z = bulletPrefab.transform.position.z;
		targetPosition.z = bulletPrefab.transform.position.z;

		//2
		GameObject newBullet = (GameObject) Instantiate(bulletPrefab);
		newBullet.transform.position = startPosition;
		BulletBehavior bulletComp = newBullet.GetComponent<BulletBehavior>();
		bulletComp.target = target.gameObject;
		bulletComp.startPosition = startPosition;
		bulletComp.targetPosition = targetPosition;

		//3
		Animator animator = monsterData.CurrentLevel.visualization.GetComponent<Animator>();
		animator.SetTrigger("fireShot");
		AudioSource audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.PlayOneShot(audioSource.clip);
	}
	
	public void Update( )
	{
		GameObject target = null;

		float minimalEnemyDistance = float.MaxValue;
		foreach (var enemy in enemiesInRange)
		{
			float distanceToGoal = enemy.GetComponent<MoveEnemy>().DistanceToGoal();
			if (distanceToGoal < minimalEnemyDistance)
			{
				target = enemy;
				minimalEnemyDistance = distanceToGoal;
			}
		}

		if (target != null)
		{
			if (Time.time - lastShotTime > monsterData.CurrentLevel.fireRate)
			{
				Shoot(target.GetComponent<Collider2D>());
				lastShotTime = Time.time;
			}
			Vector3 direction = gameObject.transform.position - target.transform.position;
			gameObject.transform.rotation = Quaternion.AngleAxis( Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI, new Vector3(0, 0, 1));
		}
	}

	private void OnEnemyDestroy( GameObject enemy )
	{
		enemiesInRange.Remove( enemy );
	}

	private void OnTriggerEnter2D( Collider2D other )
	{
		if (other.gameObject.tag.Equals( "Enemy" ))
		{
			enemiesInRange.Add( other.gameObject );
			EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
			del.enemyDelegate += OnEnemyDestroy;
		}
	}

	private void OnTriggerExit2D( Collider2D other )
	{
		if(other.gameObject.tag.Equals( "Enemy" ))
		{
			enemiesInRange.Remove( other.gameObject );
			EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
			del.enemyDelegate -= OnEnemyDestroy;
		}
	}
}

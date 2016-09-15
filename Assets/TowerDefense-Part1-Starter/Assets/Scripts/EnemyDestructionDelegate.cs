using UnityEngine;
using System.Collections;

public class EnemyDestructionDelegate : MonoBehaviour
{
	public delegate void EnemyDelegate(GameObject enemy);
	public EnemyDelegate enemyDelegate;

	public void Start ()
	{
		
	}
	
	public void Update ()
	{
	
	}

	void OnDestroy()
	{
		if (enemyDelegate != null)
		{
			enemyDelegate(gameObject);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
	// enemy prefab object
	public GameObject enemySpawn = null;
	// jednotka casu ve vterinach po ktere se ma enemy respawnout
	public float respawnDelay = 4.0f;
	
	
	private float respawnTime = 0.0f;

	//registrujeme event
	private void OnEnable()
	{
		EnemyController.onEnemyDeath += scheduleRespawn;
	}

	private void OnDisable()
	{
		EnemyController.onEnemyDeath -= scheduleRespawn;
	}
	
	// metoda naplanuje respawn objektu na zaklade nahodnosti
	private void scheduleRespawn(int addscore)
	{
		if (Random.Range(0,10) < 5)
		{
			return;
		}
		// nastavime cas
		respawnTime = Time.time + respawnDelay;
	}

	private void Update()
	{
		// pokud je nastaven respawnTime, tak po case spawni enemy.
		// pri crossPlatform pristupu predelat na objectpoling!
		if (respawnTime > 0.0f)
		{
			if (respawnTime < Time.time)
			{
				respawnTime = 0.0f;
				// vytvoreni noveho objektu, ktery priradime v editoru
				GameObject newEnemy = Instantiate(enemySpawn) as GameObject;
				// nastaveni pozice na pozici aktualniho objektu
				newEnemy.transform.position = transform.position;
			}
		}
	}
}

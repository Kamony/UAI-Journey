using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
	// atributy pohybu
	public float walkingSpeed = 0.45f;
	// atributy posmechu
	public float timeToMock = 2f;
	
	
	private bool walkingLeft = true;

	private bool isMocking = false;
	private Animator enemyAnimator = null;
	private float time = 0;
	
	// objekt registrujici zasah kulkou hrace 
	public EnemyDamageListener bulletCollisionListener = null;
	
	
	// delegat registrujici smrt objektu enemy
	public delegate void EnemyDeath(int addScore);
	public static event EnemyDeath onEnemyDeath;

	private void Awake()
	{
		enemyAnimator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		PlayerStateListener.onDeadAction += playerMockery;
		bulletCollisionListener.hitByBullet += hitByPlayerBullet;
	}

	private void hitByPlayerBullet()
	{
		// aktualizuj event - score + 5;
		if (onEnemyDeath != null)
		{
			onEnemyDeath(5);
		}	
		// znic objekt - enemy
		Destroy(gameObject, 0.1f);
	}

	private void OnDisable()
	{
		PlayerStateListener.onDeadAction -= playerMockery;
		bulletCollisionListener.hitByBullet -= hitByPlayerBullet;
	}
	
	// posmesek pri smrti hrace
	private void playerMockery()
	{
		if (!isMocking)
		{
			transform.Find("haha").gameObject.SetActive(true);
			time = Time.time + timeToMock;
		}		
		isMocking = true;
	}

	// start funkce je volana pred zacatkem kazdeho framu
	private void Start()
	{
			// nahodne rozhodneme o smeru chuze
			walkingLeft = (Random.Range(0, 2) == 1);
			updateWalkOrientation();
	}

	private void Update()
	{
		if (walkingLeft)
			{
				// pohyb vlevo
				transform.Translate(new Vector3(walkingSpeed * Time.deltaTime, 0.0f, 0.0f));
			}
			else
			{
				// pohyb vpravo
				transform.Translate(new Vector3(-1.0f * walkingSpeed * Time.deltaTime, 0.0f, 0.0f));
			}	

		if (isMocking && time < Time.time)
		{
			isMocking = !isMocking;
			transform.Find("haha").gameObject.SetActive(false);
		}
		
	}

	// synchronizace spritu a smeru chuze - vizualni efekt
	private void updateWalkOrientation()
	{
		// info o pozici objektu
		Vector3 localScale = transform.localScale;

		if (walkingLeft)
		{
			if (localScale.x > 0.0f)
			{
				localScale.x = localScale.x * -1.0f;
				transform.localScale = localScale;
			}
		}
		else
		{
			if (localScale.x < 0.0f)
			{
				localScale.x = localScale.x * -1.0f;
				transform.localScale = localScale;
			}
		}
	}

	// zmena smeru
	public void switchDirections()
	{
		walkingLeft = !walkingLeft;
		updateWalkOrientation();
	}
	
	
}

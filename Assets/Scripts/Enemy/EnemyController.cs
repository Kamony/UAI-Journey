using System.Collections;
using System.Collections.Generic;
using Anima2D;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
	
	[SerializeField] private float health = 2;	
	// atributy pohybu
	[SerializeField] private float walkingSpeed = 0.45f;
	// atributy posmechu
	[SerializeField] private float timeToMock = 2f;
	
	private bool walkingLeft = true;

	private bool isMocking = false;
	private Animator enemyAnimator = null;
	private float time = 0;

	private SpriteMeshInstance[] BodyRenders;
	
	// objekt registrujici zasah kulkou hrace 
	public EnemyDamageListener bulletCollisionListener = null;
	
	// delegat registrujici smrt objektu enemy
	public delegate void EnemyDeath(int addScore);
	public static event EnemyDeath onEnemyDeath;

	private void Awake()
	{
		enemyAnimator = GetComponent<Animator>();
		BodyRenders = GetComponentsInChildren<SpriteMeshInstance>();
	}

	private void OnEnable()
	{
		PlayerStateListener.onDeadAction += playerMockery;
		bulletCollisionListener.hitByBullet += hitByPlayerBullet;
	}

	private void hitByPlayerBullet()
	{
		health--;
		if (health <=0)
		{
			// aktualizuj event - score + 5;
			if (onEnemyDeath != null)
			{
				onEnemyDeath(5);
			}
			StartCoroutine(death());

		}
		foreach (SpriteMeshInstance render in BodyRenders)
		{
			render.color = Color.Lerp(render.color,new Color(9,255,0),Time.deltaTime);
		}
		walkingSpeed = walkingSpeed * 0.5f;

	}
	
	// znic objekt - enemy
	private IEnumerator death()
	{
		enemyAnimator.SetTrigger("Death");
		yield return new WaitForSeconds(0.5f);
		// promenna uchovavajici dosavadni stav zabitych nepratel
		GameManager.Instance.NumberOfEnemiesDestroyed += 1;
		Destroy(gameObject);
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
		if (Raycasting())
		{
			switchDirections();
		}
		
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
	// metoda vysila paprsek z objektu a kontroluje kolizi s objektem Border
	private bool Raycasting()
	{
		Vector3 direction;
		if (walkingLeft)
		{
			direction = new Vector3(transform.position.x + 1,transform.position.y);	
		} else direction = new Vector3(transform.position.x - 1,transform.position.y);
		 
		Debug.DrawLine(transform.position, direction);

		return Physics2D.Linecast(transform.position, direction, 1 << LayerMask.NameToLayer("Border"));
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

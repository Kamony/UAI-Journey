using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets._2D;

public class MathBossController : MonoBehaviour
{

	public delegate void bossEventHadnler(int score);

	public static event bossEventHadnler bossDeath;
	
	public delegate void CameraTargerChange();

	public static event CameraTargerChange onCameraTargetChange;

	
	private Camera2DFollow bossCameraTarget;
	private Transform currTargetTransform;
	private float numberOfTriggers = 0f;
	
	
	public GameObject inActiveNode = null;
	public GameObject dropToStartNode = null;
	public GameObject dropFXSpawnPoint = null;
	public List<GameObject> dropNodeList = new List<GameObject>();

	public GameObject bossDeathFX = null;
	public GameObject bossDropFX = null;
	public GameObject Canvas;


	public GameObject portal = null;
	
	private Slider healthBar;

	public float MoveSpeed = 0.1f;
	public float eventWaitDelay = 3f;

	public int enemiesToStartBattle = 10;
	
	public enum bossEvents
	{
		inactive = 0,
		fallingToNode,
		waitingToJump,
		waitingToFall,
		jumpingOffPlatform
		
	}

	public bossEvents CurrEvent = bossEvents.inactive;

	private GameObject targetNode = null;
	private float timeForNextEvent = 0.0f;
	private Vector3 targetPosition = Vector3.zero;

	public int health = 20;

	private Animator animController;
	private int startHealth = 20;
	private bool isDead = false;
	private int enemiesLeftToKill = 0;


	private void OnEnable()
	{
		EnemyController.onEnemyDeath += enemyDied;
		
		
	}
	
	private void OnDisable()
	{
		EnemyController.onEnemyDeath -= enemyDied;
		
	}

	private void Awake()
	{
		animController = GetComponent<Animator>();

		healthBar = FindObjectOfType<Slider>();
		bossCameraTarget=Camera.main.GetComponent<Camera2DFollow>();
		currTargetTransform = bossCameraTarget.target;

	}

	private void Start()
	{
		transform.position = inActiveNode.transform.position;
		enemiesLeftToKill = enemiesToStartBattle;

		Canvas.GetComponentInChildren<Slider>().maxValue = health;
		Canvas.GetComponentInChildren<Slider>().value = health;
	}

	private void enemyDied(int addscore)
	{
		if (CurrEvent == bossEvents.inactive)
		{
			enemiesLeftToKill -= 1;
			if (enemiesLeftToKill <= 0)
			{
				beginBossBattle();	
			}
		}
	}

	private void hitByPlayerBullet()
	{
		health -= 1;
		Canvas.GetComponentInChildren<Slider>().value--;
	
		if (health <= 0)
		{
			killBoss();	
		}
	}

	private void killBoss()
	{
		GameObject deathFxParticle = (GameObject) Instantiate(bossDeathFX);
		deathFxParticle.transform.position = dropFXSpawnPoint.transform.position;
		if (bossDeath != null)
		{
			bossDeath(1000);
		}
		portal.SetActive(true);
		
		Destroy(gameObject);
		
	}


	// Update is called once per frame
	void Update () {
		Debug.Log(enemiesLeftToKill);
		switch (CurrEvent)
		{
			case bossEvents.inactive:
				break;
			case bossEvents.fallingToNode:
			{
				if (transform.position.y > targetNode.transform.position.y)
				{
					transform.Translate(new Vector3(0f, -MoveSpeed * Time.deltaTime, 0f));

					if (transform.position.y < targetNode.transform.position.y)
					{
						Vector3 targetPos = targetNode.transform.position;
						transform.position = targetPos;
					}
				}
				else
				{
					createDropFX();
					timeForNextEvent = 0.0f;
					CurrEvent = bossEvents.waitingToJump;
				}
			}
				break;
			case bossEvents.waitingToFall:
			{
				if (timeForNextEvent == 0.0f)
				{
					timeForNextEvent = Time.time + eventWaitDelay;
				}
				else if (timeForNextEvent < Time.time)
				{
					targetNode = dropNodeList[Random.Range(0, dropNodeList.Count)];
					transform.position = getSkyPositionOfNode(targetNode);
					CurrEvent = bossEvents.fallingToNode;
					timeForNextEvent = 0.0f;
				}
			}
				break;
			case bossEvents.waitingToJump:
			{
				
				if (timeForNextEvent == 0.0f)
				{
					timeForNextEvent = Time.time + eventWaitDelay;
					animController.SetBool("Spread",true);
				}
				else if (timeForNextEvent < Time.time)
				{
					animController.SetBool("Spread",false);
					targetPosition = getSkyPositionOfNode(targetNode);
					CurrEvent = bossEvents.jumpingOffPlatform;
					timeForNextEvent = 0.0f;
					targetNode = null;
				}
			}
				break;
			case bossEvents.jumpingOffPlatform:
			{
				if (transform.position.y < targetPosition.y)
				{
					transform.Translate(new Vector3(0f, MoveSpeed * Time.deltaTime, 0f));

					if (transform.position.y > targetPosition.y)
					{
						transform.position = targetPosition;
					}
					else
					{
						
						timeForNextEvent = 0.0f;
						CurrEvent = bossEvents.waitingToFall;
					}
				}
			}
				break;
		}
	}


	public void beginBossBattle()
	{
		if (onCameraTargetChange != null)
		{
			onCameraTargetChange();
		}
		
		bossCameraTarget.target = this.transform;
		targetNode = dropToStartNode;
		CurrEvent = bossEvents.fallingToNode;

		timeForNextEvent = 0.0f;
		startHealth = health;
		Canvas.SetActive(true);
	}
	
	
	private Vector3 getSkyPositionOfNode(GameObject o)
	{
		Vector3 targetPosition = targetNode.transform.position;
		targetPosition.y += 9f;
		return targetPosition;
	}

	private void createDropFX()
	{
		GameObject dropFxParticle = (GameObject) Instantiate(bossDropFX);
		dropFxParticle.transform.position = dropFXSpawnPoint.transform.position;
		Destroy(dropFxParticle, 2);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("PlayerBullet"))
		{
			hitByPlayerBullet();
		}
		if (other.CompareTag("SpecialTrigger"))
		{
			
		}
		
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("SpecialTrigger"))
		{
			bossCameraTarget.target = currTargetTransform;			
		}
	}
}

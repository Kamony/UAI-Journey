using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : TrapController
{
	
	[SerializeField] private float delay = 1f;
	[SerializeField] private float durationOfLaser = 0.5f;
	[SerializeField] private float chanceOfFire = 30f;
	
	private GameObject laser = null;
	private bool laserActive = false;
	private float timeOfShoot = 0f;
	
	private void Awake()
	{
		laser = transform.Find("Laser").gameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float random = Random.Range(0,100);

		if (laserActive == false && (timeOfShoot+delay) < Time.time)
		{
			if (fireing(random))
			{
				startFireing();
				timeOfShoot = Time.time;
			}
		}
		else
		{
			if ((timeOfShoot + durationOfLaser) < Time.time)
			{
				stopFireing();
				timeOfShoot = Time.time;
			}
			
		}
		
		
		
	}

	private void startFireing()
	{
		laser.SetActive(true);
		laserActive = true;
	}

	private void stopFireing()
	{
		laser.SetActive(false);
		laserActive = false;
	}
	
	private bool fireing(float random)
	{
		return (random < chanceOfFire);
	}
	
}

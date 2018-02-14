using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathBossPageController : MonoBehaviour
{

	public int Health = 2;
	public GameObject pageDeathFX;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("PlayerBullet"))
		{
			Health--;
			if (Health <= 0)
			{
				createFX();
				Destroy(gameObject);
			}
		}
	}

	
	private void createFX()
	{
		GameObject FxParticle = (GameObject) Instantiate(pageDeathFX);
		
		FxParticle.transform.position = transform.position;
		
		Destroy(FxParticle, 2); 
	}

}

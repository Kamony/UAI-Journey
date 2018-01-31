using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
	public delegate void CheckPointChange(GameObject newPosition);

	public static event CheckPointChange onCheckpointChange;



	private GameObject flag = null;


	private float flagTravel = 0;

	private void Awake()
	{
		flag = transform.Find("Flag").gameObject;
	}


	private void Update()
	{
		if (flag.activeInHierarchy && flagTravel < 2.7f)
		{
			flag.transform.position = new Vector3(flag.transform.position.x,flag.transform.position.y +0.1f,0);
			flagTravel += 0.1f;
		}
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (onCheckpointChange != null)
			{
				onCheckpointChange(this.gameObject);
				flag.SetActive(true);
			}			
		}
	}
}

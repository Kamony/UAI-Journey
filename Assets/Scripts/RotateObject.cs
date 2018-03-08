using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{

	[SerializeField] private float angle = 0f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale == 1f)
		{
			transform.Rotate(0,0,angle);
		}
		
	}
}

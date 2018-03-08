using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
	[SerializeField] private bool horizontal = false;
	[SerializeField] private float speed = 1f;
	
	[SerializeField] private Collider2D upBorder = null;
	[SerializeField] private Collider2D DownBorder = null;
	

	private bool isFalling = false;


	// Update is called once per frame
	void FixedUpdate () {

		if (horizontal)
		{
			if (isFalling)
			{
				moveRight();
			}
			else
			{
				moveLeft();
			}
		}
		else
		{
			if (isFalling)
			{
				moveDown();
			}
			else
			{
				moveUp();
			}
		}
		
		
	}

	private void moveUp()
	{
		transform.Translate(Vector3.up * (speed + Time.deltaTime));
	}

	private void moveDown()
	{
		transform.Translate(Vector3.down * (speed + Time.deltaTime) );
	}

	private void moveLeft()
	{
		transform.Translate(Vector3.left * (speed + Time.deltaTime) );
	}

	private void moveRight()
	{
		transform.Translate(Vector3.right * (speed + Time.deltaTime) );
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Border"))
		{
			if (isFalling)
			{
				isFalling = false;
				return;
			}
			isFalling = true;
		}
	}
}

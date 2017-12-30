using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuideWatcher : MonoBehaviour
{
	// enemy game object
	public EnemyController enemyObj;

	public float RaycastLength = 3;

	private bool isColliding = false;
	
	// pokud watcher opusti platformu mel by se otocit, aby nespadl
/*	private void OnTriggerExit2D(Collider2D other)
	{		
		
		if (other.CompareTag("Platform"))
		{
			if (isColliding)
			{
				return;
			}
			
			enemyObj.switchDirections();
			isColliding = true;

		}
	
	}*/

	/*private void OnTriggerEnter2D(Collider2D other)
	{

		if (other.tag == "Border")
		{
			
			if (isColliding)
			{
				Debug.Log("ted");
				return;
			}
			enemyObj.switchDirections();
			isColliding = true;

		}
	}*/

	private void Update()
	{
//		Vector2 direction;
//		direction = enemyObj.getWalkDirection() ? Vector2.left : Vector2.right;
//		Debug.DrawRay(transform.position,direction,Color.red);
//
//		if (Physics2D.Raycast(transform.position,direction,RaycastLength).collider.CompareTag("Border"))
//		{
//			enemyObj.switchDirections();
//		}
//		isColliding = false;
	}
}

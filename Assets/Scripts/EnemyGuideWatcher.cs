using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuideWatcher : MonoBehaviour
{
	// enemy game object
	public EnemyController enemyObj;
	
	
	// pokud watcher opusti platformu mel by se otocit, aby nespadl
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Platform"))
		{
			enemyObj.switchDirections();
		}
	}
}

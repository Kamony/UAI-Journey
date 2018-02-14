using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionListener : MonoBehaviour
{

	public PlayerStateListener targetStateListener = null;
	
	
	public delegate void SceneChange();
	public static event SceneChange onSceneChange;
	
	
	private void OnTriggerEnter2D(Collider2D collidedObj)
	{
		switch (collidedObj.tag)
		{
			// v pripade kolize s platformou zmen stav
				case "Platform":
					targetStateListener.onStateChange(PlayerStateController.playerStates.landing);
					break;
			// v pripade padu hrace z platformy a kolize s deathTriggrem
				case "DeathTrigger"	:
					targetStateListener.onStateChange(PlayerStateController.playerStates.kill);
					break;
				case "Boss" :
					targetStateListener.onStateChange(PlayerStateController.playerStates.takingDMG);
					break;
				case "Enemy" :
					targetStateListener.onStateChange(PlayerStateController.playerStates.takingDMG);
					break;
				case "Scene" :
					if (onSceneChange != null)
					{
						onSceneChange();
					}
					break;
					
		}
	}
	
	
	
	private void OnTriggerExit2D(Collider2D collidedObj)
	{
		// v pripade zruseni kolize - padani napr.
		switch (collidedObj.tag)
		{
			case "Platform":
			{
					targetStateListener.onStateChange(PlayerStateController.playerStates.falling);
				break;
			}	
		}
	}

}

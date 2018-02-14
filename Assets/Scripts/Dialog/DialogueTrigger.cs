using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

	public Dialogue dialogue;
	
	public void triggerDialogue()
	{
		DialogueManager.Instance.startDialogue(dialogue);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			triggerDialogue();
			Destroy(gameObject);
		}
	}
}

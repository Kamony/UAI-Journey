using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DialogueManager : MonoBehaviour
{

	public static DialogueManager Instance;

	public TextMeshProUGUI TextArea;

	private Animator anim;
	
	private Queue<string> sentences;

	private bool isOpen = false;
	
	private float timer;
	
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{
		Instance = this;
		sentences = new Queue<string>();
		anim = GetComponent<Animator>();
	}

	public void startDialogue(Dialogue dialogue)
	{
		Time.timeScale = 0f;
		
		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}
		
		DisplayNextSentence();
	
	}

	public void DisplayNextSentence()
	{
		if (anim == null)
		{
			Debug.Log("Animator nenalezen");
		}
		anim.SetBool("Open",true);
		
		if (sentences.Count == 0)
		{
			endDialogue();
			return;
		}

		string line = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(typeSentence(line));
	}

	IEnumerator typeSentence(string sentence)
	{
		TextArea.text = "";
		foreach (char c in sentence.ToCharArray())
		{
			TextArea.text += c;
			yield return null;
		}
		isOpen = true;
	}
	
	private void endDialogue()
	{
		
		anim.SetBool("Open",false);
		
		isOpen = false;
		Time.timeScale = 1f;
	}


	private void Update()
	{
		if (isOpen)
		{
			if (Input.touchCount == 1)
			{
				if (Input.GetTouch(0).phase == TouchPhase.Began)
				{
					DisplayNextSentence();
				}
			}
		}
	}
}

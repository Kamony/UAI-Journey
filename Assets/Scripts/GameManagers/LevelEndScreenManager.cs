using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEndScreenManager : MonoBehaviour
{
	public static LevelEndScreenManager Instance;
	
	[SerializeField] private TextMeshProUGUI deaths;
	[SerializeField] private TextMeshProUGUI score;
	[SerializeField] private TextMeshProUGUI timeSpent;
	[SerializeField] private Button btn;
	[SerializeField] private Animator anim;
	
	private int deathCounter = 0;
	private int scoreCounter = 0;
	private int timeCounter = 0;


	private void OnEnable()
	{
		PlayerStateListener.onDeadAction += countDeaths;
		PlayerCollisionListener.onSceneChange += showScreen;
		SceneManager.sceneLoaded += checkForActiveScreen;
		Debug.Log("Subscribe");
	}

	private void checkForActiveScreen(Scene arg0, LoadSceneMode arg1)
	{
		if (anim.GetBool("Open"))
		{
			anim.SetBool("Open",false);
		}
	}

	private void OnDisable()
	{
		PlayerStateListener.onDeadAction -= countDeaths;
		PlayerCollisionListener.onSceneChange -= showScreen;
	}

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			DontDestroyOnLoad(gameObject);
			Debug.Log("Dont Destroy On Load");
			Instance = this;
		}	
	}

	private void showScreen()
	{
		setGUIStats();
		
		GameManager.Instance.toggleTouchInput();
		
		anim.SetBool("Open",true);
		
		Time.timeScale = 0f;
		
	}

	private void Start()
	{
		btn.onClick.AddListener(onClickAction);
	}

	public void onClickAction()
	{
		Debug.Log("Click action");
		
		anim.SetBool("Open",false);
		
		Time.timeScale = 1f;
		
		reset();

		if (!anim.GetBool("Open"))
		{
			Debug.Log(anim.GetBool("Open"));
			GameManager.Instance.LoadNextLevel();	
		}
		
	}

	private void countDeaths()
	{
		deathCounter += 1;
	}

	public void setGUIStats()
	{
		scoreCounter = PlayerScoreWatcher.score;
		timeCounter = Mathf.RoundToInt(Time.timeSinceLevelLoad);
		
		
		deaths.text = deathCounter.ToString();
		score.text = scoreCounter.ToString();
		timeSpent.text = timeCounter.ToString() + " sec";
	}

	public void reset()
	{
		deathCounter = 0;
	}

}

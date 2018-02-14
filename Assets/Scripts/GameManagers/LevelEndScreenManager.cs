using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelEndScreenManager : MonoBehaviour
{
	public static LevelEndScreenManager Instance;
	
	
	public TextMeshProUGUI deaths;
	public TextMeshProUGUI score;
	public TextMeshProUGUI timeSpent;

	private int deathCounter = 0;
	private int scoreCounter = 0;
	private int timeCounter = 0;


	
	private void Start()
	{
		Instance = this;
		//DontDestroyOnLoad(gameObject);
	}

	private void OnEnable()
	{
		PlayerStateListener.onDeadAction += countDeaths;
	}

	private void OnDisable()
	{
		PlayerStateListener.onDeadAction -= countDeaths;
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

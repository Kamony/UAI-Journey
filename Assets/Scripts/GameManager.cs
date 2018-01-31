using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// Singleton pro spravu scen
public class GameManager : MonoBehaviour
{
	// uplatnime Singleton pristup, abychom zabranili duplikaci objektu ve hre
	public static GameManager Instance;

	public  bool isSaved = false;

	public  bool resumePressed = false;

	public int numberOfEnemiesDestroyed = 0;


	public GameObject TouchInput;
	public GameObject loadingScreen;
	public Slider slider;
	
	public delegate void SaveGameStats(DataStructure ds);
	public static SaveGameStats onLoadAttempt;

	public DataStructure ds;
	public bool vibrationEnabled = true;

	private void Awake()
	{
		ds = new DataStructure();

	}

	private void OnEnable()
	{
		PlayerCollisionListener.onSceneChange += LoadNextLevel;
		SceneManager.sceneLoaded += initPlayer;
		PlayerStateListener.onDeadAction += PreserveData;
	}


	private void OnDisable()
	{
		PlayerCollisionListener.onSceneChange -= LoadNextLevel;
		SceneManager.sceneLoaded -= initPlayer;
		PlayerStateListener.onDeadAction -= PreserveData;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			SaveGame();
			//SaveAsync();
		}
	}

	private void OnApplicationQuit()
	{
		SaveGame();
	}

	private void initPlayer(Scene arg0, LoadSceneMode arg1)
	{
		loadingScreen.SetActive(false);		
		if (arg0.buildIndex > 0 && arg0.isLoaded)
		{
			
			TouchInput.SetActive(true);
			
			if (resumePressed)
			{
				if (onLoadAttempt != null)
				{
					onLoadAttempt(ds);
				}
			}
			StartCoroutine(updateEnemyStateInGame());
			return;
		}
		Debug.Log("Cannot be loaded");
	}

	void Start () {
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		
	}
	
	// nahraje dalsi level a ulozi si data
	private void LoadNextLevel()
	{
		TouchInput.SetActive(false);
		PreserveData();
		StartCoroutine(loadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));
	}
	// ulozime stav hry a prejdeme do hlavniho menu
	public void LoadMainMenu()
	{
		TouchInput.SetActive(false);
		PreserveData();
		StartCoroutine(loadAsynchronously(0));
	}
	
	// ulozime data do vytvorene tridy pro serializaci
	public void PreserveData()
	{
		int scene = SceneManager.GetActiveScene().buildIndex;
		// nechci ukladat v hlavnim menu
		if (scene > 0)
		{
			ds.score = PlayerScoreWatcher.score;
			ds.health = PlayerScoreWatcher.health;
			ds.sceneNo = scene;
			Transform playerPos = FindObjectOfType<PlayerStateListener>().playerRespawnPoint.transform;
			ds.playerPosX = playerPos.position.x;
			ds.playerPosY = playerPos.position.y;
			ds.numberOfDestroyedEnemies = numberOfEnemiesDestroyed;
			isSaved = true;
		}
		isSaved = false;
	}
	
	// ulozime hru do souboru
	public void SaveGame()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gameSave.dat");
		
		PreserveData();
		if (isSaved)
		{
			bf.Serialize(file,ds);
		}
		
		file.Close();
	}


	public void SaveAsync()
	{
		Thread thread = new Thread(SaveGame);
		thread.Start();
		
	}
	
	// asynchrone nahraje dalsi level, v pripade volani z hlavniho menu  - level 1
	public void LoadGame()
	{
		TouchInput.SetActive(false);
		StartCoroutine(loadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));

	}
	
	// asynchronni volani sceny a update slideru pro visualni efekt
	IEnumerator loadAsynchronously(int sceneNumber)
	{
		loadingScreen.SetActive(true);
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNumber);
		
		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / 0.9f);
			Debug.Log(progress);
			slider.value = progress;
			yield return null;
		}
	}
	
	// odebereme ze sceny nepratele, aby je nebylo mozno zabit vickrat
	IEnumerator updateEnemyStateInGame()
	{
		// mame jednotnou politiku davat nepratele do gameobjectu BEERS
		GameObject enemiesContainer = GameObject.Find("/Enemies/Beers");
		for (int i = 0; i < ds.numberOfDestroyedEnemies; i++)
		{
			enemiesContainer.transform.GetChild(i).gameObject.SetActive(false);
			yield return null;
		}
		
	}
	
	//nacteme hru ze souboru
	public void LoadSavedGame()
	{
		loadingScreen.SetActive(true);
		if (File.Exists(Application.persistentDataPath + "/gameSave.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gameSave.dat", FileMode.Open);

			ds = (DataStructure) bf.Deserialize(file);

			StartCoroutine(loadAsynchronously(ds.sceneNo));
			file.Close();
			resumePressed = true;
		}
	}
	// nacteme hru jenom z dataStruktury
	public void LoadPreservedGame()
	{
		StartCoroutine(loadAsynchronously(ds.sceneNo));
		resumePressed = true;
	}
}

[Serializable]
public class DataStructure
{
	public float playerPosX;
	public float playerPosY;
	public int sceneNo;
	public int score = 0;
	public int health = 5;
	public int numberOfDestroyedEnemies;
	public int playerDefaultHealth = 5;
}

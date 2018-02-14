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
	public GameObject endingScreen;
	public Slider slider;
	
	public delegate void SaveGameStats(DataStructure ds);
	public static SaveGameStats onLoadAttempt;

	public delegate void NewGame();
	public static NewGame newGame;
	

	public DataStructure ds;
	public bool vibrationEnabled = true;

	
	
	private void Awake()
	{
		ds = new DataStructure();

	}

	private void OnEnable()
	{
		PlayerCollisionListener.onSceneChange += showEndingScreen;
		SceneManager.sceneLoaded += initPlayer;
		PlayerStateListener.onDeadAction += PreserveData;
	}


	private void OnDisable()
	{
		PlayerCollisionListener.onSceneChange -= showEndingScreen;
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
		endingScreen.SetActive(false);
		Time.timeScale = 1f;
		
		if (arg0.buildIndex > 0 && arg0.isLoaded)
		{
			
			TouchInput.SetActive(true);
			global::TouchInput.Input.resetMovement();
			
			if (resumePressed)
			{
				if (onLoadAttempt != null)
				{
					onLoadAttempt(ds);
				}
				resumePressed = false;
				StartCoroutine(updateEnemyStateInGame());
			}
		
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

	private void showEndingScreen()
	{
		Time.timeScale = 0f;
		endingScreen.SetActive(true);
		LevelEndScreenManager.Instance.setGUIStats();
		LevelEndScreenManager.Instance.reset();
	}
	
	
	// nahraje dalsi level a ulozi si data
	public void LoadNextLevel()
	{
		TouchInput.SetActive(false);
		endingScreen.SetActive(false);
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
			PlayerStateListener player = FindObjectOfType<PlayerStateListener>();
			
			ds.score = PlayerScoreWatcher.score;
			ds.health = player.playerHealth;
			ds.sceneNo = scene;
			Transform playerPos = player.playerRespawnPoint.transform;
			ds.playerPosX = playerPos.position.x;
			ds.playerPosY = playerPos.position.y;
			ds.numberOfDestroyedEnemies = numberOfEnemiesDestroyed;
			isSaved = true;
			return;
		}
		isSaved = false;
	}
	
	// ulozime hru do souboru
	public void SaveGame()
	{
		BinaryFormatter bf = new BinaryFormatter();
		
		PreserveData();
		if (isSaved)
		{
			FileStream file = File.Create(Application.persistentDataPath + "/gameSave.dat");
			bf.Serialize(file,ds);
			file.Close();
		}
		
		
	}


	public void SaveAsync()
	{
		Thread thread = new Thread(SaveGame);
		thread.Start();
		
	}
	
	// asynchrone nahraje dalsi level, v pripade volani z hlavniho menu  - level 1
	public void LoadNewGame()
	{
		TouchInput.SetActive(false);
		// delegat vyhlasi stav nova hra
		if (newGame != null)
		{
			newGame();
		}
		
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

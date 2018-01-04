using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// Singleton pro spravu scen
public class GameManager : MonoBehaviour
{

	// uplatnime Singleton pristup, abychom zabranili duplikaci objektu ve hre
	static GameManager Instance;

	
	private void OnEnable()
	{
		PlayerCollisionListener.onSceneChange += LoadNextLevel;
	}

	private void OnDisable()
	{
		PlayerCollisionListener.onSceneChange += LoadNextLevel;
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

	private void LoadNextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		
	}
	
}

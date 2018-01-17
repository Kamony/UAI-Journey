using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public Button resumeButton;

	private GameManager _gm;

	private void Awake()
	{
		_gm = FindObjectOfType<GameManager>();
	}


	public void ResumeGame()
	{
		if (GameManager.Instance.isSaved)
		{
			_gm.LoadPreservedGame();
			Debug.Log("Loading preserved game");
		}
		else
		{
			_gm.LoadSavedGame();
			Debug.Log("Loading saved game");
		}
	
	}
	// novy level volame na pozadi
	public void loadLevel()
	{
		FindObjectOfType<GameManager>().LoadGame();
	}
    
	// ukonceni hry
	public void exitGame()
	{
		Debug.Log("QUIT!");
		Application.Quit();
		
	}
	
	private void Start()
	{
		if (File.Exists(Application.persistentDataPath + "/gameSave.dat") || GameManager.Instance.isSaved)
		{
			transform.GetChild(1).transform.position = new Vector3(transform.GetChild(1).transform.position.x -Screen.width/4, transform.GetChild(1).transform.position.y);
			transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x +Screen.width/4, transform.GetChild(0).transform.position.y);
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}
}

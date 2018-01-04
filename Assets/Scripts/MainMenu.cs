using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

	public GameObject loadingScreen;
	public Slider slider;
	
	// novy level volame na pozadi
	public void loadLevel()
	{
		StartCoroutine(loadAsynchronously());
	}
    
	// ukonceni hry
	public void exitGame()
	{
		Debug.Log("QUIT!");
		Application.Quit();
		
	}
	
	// asynchronni volani sceny a update slideru pro visualni efekt
	IEnumerator loadAsynchronously()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
		
		loadingScreen.SetActive(true);
		
		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / 0.9f);
			slider.value = progress;
			yield return null;
		}
	}
}

    using UnityEngine;
    
    public class PauseMenu : MonoBehaviour
    {
        public static bool gameIsPaused = false;

        public GameObject pauseMenuUI;

        public void Pause()
        {
            Time.timeScale = 0f;
            pauseMenuUI.SetActive(true);
            gameIsPaused = true;
        }

        public void Resume()
        {
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
            gameIsPaused = false;
        }

        public void loadMainMenu()
        {
            // save game
            Resume();
            GameManager.Instance.SaveGame();
            GameManager.Instance.LoadMainMenu();
        }

        public void QuitTheGame()
        {
            GameManager.Instance.SaveGame();
            Application.Quit();
        }
        
#if UNITY_EDITOR
        void Update(){
            if (Input.GetKeyDown(KeyCode.Escape))
            {
	            Pause();	
            }	
        }
#endif
        
    }

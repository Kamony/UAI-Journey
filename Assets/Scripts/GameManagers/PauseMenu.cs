    using UnityEngine;
    
    public class PauseMenu : MonoBehaviour
    {


        [SerializeField] private GameObject pauseMenuUI;

        public void Pause()
        {
            Time.timeScale = 0f;
            pauseMenuUI.SetActive(true);
     
        }

        public void Resume()
        {
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
     
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

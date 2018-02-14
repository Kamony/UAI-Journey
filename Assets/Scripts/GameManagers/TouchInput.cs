    
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchInput : MonoBehaviour
{
    public static TouchInput Input;
    
    public static float movement = 0;
    public static float jump = 0;
    public static float fire = 0;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += initReset;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= initReset; 
    }

    private void initReset(Scene arg0, LoadSceneMode arg1)
    {
        resetMovement();
        resetJump();
        resetFire();
    }

    private void Awake()
    {
        if (Input == null)
        {
            Input = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void goLeft()
    {
        movement = -1;
    }

    public void goRight()
    {
        movement = 1;
    }

    public void resetMovement()
    {
        movement = 0;
    }
   
    public void Jump()
    {
        jump = 1;
    }
    public void resetJump()
    {
        jump = 0;
    }

    public void Fire()
    {
        fire = 1;
    }

    public void resetFire()
    {
        fire = 0;
    }

}

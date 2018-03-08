    
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchInput : MonoBehaviour
{
    public static TouchInput Input;

    private float _movement = 0;
    
    public float Movement
    {
        get { return _movement; }
    }

    private float _jump = 0;

    public float Jump
    {
        get { return _jump; }
    }
    
    private float _fire = 0;

    public float Fire
    {
        get { return _fire; }
    }

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
        _movement = -1;
    }

    public void goRight()
    {
        _movement = 1;
    }

    public void resetMovement()
    {
        _movement = 0;
    }
   
    public void DoJump()
    {
        _jump = 1;
    }
    public void resetJump()
    {
        _jump = 0;
    }

    public void DoFire()
    {
        _fire = 1;
    }

    public void resetFire()
    {
        _fire = 0;
    }

}

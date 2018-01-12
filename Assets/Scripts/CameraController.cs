using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    public GameObject playerGameObject = null;
    public float cameraTrackingSpeed = 0.2f;
    public float cameraYAdjustment = 3.0f;
    public float cameraXAdjustment = 3.0f;
    public float pixelToUnits = 16f;
    
    private PlayerStateController.playerStates currentPlayerState;
    private Vector3 lastTargetPosition = Vector3.zero;
    private Vector3 currentTargetPosition = Vector3.zero;
    private float currentLerpDistance = 0.0f;


    private void Start()
    {
        // pozice hrace ve hre
        Vector3 playerPos = playerGameObject.transform.position;
        // pocatecni pozice kamery
        Vector3 cameraPos = transform.position;
        // pozice sledovaneho objektu
        Vector3 startTargetPos = playerPos;

        startTargetPos.z = cameraPos.z;
        
        lastTargetPosition = startTargetPos;
        currentTargetPosition = startTargetPos;
        currentLerpDistance = 1.0f;
    }

    
    private void OnEnable()
    {
        Debug.Log("script was enabled");
        PlayerStateController.onStateChange += onPlayerStateChange;
    }
    private void OnDisable()
    {
        Debug.Log("script was disabled");
        PlayerStateController.onStateChange -= onPlayerStateChange;
    }

    private void onPlayerStateChange(PlayerStateController.playerStates newState)
    {
        currentPlayerState = newState;
        Debug.Log(currentPlayerState.ToString());
    }

     private void LateUpdate()
    {
        onStateCycle();

        currentLerpDistance += cameraTrackingSpeed;
        // mirne posuneme kameru nahoru pro prirozenejsi vzhled ve hre
        currentTargetPosition.y += cameraYAdjustment;

        transform.position = Vector3.Lerp(lastTargetPosition, currentTargetPosition, currentLerpDistance);
    }

    private void onStateCycle()
    {
       // vyuzijeme stavajici stav hrace (sledovaneho objektu) k nadefinovani chovani kamery
        switch (currentPlayerState)
        {
            case PlayerStateController.playerStates.idle:
                trackPlayer();     
                break;
            case PlayerStateController.playerStates.left:
                trackPlayer();
                
                break;
            case PlayerStateController.playerStates.right:
                trackPlayer();
                
                break;
            case PlayerStateController.playerStates.jump:
                trackPlayer();
                break;
            case PlayerStateController.playerStates.firingWeapon:
                trackPlayer();
                break;
        }
        
       
    }
    // kamera nasleduje objekt
    private void trackPlayer()
    {
        // uchovame stavajici pozice
        Vector3 currentCamPos = transform.position;
        Vector3 currentPlayerPos = playerGameObject.transform.position;
        // jestlize jsou pozice kamery a hrace na stejne pozici
        if (currentCamPos.x == currentPlayerPos.x && currentCamPos.y == currentPlayerPos.y)
        {
            // kamera se nehybe
            currentLerpDistance = 1.0f;
            lastTargetPosition = currentCamPos;
            currentTargetPosition = currentCamPos;
            return;
        }
        
//        float rounded_x = RoundToNearestPixel(currentPlayerPos.x);
//        float rounded_y = RoundToNearestPixel(currentPlayerPos.y);
//        
//        currentPlayerPos = new Vector3(rounded_x,rounded_y);
        currentPlayerPos = new Vector3(currentPlayerPos.x,currentPlayerPos.y);
        // resetujeme lerping
        currentLerpDistance = 0.0f;
        // uchovame pozici pro budouci lerping
        lastTargetPosition = currentCamPos;
        // uchovame novou pozici sledovaneho objektu
        currentTargetPosition = currentPlayerPos;
        // zesynchronizujeme z
        currentTargetPosition.z = currentCamPos.z;
  
        
    }

    // jak se zda, tak pri praci s 2d musime zaokrouhlit pixely
    public float RoundToNearestPixel(float unityUnits)
    {
        float valueInPixels = unityUnits * pixelToUnits;
        valueInPixels = Mathf.Round(valueInPixels);
        float roundedUnityUnits = valueInPixels * (1 / pixelToUnits);
        return roundedUnityUnits;
    }
    
    private void stopTrackingPlayer()
    {
        Vector3 currentCamPos = transform.position;
        currentTargetPosition = currentCamPos;
        lastTargetPosition = currentCamPos;
        currentLerpDistance = 1.0f;
    }
}

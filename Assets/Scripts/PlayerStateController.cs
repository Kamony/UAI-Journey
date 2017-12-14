using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    public enum playerStates
    {
        idle = 0,
        left,
        right,
        jump,
        landing,
        falling,
        takingDMG,
        immortal,
        kill,
        resurrect,
        firingWeapon,
        _stateCount
    }
    
    // prodleva mezi stavy - pole velikosti stejne jako playerStates
    public static float[] stateDelayTimer = new float[(int)playerStates._stateCount];
    
    // delegat pro zmenu stavu hrace
    public delegate void playerStateHandler(PlayerStateController.playerStates newState);
    public static event playerStateHandler onStateChange;

    
    
    
    void LateUpdate()
    {
        //if (!GameStates.gameActive)
        //    return;

        // detekuj defaultni input z Uniy a prirad ho do promenne
        float horizontal = Input.GetAxis("Horizontal");
        
        // komunikujeme s ostatnimi tridami pomoci delegata aeventu onStateChange
        if (horizontal != 0.0f)
        {
            if (horizontal < 0.0f)
            {
                if (onStateChange != null)
                    onStateChange(PlayerStateController.playerStates.left);
            }
            else
            {
                if (onStateChange != null)
                    onStateChange(PlayerStateController.playerStates.right);
            }
        }
        else
        {
            if (onStateChange != null)
                onStateChange(PlayerStateController.playerStates.idle);
        }
        // detekuj defaultni input z Uniy a prirad ho do promenne
        float jump = Input.GetAxis("Jump");
        if (jump > 0.0f)
        {
            if (onStateChange != null)
                onStateChange(PlayerStateController.playerStates.jump);
        }
        // detekuj defaultni input z Uniy a prirad ho do promenne
        float firing = Input.GetAxis("Fire1");
        if (firing > 0.0f)
        {
            if (onStateChange != null)
                onStateChange(PlayerStateController.playerStates.firingWeapon);
        }
    }
}




using System;
using System.Collections;
using System.Collections.Generic;
using Anima2D;
using UnityEngine;



[RequireComponent(typeof(Animator))]
public class PlayerStateListener : MonoBehaviour {
    // upravitelne atributy pohybu hrace
    public int playerHealth = 10;
    public float playerWalkSpeed = 3f;
    public float playerJumpForceVertical = 300f;
    public float playerJumpForceHorizontal = 250f;
    // respawn point ktery se priradi v editoru
    public GameObject playerRespawnPoint = null;
    // bullet prefab ktery se priradi v editoru
    public GameObject bulletPrefab = null;
    // pocatecni bod odkud se bude strilet
    public Transform bulletSpawnTransform;

    private Animator playerAnimator = null;
    // predchozi stav hrace, aby se mel kam vratit pri strileni napr.
    private PlayerStateController.playerStates previousState = PlayerStateController.playerStates.idle;
    private PlayerStateController.playerStates currentState = PlayerStateController.playerStates.idle;
    private bool playerHasLanded = true;
    private Rigidbody2D physics;

    private  int PlayerHealthDefault;
    
    // delegat pro vyslani stavu smrti
    public delegate void playerDeadDelegate();
    public static event playerDeadDelegate onDeadAction;

    public delegate void playerTakingDmgDelegate();
    public static event playerTakingDmgDelegate OnTakingDmgAction;
    
    
    
    // prihlasime odber
    private void OnEnable()
    {
        PlayerStateController.onStateChange += onStateChange;
    }

    private void OnDisable()
    {
        PlayerStateController.onStateChange -= onStateChange;
    }

    private void Start()
    {
        physics = GetComponent<Rigidbody2D>();
        // pristup ke komponente rizeni animaci
        playerAnimator = GetComponent<Animator>();
        // nastavime zacatecni prodlevu pro stavy
        PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.jump] = 1.0f;
        PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.firingWeapon] = 1.0f;

        // uchovame nastavene zivoty
        PlayerHealthDefault = playerHealth;
       
    }


    private void Update()
    {
        onStateCycle();
  
    }

    private void FixedUpdate()
    {
        // nastavime animace dle vertikalni polohy
        playerAnimator.SetFloat("vSpeed",physics.velocity.y);
    }

    // !!! metodu jsem nahradil ve tride playerCollisionListener
    public void hitDeathTrigger()
    {
        onStateChange(PlayerStateController.playerStates.kill);
    }

    // cyklus enginu Unity v LateUpdate();
    void onStateCycle()
    {
        // stavajici info objektu hrace - x souradnice nam udava smer 
        Vector3 localScale = transform.localScale;
        // vynulejeme stavajici rotace na vsech osach 
        transform.localEulerAngles = Vector3.zero;
        // podle aktualniho stavu proved akci
        switch (currentState)
        {
            case PlayerStateController.playerStates.idle:
                
                break;

            case PlayerStateController.playerStates.left:
                // pohneme objektem doleva
                transform.Translate(new Vector3((playerWalkSpeed * -1.0f) * Time.deltaTime, 0.0f, 0.0f));
                // prohodime grafiku, aby se hrac koukal smerem pohybu
                if (localScale.x > 0.0f)
                {
                    localScale.x *= -1.0f;
                    transform.localScale = localScale;
                }
                break;
            
            case PlayerStateController.playerStates.right:
                // pohneme objektem doprava
                transform.Translate(new Vector3(playerWalkSpeed * Time.deltaTime, 0.0f, 0.0f));
                // prohodime grafiku, aby se hrac koukal smerem pohybu
                if (localScale.x < 0.0f)
                {
                    localScale.x *= -1.0f;
                    transform.localScale = localScale;
                }

                break;

            case PlayerStateController.playerStates.jump:
                break;

            case PlayerStateController.playerStates.landing:
                break;

            case PlayerStateController.playerStates.falling:
                break;
                
            case PlayerStateController.playerStates.takingDMG:
                if (playerHealth <= 0)
                {
                    onStateChange(PlayerStateController.playerStates.kill);
                }
                else
                {
                    onStateChange(PlayerStateController.playerStates.immortal);
                }
                break;

            case PlayerStateController.playerStates.kill:
                //event - vysleme info o smrti hrace
                if (onDeadAction != null)
                    onDeadAction();
                onStateChange(PlayerStateController.playerStates.resurrect);
                break;

            case PlayerStateController.playerStates.resurrect:
                onStateChange(PlayerStateController.playerStates.idle);
             
                break;

            case PlayerStateController.playerStates.firingWeapon:
                break;
        }
    }

    // funkce reagujici na event zmeny stavu hrace.
    public void onStateChange(PlayerStateController.playerStates newState)
    {
        // kontorlujeme zda se novy stav nerovna staremu stavu, neni nutno provadet akci
        if (newState == currentState)
            return;

        // zkontrolujeme specialni podminky zmeny stavu
        if (checkIfAbortOnStateCondition(newState))
            return;


        // zkontrolujeme, jestli novy stav muze prepsat stav, ve kterem se nachazi hrac
        if (!checkForValidStatePair(newState))
            return;
        
        // muzeme bezpecne provezt akce podle noveho stavu
        switch (newState)
        {
            // nastavime promenne ovladajici animace pohybu
            case PlayerStateController.playerStates.idle:
                playerAnimator.SetBool("Walking", false);
                playerAnimator.SetBool("Hurted", false);
                break;

            case PlayerStateController.playerStates.left:
                playerAnimator.SetBool("Walking", true);
                break;

            case PlayerStateController.playerStates.right:
                playerAnimator.SetBool("Walking", true);
                break;

            case PlayerStateController.playerStates.jump:
                playerAnimator.SetBool("Grounded",false);
                if (playerHasLanded)
                {
                    // nastavime jumpDirection podle tooh, jestli se skace doprava nebo doleva
                    float jumpDirection = 0.0f;
                    if (currentState == PlayerStateController.playerStates.left)
                        jumpDirection = -1.0f;
                    else if (currentState == PlayerStateController.playerStates.right)
                        jumpDirection = 1.0f;
                    else
                        jumpDirection = 0.0f;

                    // aplikujeme jumpDirection jako silu pusobici na fyzicke telo hrace
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(jumpDirection * playerJumpForceHorizontal, playerJumpForceVertical));
                    // hrac je ve vzduchu
                    playerHasLanded = false;
                    // nemuzeme skakat znovu - vypneme stav - skok
                    PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.jump] = 0f;
                }
                break;

            // hrac pristal na zemi
            case PlayerStateController.playerStates.landing:
                playerAnimator.SetBool("Grounded",true);
                playerHasLanded = true;
                PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.jump] = Time.time + 0.1f;
                break;
            // hrac ve vzduchu
            case PlayerStateController.playerStates.falling:
                playerAnimator.SetBool("Grounded",false);
                PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.jump] = 0.0f;
                break;
            case PlayerStateController.playerStates.takingDMG:
                playerAnimator.SetBool("Hurted", true);
                playerHealth--;
                break;
            case PlayerStateController.playerStates.immortal:
                PlayerStateController.stateDelayTimer[(int) PlayerStateController.playerStates.takingDMG] =
                    Time.time + 0.5f;
                break;
            case PlayerStateController.playerStates.kill:
                break;
            // oziveni hrace na predem specifikovane pozici
            case PlayerStateController.playerStates.resurrect:
                playerHealth = PlayerHealthDefault;
                transform.position = playerRespawnPoint.transform.position;
                transform.rotation = Quaternion.identity;
                // vyresetujeme jeho pohyb
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                break;

            case PlayerStateController.playerStates.firingWeapon:
                playerAnimator.Play("PlayerFireAnim");
                // Vytvor bullet object
                GameObject newBullet = (GameObject)Instantiate(bulletPrefab);

                // urci pocatecni pozici
                newBullet.transform.position = bulletSpawnTransform.position;

                // ziskame z gameObjectu Bullet jeji komponentu - skript PlayerBulletController
                PlayerBulletController bullCon = newBullet.GetComponent<PlayerBulletController>();

                // priradime gameObject - player
                bullCon.playerObject = gameObject;

                // vypalime
                bullCon.launchBullet();

                // zmenime stav na puvodni
                onStateChange(currentState);

                PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.firingWeapon] = Time.time + 1f;
                
                break;
        }

        // ulozime predchozi stav
        previousState = currentState;

        // nastavime novy stav
        currentState = newState;
    }

    // porovname jestli muze novy stav prevzit ten stary
    bool checkForValidStatePair(PlayerStateController.playerStates newState)
    {
        bool returnVal = false;

        // porovnani
        switch (currentState)
        {
            case PlayerStateController.playerStates.idle:
                // vsechny stavy mohou prevzit nicnedelani
                returnVal = true;
                break;

            case PlayerStateController.playerStates.left:
                // vsechny stavy mohou prevzit pohyb vlevo
                returnVal = true;
                break;

            case PlayerStateController.playerStates.right:
                // vsechny stavy mohou prevzit pohyb vpravo
                returnVal = true;
                break;

            case PlayerStateController.playerStates.jump:
                // ve skoku muzeme strilet a pohybovat se vlevo, vpravo nebo zemrit
                if (
                    newState == PlayerStateController.playerStates.landing
                    || newState == PlayerStateController.playerStates.left
                    || newState == PlayerStateController.playerStates.right
                    || newState == PlayerStateController.playerStates.kill
                    || newState == PlayerStateController.playerStates.firingWeapon
                  )
                    returnVal = true;
                else
                    returnVal = false;
                break;

            case PlayerStateController.playerStates.landing:
                if (
                    newState == PlayerStateController.playerStates.left
                    || newState == PlayerStateController.playerStates.right
                    || newState == PlayerStateController.playerStates.idle
                    || newState == PlayerStateController.playerStates.firingWeapon
                  )
                    returnVal = true;
                else
                    returnVal = false;
                break;

            case PlayerStateController.playerStates.falling:
                // // muzeme i kotrolovat padani jako je u arkady typicke
                if (
                    newState == PlayerStateController.playerStates.landing
                    || newState == PlayerStateController.playerStates.left
                    || newState == PlayerStateController.playerStates.right
                    || newState == PlayerStateController.playerStates.kill
                    || newState == PlayerStateController.playerStates.firingWeapon
                  )
                    returnVal = true;
                else
                    returnVal = false;
                break;
            case PlayerStateController.playerStates.takingDMG:
                if (newState == PlayerStateController.playerStates.immortal ||
                    newState == PlayerStateController.playerStates.kill)
                {
                    returnVal = true;
                }
                else 
                    returnVal = false;
                break;
            case PlayerStateController.playerStates.immortal:
                // vsechny stavy krome smrti mohou prevzit nesmrtelnost
                if (newState == PlayerStateController.playerStates.idle)
                {
                    returnVal = true;
                }
                else
                    returnVal = false;
                break;
            case PlayerStateController.playerStates.kill:
                // po smrti se muzeme jenom obzivit
                if (newState == PlayerStateController.playerStates.resurrect)
                    returnVal = true;
                else
                    returnVal = false;
                break;

            case PlayerStateController.playerStates.resurrect:
                // po oziveni jsme ve stavu nicnedelani
                if (newState == PlayerStateController.playerStates.idle)
                    returnVal = true;
                else
                    returnVal = false;
                break;

            case PlayerStateController.playerStates.firingWeapon:
                returnVal = true;
                break;
        }
        return returnVal;
    }

    // zkontrolujeme specialni podminky, ktere by mohli zabranit zmene stavu
    private bool checkIfAbortOnStateCondition(PlayerStateController.playerStates newState)
    {
        bool returnVal = false;

        switch (newState)
        {
            case PlayerStateController.playerStates.idle:
                break;

            case PlayerStateController.playerStates.left:
                break;

            case PlayerStateController.playerStates.right:
                break;
             // mame prodlevu mezi skoky
            case PlayerStateController.playerStates.jump:
                float nextAllowedJumpTime = PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.jump];

                if (nextAllowedJumpTime == 0.0f || nextAllowedJumpTime > Time.time)
                    returnVal = true;
                break;

            case PlayerStateController.playerStates.landing:
                break;

            case PlayerStateController.playerStates.falling:
                break;
            case PlayerStateController.playerStates.takingDMG:
                if (PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.takingDMG] > Time.time)
                {
                    returnVal = true;
                }
                break;
            case PlayerStateController.playerStates.immortal:
                break;
            case PlayerStateController.playerStates.kill:
                break;

            case PlayerStateController.playerStates.resurrect:
                break;
            // mame prodlevy mezi strelami 
            case PlayerStateController.playerStates.firingWeapon:
                if (PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.firingWeapon] > Time.time)
                    returnVal = true;

                break;
        }

        // true - nedovol novy stav
        // false - v poradku, pokracuj
        return returnVal;
    }

    public void hitByCrusher()
    {
        onStateChange(PlayerStateController.playerStates.kill);
    }

}

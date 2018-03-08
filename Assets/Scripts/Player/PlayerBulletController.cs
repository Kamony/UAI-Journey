using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    [SerializeField] private GameObject playerObject = null;
    public GameObject PlayerObject { set { playerObject = value; } }
 
    [SerializeField] private float bulletSpeed = 15.0f;
    // ttl kulky :)
    [SerializeField] private float selfDestructTimer = 0.0f;
    
    // vypal kulku
    public void launchBullet()
    {
        // znamenko x-souradnice u local scale playera nam prozradi jakym smerem se kouka
        float mainXScale = playerObject.transform.localScale.x;
        
        Vector2 bulletForce;
        
        if (mainXScale < 0.0f)
        {
            // strilej vlevo
            bulletForce = new Vector2(bulletSpeed * -1.0f, 0.0f);
        }
        else
        {
            // strilej vpracvo
            bulletForce = new Vector2(bulletSpeed, 0.0f);
        }
        
        // pohneme s kulkou
        GetComponent<Rigidbody2D>().velocity = bulletForce;
        
        // pocitadlo - nastavime na 1 sec.
        selfDestructTimer = Time.time + 1.0f;
    }

    private void Update()
    {
        if (selfDestructTimer > 0.0f)
        {
            if (selfDestructTimer < Time.time)
            {
                Destroy(gameObject);
            }
        }
    }

}

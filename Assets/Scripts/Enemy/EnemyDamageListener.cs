using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageListener : MonoBehaviour
{

    [SerializeField] private Transform directionOfBeer;
    // delegat registrujici zasah kulkou
    
    public delegate void hitByPlayerBullet();
    public event hitByPlayerBullet hitByBullet;

    public  delegate void hitPlayer(int direction);
    public static event hitPlayer onHitAction;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // v pripade kolize s kulkou vysleme zpravu o zasahu
        if (other.CompareTag("PlayerBullet"))
        {
            if (hitByBullet != null)
            {
                hitByBullet();
                GetComponentInParent<AudioSource>().Play();
            }
        }
        if (other.CompareTag("Player"))
        {
            if (onHitAction != null)
            {
                int direction = directionOfBeer.localScale.x > 0 ? 1 : -1;
                onHitAction(direction);
            }   
        }
    }
    
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageListener : MonoBehaviour
{
    // delegat registrujici zasah kulkou
    public delegate void hitByPlayerBullet();

    public event hitByPlayerBullet hitByBullet;

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
    }
}
